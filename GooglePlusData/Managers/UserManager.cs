using System;
using log4net;
using GooglePlus.Data.Model;

namespace GooglePlus.Data.Managers
{
    public class UserManager
    {
        private static ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public void Save(User user)
        {
            Log.Info("Saving user");
            
            using (GooglePlus ctx = new GooglePlus())
            {
                ctx.Users.Add(user);

                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                }
            }
        }
    }
}
