using System.Collections.Generic;
using GooglePlus.Data.Model;

namespace GooglePlus.Data.Contract
{
    public interface IGoogleDataAdapter
    {
        void SaveUser(User user);

        User GetUserById(int userId);

        User GetUserByGoogleId(string googleId);

        void SaveActivity(Activity activity);

        Activity GetActivityById(long activityId);

        Activity GetActivityByGoogleId(string googleId);

        List<Activity> GetUserActivities(string userId);

        List<User> GetUsers();

        List<Post> GetPosts(int userId);

        List<Photo> GetPhotos(int userId);

        List<Share> GetShares(int userId);

        void DeleteUsers();

        void DeleteActivities();
    }
}
