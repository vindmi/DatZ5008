using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GooglePlus.ApiClient.Classes
{
    [DataContract]
    public class GooglePlusActivitiesList
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "nextPageToken")]
        public string NextPageToken { get; set; }

        [DataMember(Name = "updated")]
        public string Updated { get; set; }

        [DataMember(Name = "items")]
        public List<GooglePlusActivity> Items { get; set; }
    }
}
