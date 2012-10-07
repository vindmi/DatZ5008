
using System.ComponentModel.DataAnnotations.Schema;
namespace GooglePlus.Data.Model
{
    public class User
    {
        public long Id { get; set; }

        [Column("google_id")]
        public string GoogleId { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        public Gender Gender { get; set; }
    }
}
