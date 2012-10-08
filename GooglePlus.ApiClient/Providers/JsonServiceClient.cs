using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using GooglePlus.ApiClient.Contract;

namespace GooglePlus.ApiClient.Providers
{
    public class JsonServiceClient : IRestServiceClient
    {
        public T GetData<T>(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            var jsonSerializer = new DataContractJsonSerializer(typeof(T));

            using(StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(sr.ReadToEnd())))
                {
                    return (T)jsonSerializer.ReadObject(stream);
                }
            }
        }
    }
}
