using log4net;
using GooglePlus.Data.Managers;
using GooglePlus.Data.Model;
using System.Configuration;
using System;
using GooglePlus.ApiClient.Classes;
using Spring.Context;
using Spring.Context.Support;
using GooglePlus.ApiClient.Contract;
using GooglePlus.Main.Contract;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace GooglePlus.Main
{
    class Program
    {
        private static ILog log = log4net.LogManager.GetLogger(typeof(Program));

        static IApplicationContext ctx = ContextRegistry.GetContext();

        static void Main(string[] args)
        {
            log.Debug("Main START");
            
            LoadUsersFromGooglePlus();

            log.Debug("Main END");
        }

        private static void LoadUsersFromGooglePlus()
        {
            log.Debug("Load from GooglePlus started");

            var googleService = (IGooglePlusPeopleProvider)ctx.GetObject("IGooglePlusPeopleProvider");
            UserManager userManager = (UserManager)ctx.GetObject("IUserManager");
            
            //get initial user ids
            var userIdStore = (IUserIdStore)ctx.GetObject("IUserIdStore");
            var users = userIdStore.UserIds.Split(',');

            UserConverter userConverter = new UserConverter();
            foreach (string userId in users)
            {
                try
                {
                    //get user data from google
                    var googleUser = googleService.GetProfile(userId);
                    //convert user
                    User user = userConverter.Convert(googleUser);
                    //save user on database
                    userManager.Save(user);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                }
            }

            log.Debug("Load from GooglePlus finished");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }      
    }
}
