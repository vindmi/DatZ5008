using System;
using log4net;
using GooglePlus.Data.Model;
using GooglePlus.Data.Contract;

namespace GooglePlus.Data.Managers
{
    public class UserManager
    {
        private static ILog log = log4net.LogManager.GetLogger(typeof(UserManager));

        private IObjectPersister<User, long> userPersister;

        public UserManager(IObjectPersister<User, long> userPersister)
        {
            this.userPersister = userPersister;
        }
        
        public void Save(User user)
        {
            log.Info("Saving user: " + user.GoogleId);

            Validate(user);
            
            try
            {
                userPersister.Write(user);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        private void Validate(User user)
        {

        }
    }
}
