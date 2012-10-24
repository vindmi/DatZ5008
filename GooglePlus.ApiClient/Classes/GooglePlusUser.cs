using System.Runtime.Serialization;

namespace GooglePlus.ApiClient.Classes
{
    [DataContract(Name="user")]
    public class GooglePlusUser
    {
        [DataMember(Name="id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public Name Name { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "tagline")]
        public string Tagline { get; set; }
    }

    [DataContract(Name = "name")]
    public class Name
    {
        [DataMember(Name = "familyName")]
        public string FamilyName { get; set; }

        [DataMember(Name = "givenName")]
        public string GivenName { get; set; }
    }
}
