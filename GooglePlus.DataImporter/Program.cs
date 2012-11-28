using log4net;
using System;
using Spring.Context.Support;
using GooglePlus.Data;
using System.Configuration;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace GooglePlus.DataImporter
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            log.Debug("Main START");
            var ctx = ContextRegistry.GetContext();
            var importer = (UserImportDataProcessor)ctx.GetObject("IUserImportDataProcessor");

            try
            {
                DatabaseInitializer.EnsureDatabase();
            }
            catch
            {
                log.Fatal("Could not ensure database existance");
                return;
            }

            var userIds = ConfigurationManager.AppSettings["userIds"];

            importer.ImportData(userIds.Split(','));

            log.Debug("Main END");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
