using System.Data.Entity;
using GooglePlus.Data.Model;

namespace GooglePlus.Data
{
    internal class GooglePlus : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Activity> Activities { get; set; }
    }
}
