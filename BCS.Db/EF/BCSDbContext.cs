using System;
using BCS.Application.Entity;
using System.Data.Entity;

namespace BCS.Db.EF
{
    public class BCSDbContext : DbContext
    {
        public BCSDbContext()
            : base("name=BCSConnection")
        {
            //this.Database.Connection.Open()
            //Database.SetInitializer(new BCSInitializer());
        }

        public virtual DbSet<MainUser> MainUsers { get; set; }
        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
        public virtual DbSet<ErrorLogs> ErrorLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure StudentId as FK for StudentAddress
            modelBuilder.Entity<MainUser>()
                        .HasOptional(s => s.Details)
                        .WithRequired(d => d.MainUser);

        }
    }
}
