using log4net;
using GooglePlus.Data.Managers;
using GooglePlus.Data.Model;
using GooglePlus.Data.Services;
using System.Configuration;

namespace GooglePlus.Main
{
    class Program
    {
        private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            log.Debug("Main START");

            var usr = new User();
            usr.FirstName = "a";
            usr.LastName = "b";

            new UserManager().Save(usr);

            log.Debug("Main END");
        }

        private void LoadUsersFromGooglePlus()
        {
            string apiKey = ConfigurationManager.AppSettings["googlePlusApiKey"];
            string uri = ConfigurationManager.AppSettings["googlePlusApiGetPeopleUri"];

            var googleService = new GooglePlusService(apiKey);
            var userIDs = ConfigurationManager.AppSettings["googlePlusUserIDs"];
            var users = userIDs.Split(',');

            foreach (string u in users)
            {
                googleService.GetUserData(u, uri);
            }
        }
    }
}
