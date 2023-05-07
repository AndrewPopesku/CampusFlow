using Microsoft.EntityFrameworkCore;
using CampusFlow.Models;

namespace CampusFlow.Data
{
    public class CampusContext : DbContext
    {
        public CampusContext(DbContextOptions<CampusContext> options) : base(options)
        {
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<CampusFlow.Models.TimeSlot>? TimeSlot { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Subject>().HasMany(s => s.Classes).WithOne(s => s.Subject);
        }

    }
}
