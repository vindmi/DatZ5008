using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using GooglePlus.Data.Model;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;

namespace GooglePlus.Data.Services
{
    public class GooglePlusService
    {
        private static string googlePlusApiKey;
        private static string googlePlusApiGetPeopleUri;

        public GooglePlusService(string apikey)
        {
            googlePlusApiKey = apikey;
        }

        public User GetUserData(string userId, string googleApiUri)
        {
            googlePlusApiGetPeopleUri = googleApiUri;
            return GetGooglePlusApiData<User>(userId);
        }

        public static T GetGooglePlusApiData<T>(string userID)
        {
            string uri = string.Format(googlePlusApiGetPeopleUri + "/{0}?key={1}", userID, googlePlusApiKey);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            //return sr.ReadToEnd();

            var jsonSerializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(sr.ReadToEnd() as string));
            return (T)jsonSerializer.ReadObject(stream);
        }
    }
}
