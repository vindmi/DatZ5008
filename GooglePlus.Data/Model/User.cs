
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GooglePlus.Data.Model
{
    public class User
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("google_id")]
        public string GoogleId { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        public Gender Gender { get; set; }

        [Column("birthday")]
        public DateTime? BirthDay { get; set; }

        [Column("location")]
        [StringLength(255)]
        public string Location { get; set; }

        [Column("education")]
        public string Education { get; set; }
    }
}
