using System;

namespace GooglePlus.Data.Model
{
    public class Feed
    {
        public FeedType Type;

        public DateTime CreatedDate;

        public long ReferenceId;

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
