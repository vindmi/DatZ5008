using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GooglePlusData.Model;

namespace GooglePlusData.Managers
{
    public class UserManager
    {
        public void Save(User user)
        {
            using (GooglePlusPlus ctx = new GooglePlusPlus())
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();
            }
        }
    }
}
