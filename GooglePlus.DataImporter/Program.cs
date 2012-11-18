using log4net;
using System;
using Spring.Context.Support;
using GooglePlus.Data;
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


            importer.ImportData();

            log.Debug("Main END");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
