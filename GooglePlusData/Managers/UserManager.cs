using System;
using GooglePlusData.Model;
using log4net;

namespace GooglePlusData.Managers
{
    public class UserManager
    {
        private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public void Save(User user)
        {
            log.Info("Saving user");
            
            using (GooglePlusPlus ctx = new GooglePlusPlus())
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
