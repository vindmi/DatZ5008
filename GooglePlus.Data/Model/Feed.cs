using System;

namespace GooglePlus.Data.Model
{
    public class Feed
    {
        public FeedType Type { get; set; }

        public DateTime CreatedDate { get; set; }

        public long ReferenceId { get; set; }

        public override string ToString()
        {
            return String.Format("Feed [feedType={0}, date={1}, referenceId={2}]",
                Type.ToString(), CreatedDate.ToShortDateString(), ReferenceId.ToString());
        }
    }

    public enum FeedType
    {
        POST,
        SHARE,
        PHOTO
    }
}
