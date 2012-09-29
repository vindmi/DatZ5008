using log4net;
using GooglePlus.Data.Managers;
using GooglePlus.Data.Model;
using GooglePlus.ApiClient;
using System.Configuration;
using System;

namespace GooglePlus.Main
{
    class Program
    {
        private static ILog log = log4net.LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            log.Debug("Main START");
            
            LoadUsersFromGooglePlus();

            log.Debug("Main END");
        }

        private static void LoadUsersFromGooglePlus()
        {
            log.Debug("Load from GooglePlus started");

            string apiKey = ConfigurationManager.AppSettings["googlePlusApiKey"];
            string uri = ConfigurationManager.AppSettings["googlePlusApiGetPeopleUri"];
            var userIDs = ConfigurationManager.AppSettings["googlePlusUserIDs"];
            var googleService = new GooglePlusApiClient(apiKey);
            var users = userIDs.Split(',');

            foreach (string u in users)
            {
                try
                {
                    var user = googleService.GetUserData(u, uri);
                    new UserManager().Save(user);
                }
                catch (Exception)
                {
                    //log.Error(ex.Message, ex);
                }
            }

            log.Debug("Load from GooglePlus finished");
        }      
    }
}
