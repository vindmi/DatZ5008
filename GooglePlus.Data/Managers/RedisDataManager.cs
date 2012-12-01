using System.Collections.Generic;
using ServiceStack.Redis;
using log4net;
using GooglePlus.Data.Model;

namespace GooglePlus.Data.Managers
{
    public class RedisDataManager
    {
        private static ILog log = LogManager.GetLogger(typeof(RedisDataManager));

        private const string redisKey = "feeds:usr:";
        private readonly IRedisClient redisClient;

        public RedisDataManager(IRedisClient redisClient)
        {
            this.redisClient = redisClient;
        }

        public void AddFeed(Feed feed, int userId)
        {
            log.Info("Feed added for user: " + userId.ToString());

            redisClient.As<Feed>().Lists[redisKey + userId.ToString()].Add(feed);
        }

        public List<Feed> GetFeeds(int userId)
        {
            log.Info("Get Feeds for user: " + userId.ToString());

            var list = redisClient.As<Feed>().Lists[redisKey + userId.ToString()];

            return list.GetAll();
        }

        public void ClearData(int userId)
        {
            log.Info("Clear all Feeds for user: " + userId.ToString());
            
            var list = redisClient.As<Feed>().Lists[redisKey + userId.ToString()];
            list.RemoveAll();
        }
    }
}
