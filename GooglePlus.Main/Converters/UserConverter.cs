using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                FirstName = googleUser.name.givenName,
                LastName = googleUser.name.familyName,
                Username = string.Format("{0}_{1}", googleUser.name.givenName,googleUser.name.familyName).ToLower(),
                GoogleId = googleUser.id,
                Password = Guid.NewGuid().ToString()
            };
        }
    }
}
