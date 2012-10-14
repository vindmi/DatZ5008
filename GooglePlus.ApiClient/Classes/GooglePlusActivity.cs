using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GooglePlus.ApiClient.Classes
{
    public class GooglePlusActivity
    {
        public string id { get; set; }

        public GooglePlusUser actor { get; set; }

        public string title { get; set; }

       // public DateTime published { get; set; }

        //public DateTime updated { get; set; }

        public string url { get; set; }

        public string verb { get; set; }

        public string annotation { get; set; }

        public GooglePlusObject @object { get; set; }
    }

    public class GooglePlusObject
    {
        public string id { get; set; }

        public string objectType { get; set; }

        public string content { get; set; }

        public string url { get; set; }

        public List<GooglePlusAttachment> attachments { get; set; }
    }

    public class GooglePlusAttachment
    {
        public string id { get; set; }

        public string content { get; set; }

        public string url { get; set; }

        public GooglePlusImage fullImage { get; set; }

        public string objectType { get; set; }

        public string displayName { get; set; }
    }

    public class GooglePlusImage
    {
        public string url { get; set; }

        public int height { get; set; }

        public int width { get; set; }

        public string type { get; set; }
    }

}
