using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GooglePlus.ApiClient.Classes
{
    [DataContract]
    public class GooglePlusActivity
    {
        [DataMember(Name="id")]
        public string Id { get; set; }

        [DataMember(Name = "actor")]
        public GooglePlusUser Actor { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "published")]
        public string Published { get; set; }

        [DataMember(Name = "updated")]
        public string Updated { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "verb")]
        public string Verb { get; set; }

        [DataMember(Name = "annotation")]
        public string Annotation { get; set; }

        [DataMember(Name = "object")]
        public GooglePlusObject GoogleObject { get; set; }
    }

    [DataContract]
    public class GooglePlusObject
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "objectType")]
        public string ObjectType { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "attachments")]
        public List<GooglePlusAttachment> Attachments { get; set; }
    }

    [DataContract]
    public class GooglePlusAttachment
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "fullImage")]
        public GooglePlusImage FullImage { get; set; }

        [DataMember(Name = "objectType")]
        public string ObjectType { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }
    }

    [DataContract]
    public class GooglePlusImage
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "height")]
        public int Height { get; set; }

        [DataMember(Name = "width")]
        public int Width { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }

}
