﻿using System.Collections.Generic;
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

        public void DeleteUsers()
        {
            foreach (var usr in db.Users)
            {
                db.Users.Remove(usr);
            }

            db.SaveChanges();
        }

        public void DeleteActivities()
        {
            foreach (var ac in db.Activities)
            {
                db.Activities.Remove(ac);
            }

            db.SaveChanges();
        }

        public void SaveUser(User data)
        {
            User existingUser = GetUserById(data.Id);

            if (existingUser == null)
            {
                existingUser = GetUserByGoogleId(data.GoogleId);
            }

            if (existingUser != null)
            {
                existingUser.FirstName = data.FirstName;
                existingUser.Gender = data.Gender;
                existingUser.GoogleId = data.GoogleId;
                existingUser.LastName = data.LastName;
                existingUser.Username = data.Username;
            }
            else
            {
                db.Users.Add(data);
            }

            db.SaveChanges();
        }

        public User GetUserById(int key)
        {
            return db.Users.Find(key);
        }

        public User GetUserByGoogleId(string googleId)
        {
            return db.Users.FirstOrDefault(u => u.GoogleId == googleId);
        }

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

        public List<User> GetUsers()
        {
            return db.Users.ToList();
        }

        public List<Post> GetPosts(int userId)
        {
            return GetActivities<Post>(userId).ToList();
        }

        public List<Photo> GetPhotos(int userId)
        {
            return GetActivities<Photo>(userId).ToList();
        }

        public List<Share> GetShares(int userId)
        {
            return GetActivities<Share>(userId).ToList();
        }

        private IQueryable<T> GetActivities<T>(int userId)
            where T : Activity
        {
            return db.Activities
                .Include("Author")
                .OfType<T>()
                .Where(a => a.Author.Id == userId);
        }
    }
}
