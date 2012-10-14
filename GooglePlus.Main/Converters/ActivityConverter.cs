using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GooglePlus.ApiClient.Classes;
using GooglePlus.Data.Model;

namespace GooglePlus.Main.Converters
{
    public class ActivityConverter
    {
        public Photo ConvertPhoto(GooglePlusAttachment attachment)
        {            
            return new Photo
                {
                    Src = attachment.url,
                    Comment = attachment.content
                };
        }

        public Share ConvertShare(GooglePlusObject gObject)
        {

            return new Share()
                {
                    Comment = gObject.content
                };
        }

        public Post ConvertPost(GooglePlusObject gObject)
        {
            return new Post()
                {
                    Text = gObject.content
                };
        }
    }
}
