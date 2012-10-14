using log4net;
using GooglePlus.Data.Managers;
using GooglePlus.Data.Model;
using System.Configuration;
using System;
using GooglePlus.ApiClient.Classes;
using Spring.Context;
using Spring.Context.Support;
using GooglePlus.ApiClient.Contract;
using GooglePlus.Main.Converters;
using GooglePlus.Main.Contract;
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

            importer.ImportData(new UserConverter(), new ActivityConverter());

            log.Debug("Main END");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
