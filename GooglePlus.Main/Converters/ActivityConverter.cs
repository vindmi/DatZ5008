using System;
using GooglePlus.ApiClient.Classes;
using GooglePlus.Data.Model;

namespace GooglePlus.Main.Converters
{
    public class ActivityConverter
    {
        public T ConvertActivity<T>(GooglePlusActivity activity)
            where T: Activity, new()
        {
            DateTime created;
            DateTime.TryParse(activity.Published, out created);

            var act = new T { googleId = activity.Id, Created = created };

            return act;
        }

        public Photo ConvertToPhoto(GooglePlusActivity activity, GooglePlusAttachment attachment)
        {
            Photo p = ConvertActivity<Photo>(activity);

            p.Src = attachment.FullImage.Url;
            p.Comment = attachment.Content;
            p.Url = attachment.Url;

            return p;
        }     

        public Share ConvertToShare(GooglePlusActivity activity, GooglePlusObject gObject)
        {
            Share share = ConvertActivity<Share>(activity);

            share.Comment = gObject.Content;

            return share;
        }

        public Post ConvertToPost(GooglePlusActivity activity, GooglePlusObject gObject)
        {
            Post post = ConvertActivity<Post>(activity);

            post.Text = gObject.Content;

            return post;
        }
    }
}
