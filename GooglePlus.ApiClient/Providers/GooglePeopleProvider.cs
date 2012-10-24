using System;
using GooglePlus.ApiClient.Classes;
using GooglePlus.ApiClient.Contract;

namespace GooglePlus.ApiClient.Providers
{
    public class GooglePeopleProvider : AbstractGoogleDataProvider, IGooglePlusPeopleProvider
    {
        public GooglePlusUser GetProfile(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }

            if (String.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("userId");
            }

            string uri = String.Format(Uri + "/{0}?key={1}", userId, ApiKey);

            return JsonDataProvider.GetData<GooglePlusUser>(uri);
        }
    }
}
