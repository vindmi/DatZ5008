using System;
using GooglePlus.Data.Model;
using GooglePlus.ApiClient.Classes;

namespace GooglePlus.DataImporter
{
    public class UserMapper
    {
        public User CreateFrom(GooglePlusUser googleUser)
        {
            var user = new User();

            Map(googleUser, user);

            return user;
        }

        public void Map(GooglePlusUser googleUser, User user)
        {
            if (googleUser.Name != null)
            {
                user.FirstName = googleUser.Name.GivenName;
                user.LastName = googleUser.Name.FamilyName;
                user.GoogleId = googleUser.Id;
            }
        }
    }
}
