using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GooglePlus.Data.Model;

namespace GooglePlus.Data.Contract
{
    public interface IGoogleDataAdapter
    {
        void SaveUser(User user);

        User GetUserById(long userId);

        User GetUserByGoogleId(string googleId);

        void SaveActivity(Activity activity);

        Activity GetActivityById(long activityId);

        Activity GetActivityByGoogleId(string googleId);

        List<Activity> GetUserActivities(string userId);
    }
}
