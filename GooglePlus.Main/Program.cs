using log4net;
using System;
using Spring.Context.Support;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace GooglePlus.Main
{
    class Program
    {
        private static ILog log = log4net.LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            log.Debug("Main START");

            var ctx = ContextRegistry.GetContext();
            var importer = (UserImportDataProcessor)ctx.GetObject("IUserImportDataProcessor");

            importer.ImportData();

            log.Debug("Main END");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
