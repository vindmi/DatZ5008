using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GooglePlus.Data.Contract;
using GooglePlus.Data.Model;

namespace GooglePlus.Data.Persisters
{
    public class UserPersister : IObjectPersister<User, long>
    {
        private GooglePlus db;

        public UserPersister()
        {
            db = new GooglePlus();
        }

        public void Write(User data)
        {
            User existingUser = Read(data.Id);

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

        public User Read(long key)
        {
            return db.Users.Find(key);
        }
    }
}
