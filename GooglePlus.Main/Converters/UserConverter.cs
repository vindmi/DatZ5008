using System;
using GooglePlus.Data.Model;
using GooglePlus.ApiClient.Classes;

namespace GooglePlus.Main.Converters
{
    public class UserConverter
    {
        public User Convert(GooglePlusUser googleUser)
        {
            return new User
            {
                FirstName = googleUser.Name.GivenName,
                LastName = googleUser.Name.FamilyName,
                Username = string.Format("{0}_{1}", googleUser.Name.GivenName,googleUser.Name.FamilyName).ToLower(),
                GoogleId = googleUser.Id,
                Password = Guid.NewGuid().ToString()
            };
        }
    }
}
