using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GooglePlus.Data.Model;
using GooglePlus.ApiClient.Classes;

namespace GooglePlus.Main
{
    public class UserConverter
    {
        public User Convert(GooglePlusUser googleUser)
        {
            return new User
            {
                FirstName = googleUser.name.givenName,
                LastName = googleUser.name.familyName,
                Username = googleUser.displayName,
                GoogleId = googleUser.id
            };
        }
    }
}
