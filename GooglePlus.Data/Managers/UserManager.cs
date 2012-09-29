using System;
using log4net;
using GooglePlus.Data.Model;

namespace GooglePlus.Data.Managers
{
    public class UserManager
    {
        private static ILog log = log4net.LogManager.GetLogger(typeof(UserManager));
        
        public void Save(User user)
        {
            log.Info("Saving user: " + user.GoogleId);
            
            using (GooglePlus ctx = new GooglePlus())
            {
                ctx.Users.Add(user);

                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                }
            }
        }
    }
}
