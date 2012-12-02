using System;
using System.Collections.Generic;
using System.Linq;
using GooglePlus.ApiClient.Contract;
using GooglePlus.Data.Managers;
using log4net;
using GooglePlus.Data.Model;

namespace GooglePlus.DataImporter
{
    public class UserImportDataProcessor
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserImportDataProcessor));

        private readonly IGooglePlusPeopleProvider peopleProvider;
        private readonly IGooglePlusActivitiesProvider activitiesProvider;
        private readonly GoogleDataManager dataManager;
        private readonly RedisDataManager redisDataManager;

        private readonly UserMapper userMapper;
        private readonly ActivityMapper activityMapper;

        public bool IsClearDatabaseRequired { get; set; }
        public bool IsFeedSavingEnabled { get; set; }

        public UserImportDataProcessor(
            IGooglePlusPeopleProvider peopleProvider,
            IGooglePlusActivitiesProvider activitiesProvider,
            GoogleDataManager dataManager,
            RedisDataManager redisDataManager)
        {
            this.peopleProvider = peopleProvider;
            this.activitiesProvider = activitiesProvider;
            this.dataManager = dataManager;
            this.redisDataManager = redisDataManager;

            userMapper = new UserMapper();
            activityMapper = new ActivityMapper();
        }

        public void ImportData(string googleId, User user = null)
        {
            try
            {
                //get user data from google
                var googleUser = peopleProvider.GetProfile(googleId);

                if (user == null)
                {
                    user = userMapper.CreateFrom(googleUser);
                }
                else
                {
                    userMapper.Map(googleUser, user);
                }

                dataManager.SaveUser(user);

                LoadActivities(googleId, user);

                if (IsFeedSavingEnabled)
                {
                    ImportFeeds(user.Id);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        public void ImportData(string[] users)
        {
            log.Debug("Load from GooglePlus started");

            if (IsClearDatabaseRequired)
            {
                dataManager.DeleteImportedData();
            }

            foreach (string userId in users)
            {
                ImportData(userId);
            }

            log.Debug("Load from GooglePlus finished");
        }

        private void LoadActivities(string userId, User user)
        {
            log.Debug(string.Format("Activities load from GooglePlus started for user: {0}", userId));

            var activities = activitiesProvider.GetActivities(userId);

            var existingActivities = dataManager.GetActivities(user.Id);

            foreach (var item in activities.Items)
            {
                //if google activity already added continue on next activity
                if (item.GoogleObject == null
                    || existingActivities.Any(a => String.Equals(a.googleId, item.Id)))
                {
                    continue;
                }

                Activity activity;

                switch (item.Verb)
                {
                    case "post":
                        activity = activityMapper.CreatePost(item, item.GoogleObject);
                        break;
                    case "share":
                        activity = activityMapper.CreateShare(item, item.GoogleObject);
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
                            var photo = activityMapper.CreatePhoto(item, attachment);
                            photo.Author = user;
                            dataManager.SaveActivity(photo);
                        }
                    }
                }               
            }

            log.Debug("Activities load from GooglePlus finished");
        }

        private void ImportFeeds(int userId)
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
                AddRedisFeed(ac);
            }
        }

        public void AddRedisFeed(Activity activity)
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

            redisDataManager.AddFeed(feed, activity.Author.Id);
        }
    }
}
