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
        private readonly RedisDataManager redisDataManager;
        private readonly IUserIdStore userIdStore;

        private readonly UserConverter userConverter;
        private readonly ActivityConverter activityConverter;

        public bool IsClearDatabaseRequired { get; set; }
        public bool IsFeedSavingEnabled { get; set; }

        public UserImportDataProcessor(
            IGooglePlusPeopleProvider peopleProvider,
            IGooglePlusActivitiesProvider activitiesProvider,
            GoogleDataManager dataManager,
            RedisDataManager redisDataManager,
            IUserIdStore userIdStore)
        {
            this.peopleProvider = peopleProvider;
            this.activitiesProvider = activitiesProvider;
            this.dataManager = dataManager;
            this.redisDataManager = redisDataManager;
            this.userIdStore = userIdStore;

            userConverter = new UserConverter();
            activityConverter = new ActivityConverter();
        }

        private void ClearDatabase()
        {
            dataManager.DeleteActivities();
            dataManager.DeleteUsers();
        }
        
        public void ImportData()
        {
            log.Debug("Load from GooglePlus started");

            if (IsClearDatabaseRequired)
            {
                ClearDatabase();
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
                        ImportFeeds(userId);
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
                if (item.GoogleObject == null)
                {
                    continue;
                }

                //if google activity already added continue on next activity
                Activity googleActivity = dataManager.GetActivityByGoogleId(item.Id);
                if (googleActivity != null)
                    continue;

                Activity activity;

                switch (item.Verb)
                {
                    case "post":
                        activity = activityConverter.ConvertToPost(item, item.GoogleObject);
                        break;
                    case "share":
                        activity = activityConverter.ConvertToShare(item, item.GoogleObject);
                        break;
                    default:
                        continue;
                }

                activity.Author = user;

                dataManager.SaveActivity(activity);

                //look for photos
                if (item.GoogleObject.Attachments != null)
                {
                    foreach (var attachment in item.GoogleObject.Attachments)
                    {
                        if (attachment.ObjectType.Equals("photo"))
                        {
                            var photo = activityConverter.ConvertToPhoto(item, attachment);
                            photo.Author = user;
                            dataManager.SaveActivity(photo);
                        }
                    }
                }               
            }

            log.Debug("Activities load from GooglePlus finished");
        }

        private void ImportFeeds(string userId)
        {
            if (IsClearDatabaseRequired)
            {
                redisDataManager.ClearData(userId);
            }

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

            redisDataManager.AddFeed(feed, activity.Author.GoogleId);
        }
    }
}
