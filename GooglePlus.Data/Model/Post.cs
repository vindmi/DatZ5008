using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
