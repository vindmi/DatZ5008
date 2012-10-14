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

        static IApplicationContext ctx = ContextRegistry.GetContext();
        
        private static GoogleDataManager dataManager;

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
            dataManager = (GoogleDataManager)ctx.GetObject("IUserManager");
            
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
                    dataManager.SaveUser(user);
                    //load user activities
                    LoadActivities(userId, user);
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

        private static void LoadActivities(string userId, User user)
        {
            log.Debug(string.Format("Activities load from GooglePlus started for user: {0}", userId));

            var googleService = (IGooglePlusActivitiesProvider)ctx.GetObject("IGooglePlusActivitiesProvider");
            
            ActivityConverter actConverter = new ActivityConverter();
            UserConverter userConverter = new UserConverter();

            try
            {
                var activities = googleService.GetActivities(userId);
                foreach (var item in activities.Items)
                {
                    var activity = actConverter.ConvertActivity(item, user);
                    
                    if (item.GoogleObject != null)
                    {
                        switch (item.Verb)
                        {
                            case "post":
                                var post = actConverter.ConvertPost(item.GoogleObject, activity);
                                dataManager.SavePost(post);
                                break;
                            case "share":
                                var share = actConverter.ConvertShare(item.GoogleObject, activity);
                                dataManager.SaveShare(share);
                                break;
                        }
                        //look for photos
                        if (item.GoogleObject.Attachments != null)
                        {
                            foreach (var attachment in item.GoogleObject.Attachments)
                            {
                                if (attachment.ObjectType.Equals("photo"))
                                {
                                    var photo = actConverter.ConvertPhoto(attachment, activity);
                                    //dataManager.SavePhoto(photo);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }

            log.Debug("Activities load from GooglePlus finished");
        }
    }
}
