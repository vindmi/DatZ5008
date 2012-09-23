using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GooglePlus.ApiClient.Classes
{
    public class GooglePlusUser
    {
        public string id { get; set; }

        public Name name { get; set; }

        public string displayName { get; set; }

        public string tagline { get; set; }
    }

    public class Name
    {
        public string familyName { get; set; }

        public string givenName { get; set; }
    }
}
