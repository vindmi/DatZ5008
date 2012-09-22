using System;
using GooglePlusData.Model;
using log4net;

namespace GooglePlusData.Managers
{
    public class UserManager
    {
        public void Save(User user)
        {
            using (GooglePlusPlus ctx = new GooglePlusPlus())
            {
                ctx.Users.Add(user);

                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
