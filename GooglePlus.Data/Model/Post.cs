using System.ComponentModel.DataAnnotations.Schema;

namespace GooglePlus.Data.Model
{
    [Table("Post")]
    public class Post : Activity
    {
        [Column("text")]
        public string Text { get; set; }
    }
}
