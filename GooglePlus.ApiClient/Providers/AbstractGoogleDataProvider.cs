using System.Collections.Generic;
using GooglePlus.ApiClient.Contract;

namespace GooglePlus.ApiClient.Providers
{
    public abstract class AbstractGoogleDataProvider
    {
        public IJsonServiceClient JsonDataProvider { protected get; set; }

        public string ApiKey
        {
            protected get;
            set;
        }

        public string Uri
        {
            protected get;
            set;
        }
    }
}
