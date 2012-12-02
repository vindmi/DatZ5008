using System.Collections.Generic;
using ServiceStack.Redis;
using log4net;
using GooglePlus.Data.Model;

namespace GooglePlus.Data.Managers
{
    public class RedisDataManager
    {
        private static ILog log = LogManager.GetLogger(typeof(RedisDataManager));

        private const string redisFeedKey = "feeds:usr:";
        private const string redisSubscribeKey = "subscribe:usr:";
        private readonly IRedisClient redisClient;

        public RedisDataManager(IRedisClient redisClient)
        {
            this.redisClient = redisClient;
        }

        #region Feeds

        public void AddFeed(Feed feed, int userId)
        {
            log.Info("Feed added for user: " + userId.ToString());

            redisClient.As<Feed>().Lists[redisFeedKey + userId.ToString()].Add(feed);
        }

        public List<Feed> GetFeeds(int userId)
        {
            log.Info("Get Feeds for user: " + userId.ToString());

            var list = redisClient.As<Feed>().Lists[redisFeedKey + userId.ToString()];

            return list.GetAll();
        }

        public void ClearData(int userId)
        {
            log.Info("Clear all Feeds for user: " + userId.ToString());
            
            var list = redisClient.As<Feed>().Lists[redisFeedKey + userId.ToString()];
            list.RemoveAll();
        }

        #endregion

        #region Subscriptions

        public void AddSubscription(int subscribeToUserId, int userId)
        {
            log.Info(string.Format("Subscription added for user {0} to user {1}", userId.ToString(), subscribeToUserId.ToString()));

            redisClient.As<int>().Lists[redisSubscribeKey + userId.ToString()].Add(subscribeToUserId);
        }

        public void DeleteSubscription(int subscribeToUserId, int userId)
        {
            log.Info(string.Format("User {0} unsubscribes from user {1}", userId.ToString(), subscribeToUserId.ToString()));

            var list = redisClient.As<int>().Lists[redisSubscribeKey + userId.ToString()];
            list.Remove(subscribeToUserId);
        }

        public List<int> GetSubscriptions(int userId)
        {
            log.Info("Get Subscriptions for user: " + userId.ToString());

            var list = redisClient.As<int>().Lists[redisSubscribeKey + userId.ToString()];

            return list.GetAll();
        }

        #endregion
    }
}
