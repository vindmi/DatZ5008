using System;
using System.Collections.Generic;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(UserImportDataProcessor));

        private readonly IGooglePlusPeopleProvider peopleProvider;
        private readonly IGooglePlusActivitiesProvider activitiesProvider;
        private readonly GoogleDataManager dataManager;
        private readonly IUserIdStore userIdStore;

        private readonly UserConverter userConverter;
        private readonly ActivityConverter activityConverter;
        private readonly Lazy<RedisDataManager> redisDataManager = new Lazy<RedisDataManager>(() => new RedisDataManager());

        public bool IsClearDatabaseRequired { get; set; }
        public bool IsFeedSavingEnabled { get; set; }

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

            userConverter = new UserConverter();
            activityConverter = new ActivityConverter();
        }

        public void ImportData()
        {
            log.Debug("Load from GooglePlus started");

            if (IsClearDatabaseRequired)
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

                    if (IsFeedSavingEnabled)
                    {
                        SaveFeeds(userId);
                        //get list of feeds, only for checking!!
                        var list = redisDataManager.Value.GetFeeds(userId);

                        log.Debug("Saved feeds " + list.Count);
                    }
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
            if (activities == null || activities.Count == 0)
            {
                return;
            }

            foreach (Activity ac in activities)
            {
                AddFeed(ac);
            }
        }

        private void AddFeed(Activity activity)
        {
            FeedType type;

            if (activity is Post)
            {
                type = FeedType.POST;
            }
            else if (activity is Share)
            {
                type = FeedType.SHARE;
            }
            else if (activity is Photo)
            {
                type = FeedType.PHOTO;
            }
            else
            {
                log.Error("Unable to recognize FeedType: " + activity.GetType() + ". Skipping this activity...");
                throw new ArgumentException();
            }

            Feed feed = new Feed
            {
                ReferenceId = activity.Id,
                Type = type,
                CreatedDate = activity.Created
            };

            redisDataManager.Value.AddFeed(feed, activity.Author.GoogleId);
        }
    }
}
