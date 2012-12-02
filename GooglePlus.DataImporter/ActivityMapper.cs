using System;
using GooglePlus.ApiClient.Classes;
using GooglePlus.Data.Model;

namespace GooglePlus.DataImporter
{
    public class ActivityMapper
    {
        public T CreateFrom<T>(GooglePlusActivity activity)
            where T: Activity, new()
        {
            DateTime created;
            DateTime.TryParse(activity.Published, out created);

            var act = new T { googleId = activity.Id, Created = created };

            return act;
        }

        public Photo CreatePhoto(GooglePlusActivity activity, GooglePlusAttachment attachment)
        {
            Photo p = CreateFrom<Photo>(activity);

            p.Src = attachment.FullImage.Url;
            p.Comment = attachment.Content;
            p.Url = attachment.Url;

            return p;
        }     

        public Share CreateShare(GooglePlusActivity activity, GooglePlusObject gObject)
        {
            Share share = CreateFrom<Share>(activity);

            share.Comment = gObject.Content;

            return share;
        }

        public Post CreatePost(GooglePlusActivity activity, GooglePlusObject gObject)
        {
            Post post = CreateFrom<Post>(activity);

            post.Text = gObject.Content;

            return post;
        }
    }
}
