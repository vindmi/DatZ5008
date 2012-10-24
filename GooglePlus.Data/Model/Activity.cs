using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GooglePlus.Data.Model
{
    [Table("Activity")]
    public class Activity
    {
        public long Id { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("author")]
        public User Author { get; set; }

        [Column("google_id")]
        public string googleId { get; set; }
    }
}
