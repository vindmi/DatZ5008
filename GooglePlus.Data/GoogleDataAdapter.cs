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
            throw new NotImplementedException();
        }
    }
}
