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
        public Activity ConvertActivity(GooglePlusActivity activity, User user)
        {
            DateTime created = DateTime.Now;
            DateTime.TryParse(activity.Published, out created);

            return new Activity
            {
                googleId = activity.Id,
                Created = created,
                Author = user
            };
        }
        
        public Photo ConvertPhoto(GooglePlusAttachment attachment, Activity activity)
        {            
            return new Photo
                {
                    googleId = attachment.Id,
                    Created = activity.Created,
                    Author = activity.Author,
                    Src = attachment.FullImage.Url,
                    Comment = attachment.Content,
                    Url = attachment.Url
                };
        }

        public Share ConvertShare(GooglePlusObject gObject, Activity activity)
        {
            return new Share()
                {
                    googleId = activity.googleId,
                    Created = activity.Created,
                    Author = activity.Author,
                    Comment = gObject.Content
                };
        }

        public Post ConvertPost(GooglePlusObject gObject, Activity activity)
        {
            return new Post()
                {
                    googleId = activity.googleId,
                    Created = activity.Created,
                    Author = activity.Author,
                    Text = gObject.Content
                };
        }
    }
}
