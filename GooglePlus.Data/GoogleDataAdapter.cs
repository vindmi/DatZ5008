using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GooglePlus.Data.Contract;
using GooglePlus.Data.Model;

namespace GooglePlus.Data
{
    public class GoogleDataAdapter : IGoogleDataAdapter
    {
        private GooglePlus db;

        public GoogleDataAdapter()
        {
            db = new GooglePlus();
        }

        #region User

        public void SaveUser(User data)
        {
            User existingUser = GetUserByGoogleId(data.GoogleId);

            if (existingUser != null)
            {
                existingUser.FirstName = data.FirstName;
                existingUser.Gender = data.Gender;
                existingUser.GoogleId = data.GoogleId;
                existingUser.LastName = data.LastName;
                existingUser.Password = data.Password;
                existingUser.Username = data.Username;
            }
            else
            {
                db.Users.Add(data);
            }

            db.SaveChanges();
        }

        public User GetUserById(long key)
        {
            return db.Users.Find(key);
        }

        public User GetUserByGoogleId(string googleId)
        {
            return db.Users.FirstOrDefault(u => u.GoogleId == googleId);
        }

        #endregion

        #region Activity

        public void SaveActivity(Activity data)
        {
            Activity activity = GetActivityByGoogleId(data.googleId);

            if (activity == null)
            {
                db.Activities.Add(data);
                db.SaveChanges();
            }
        }

        public Activity GetActivityByGoogleId(string googleId)
        {
            return db.Activities.FirstOrDefault(a => a.googleId == googleId);
        }

        #endregion
    }
}
