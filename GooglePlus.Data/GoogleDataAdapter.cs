using System.Collections.Generic;
using System.Linq;
using GooglePlus.Data.Contract;
using GooglePlus.Data.Model;

namespace GooglePlus.Data
{
    public class GoogleDataAdapter : IGoogleDataAdapter
    {
        private readonly GooglePlus db;

        public GoogleDataAdapter()
        {
            db = new GooglePlus();
        }

        #region User

        public void SaveUser(User data)
        {
            User existingUser = GetUserById(data.Id);

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
            Activity activity = GetActivityById(data.Id);

            if (activity == null)
            {
                db.Activities.Add(data);
                db.SaveChanges();
            }
        }

        public Activity GetActivityById(long key)
        {
            return db.Activities.Find(key);
        }

        public Activity GetActivityByGoogleId(string googleId)
        {
            return db.Activities.FirstOrDefault(a => a.googleId == googleId);
        }

        public List<Activity> GetUserActivities(string userId)
        {
            return db.Activities
                .Include("Author")
                .Where(a => a.Author.GoogleId == userId)
                .ToList();
        }

        #endregion
    }
}
