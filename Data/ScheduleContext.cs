using Microsoft.EntityFrameworkCore;
using Schedule.Models;

namespace Schedule.Data
{
    public class ScheduleContext : DbContext
    {
        public ScheduleContext(DbContextOptions<ScheduleContext> options) : base(options)
        {
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentSchedule> Schedules { get; set; }
        public DbSet<Schedule.Models.TimeSlot>? TimeSlot { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Subject>().HasMany(s => s.Schedules).WithOne(s => s.Subject);
        }

    }
}
