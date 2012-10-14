using System;
using log4net;
using GooglePlus.Data.Model;
using GooglePlus.Data.Contract;

namespace GooglePlus.Data.Managers
{
    public class GoogleDataManager
    {
        private static ILog log = log4net.LogManager.GetLogger(typeof(GoogleDataManager));

        private IGoogleDataAdapter dataAdapter;

        public GoogleDataManager(IGoogleDataAdapter dataAdapter)
        {
            this.dataAdapter = dataAdapter;
        }
        
        public void SaveUser(User user)
        {
            log.Info("Saving user: " + user.GoogleId);

            ValidateUser(user);
            
            try
            {
                dataAdapter.SaveUser(user);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        private void ValidateUser(User user)
        {

        }

        public void SavePost(Post post)
        {
            log.Info("Saving post: " + post.googleId);

            try
            {
                dataAdapter.SaveActivity(post);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        public void SaveShare(Share share)
        {
            log.Info("Saving share: " + share.googleId);

            try
            {
                dataAdapter.SaveActivity(share);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        public void SavePhoto(Photo photo)
        {
            log.Info("Saving photo: " + photo.googleId);

            try
            {
                dataAdapter.SaveActivity(photo);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

    }
}
