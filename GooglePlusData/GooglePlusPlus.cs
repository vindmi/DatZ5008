using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using GooglePlusData.Model;

namespace GooglePlusData
{
    internal class GooglePlusPlus : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().Property(u => u.GoogleId).HasColumnName("google_id");
            modelBuilder.Entity<User>().Property(u => u.FirstName).HasColumnName("first_name");
            modelBuilder.Entity<User>().Property(u => u.LastName).HasColumnName("last_name");
            modelBuilder.Entity<User>().Property(u => u.Username).HasColumnName("username");
            modelBuilder.Entity<User>().Property(u => u.Password).HasColumnName("password");
        }
    }
}
