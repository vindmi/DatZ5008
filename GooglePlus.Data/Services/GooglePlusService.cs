using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using GooglePlus.Data.Model;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using GooglePlus.Data.Classes;

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
            var googleuser = GetGooglePlusApiData<GooglePlusUser>(userId);
            return ConverUser(googleuser);
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

        private static User ConverUser(GooglePlusUser googleuser)
        {
            var user = new User();

            user.GoogleId = googleuser.id;
            user.FirstName = googleuser.name.givenName;
            user.LastName = googleuser.name.familyName;
            user.Username = string.Format("{0}_{1}", user.FirstName, user.LastName).ToLower();
            user.Password = ""; //TO DO: generate password

            return user;
        }
    }
}
