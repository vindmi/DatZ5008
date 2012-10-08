using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace GooglePlus.Data.Model
{
    [Table("Photo")]
    public class Photo : Activity
    {
        [Column("src")]
        public string Src { get; set; }

        [Column("comment")]
        public string Comment { get; set; }
    }
}
