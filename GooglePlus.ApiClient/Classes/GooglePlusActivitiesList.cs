using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GooglePlus.ApiClient.Classes
{
    public class GooglePlusActivitiesList
    {
        public string id { get; set; }

        public string title { get; set; }

        public string nextPageToken { get; set; }

        //public DateTime updated { get; set; }

        public List<GooglePlusActivity> items { get; set; }
    }
}
