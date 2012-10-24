using System;
using GooglePlus.ApiClient.Classes;
using GooglePlus.ApiClient.Contract;

namespace GooglePlus.ApiClient.Providers
{
    public class GoogleActivitiesProvider : AbstractGoogleDataProvider, IGooglePlusActivitiesProvider
    {
        public GooglePlusActivitiesList GetActivities(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }

            if (String.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("userId");
            }

            string uri = String.Format(Uri + "/{0}/activities/public?key={1}", userId, ApiKey);

            return JsonDataProvider.GetData<GooglePlusActivitiesList>(uri);
        }
    }
}
