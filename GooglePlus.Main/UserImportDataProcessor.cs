using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Context.Support;
using GooglePlus.ApiClient.Contract;
using GooglePlus.Data.Managers;
using log4net;
using GooglePlus.Main.Contract;
using GooglePlus.Main.Converters;
using GooglePlus.Data.Model;

namespace GooglePlus.Main
{
    public class UserImportDataProcessor
    {
        private static ILog log = log4net.LogManager.GetLogger(typeof(UserImportDataProcessor));

        private IGooglePlusPeopleProvider peopleProvider;
        private IGooglePlusActivitiesProvider activitiesProvider;
        private GoogleDataManager dataManager;
        private RedisDataManager redisDataManager;
        private IUserIdStore userIdStore;

        private UserConverter userConverter;
        private ActivityConverter activityConverter;

        public bool ClearDatabase { get; private set; }

        public UserImportDataProcessor(
            IGooglePlusPeopleProvider peopleProvider,
            IGooglePlusActivitiesProvider activitiesProvider,
            GoogleDataManager dataManager,
            IUserIdStore userIdStore)
        {
            this.peopleProvider = peopleProvider;
            this.activitiesProvider = activitiesProvider;
            this.dataManager = dataManager;
            this.userIdStore = userIdStore;

            this.userConverter = new UserConverter();
            this.activityConverter = new ActivityConverter();

            this.redisDataManager = new RedisDataManager();
        }

        public void ImportData()
        {
            log.Debug("Load from GooglePlus started");

            if (ClearDatabase)
            {
                //TODO: clear database
            }

            var users = userIdStore.UserIds.Split(',');

            foreach (string userId in users)
            {
                try
                {
                    //get user data from google
                    var googleUser = peopleProvider.GetProfile(userId);
                    //convert user
                    User user = userConverter.Convert(googleUser);
                    //save user on database
                    dataManager.SaveUser(user);
                    //load user activities
                    LoadActivities(userId, user);
                    //save Feeds on Redis DB
                    SaveFeeds(userId);

                    //get list of feeds, only for checking!!
                    var list = redisDataManager.GetFeeds(userId);
                    log.Debug("Saved feeds " + list.Count);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                }
            }

            log.Debug("Load from GooglePlus finished");
        }

        private void LoadActivities(string userId, User user)
        {
            log.Debug(string.Format("Activities load from GooglePlus started for user: {0}", userId));

            var activities = activitiesProvider.GetActivities(userId);

            foreach (var item in activities.Items)
            {
                var activity = activityConverter.ConvertActivity(item, user);

                if (item.GoogleObject == null)
                {
                    continue;
                }

                switch (item.Verb)
                {
                    case "post":
                        var post = activityConverter.ConvertPost(item.GoogleObject, activity);
                        dataManager.SavePost(post);
                        break;
                    case "share":
                        var share = activityConverter.ConvertShare(item.GoogleObject, activity);
                        dataManager.SaveShare(share);
                        break;
                }

                //look for photos
                if (item.GoogleObject.Attachments != null)
                {
                    foreach (var attachment in item.GoogleObject.Attachments)
                    {
                        if (attachment.ObjectType.Equals("photo"))
                        {
                            var photo = activityConverter.ConvertPhoto(attachment, activity);
                            dataManager.SavePhoto(photo);
                        }
                    }
                }               
            }

            log.Debug("Activities load from GooglePlus finished");
        }

        private void SaveFeeds(string userId)
        {
            List<Activity> activities = dataManager.GetActivities(userId);
            if (activities != null)
            {
                if (activities.Count == 0)
                    return;

                foreach (Activity ac in activities)
                {
                    AddFeed(ac);
                }
            }
        }

        private void AddFeed(Activity activity)
        {
            FeedType type;
            switch (activity.GetType().Name)
            {
                case "Post":
                    type= FeedType.POST;
                    break;
                case "Share":
                    type= FeedType.SHARE;
                    break;
                case "Photo":
                    type= FeedType.PHOTO;
                    break;
                default:
                    log.Error("Unable to recognize FeedType: " + activity.GetType().ToString() + ". Skipping this activity...");
                    return;
            }

            Feed feed = new Feed
            {
                ReferenceId = activity.Id,
                Type = type,
                CreatedDate = activity.Created
            };

            redisDataManager.AddFeed(feed, activity.Author.GoogleId);
        }
    }
}
