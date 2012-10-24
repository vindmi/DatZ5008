using System.ComponentModel.DataAnnotations.Schema;

namespace GooglePlus.Data.Model
{
    [Table("Share")]
    public class Share : Activity
    {
        [Column("content")]
        public string Content { get; set; }

        [Column("comment")]
        public string Comment { get; set; }
    }
}
