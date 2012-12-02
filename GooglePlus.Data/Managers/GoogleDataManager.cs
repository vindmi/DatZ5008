﻿using System;
using System.Linq;
using log4net;
using GooglePlus.Data.Model;
using GooglePlus.Data.Contract;
using System.Collections.Generic;

namespace GooglePlus.Data.Managers
{
    public class GoogleDataManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GoogleDataManager));

        private readonly IGoogleDataAdapter dataAdapter;

        public GoogleDataManager(IGoogleDataAdapter dataAdapter)
        {
            this.dataAdapter = dataAdapter;
        }
        
        public void SaveUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            log.Info("Saving user: " + user.GoogleId);
            
            try
            {
                dataAdapter.SaveUser(user);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw;
            }
        }

        public User GetUser(string googleId)
        {
            return dataAdapter.GetUserByGoogleId(googleId);
        }

        public void SaveActivity(Activity activity)
        {
            if (activity == null)
            {
                throw new ArgumentNullException();
            }

            log.Info("Saving " + activity.GetType().Name + ":" + activity.googleId);

            try
            {
                dataAdapter.SaveActivity(activity);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw;
            }
        }

        public List<Activity> GetActivities(int userId)
        {
            log.Info("Called GetActivities: " + userId);

            try
            {
                return dataAdapter.GetUserActivities(userId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw;
            }
        }

        public Activity GetActivityByGoogleId(string googleId)
        {
            return dataAdapter.GetActivityByGoogleId(googleId);
        }

        public void DeleteImportedData()
        {
            dataAdapter.DeleteUsers(dataAdapter.GetUsers().Where(u => String.IsNullOrEmpty(u.Username)));
        }
    }
}
