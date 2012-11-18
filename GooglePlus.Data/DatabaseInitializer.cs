using log4net;

namespace GooglePlus.Data
{
    public static class DatabaseInitializer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DatabaseInitializer));

        public static void EnsureDatabase()
        {
            using (var db = new GooglePlus())
            {
                log.Debug("Ensuring database");

                db.Database.CreateIfNotExists();
            }
        }
    }
}
