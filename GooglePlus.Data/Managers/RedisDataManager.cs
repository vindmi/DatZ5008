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
        private readonly RedisClient redisClient;

        public RedisDataManager()
        {
            redisClient = new RedisClient("localhost");
        }

        public void AddFeed(Feed feed, string userId)
        {
            var list = redisClient.As<Feed>().Lists[redisKey + userId];
            list.Add(feed);
        }

        public List<Feed> GetFeeds(string userId)
        {
            var list = redisClient.As<Feed>().Lists[redisKey + userId];
            return list.GetAll();
        }

        public void ClearData(string userId)
        {
            var list = redisClient.As<Feed>().Lists[redisKey + userId];
            list.RemoveAll();
        }
    }
}
