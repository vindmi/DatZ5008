using System;
using log4net;
using GooglePlus.Data.Model;
using GooglePlus.Data.Contract;

namespace GooglePlus.Data.Managers
{
    public class UserManager
    {
        private static ILog log = log4net.LogManager.GetLogger(typeof(UserManager));

        private IGoogleDataAdapter dataAdapter;

        public UserManager(IGoogleDataAdapter dataAdapter)
        {
            this.dataAdapter = dataAdapter;
        }
        
        public void Save(User user)
        {
            log.Info("Saving user: " + user.GoogleId);

            Validate(user);
            
            try
            {
                dataAdapter.SaveUser(user);
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
