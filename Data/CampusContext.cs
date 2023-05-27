using Microsoft.EntityFrameworkCore;
using CampusFlow.Models;
using System.Xml;

namespace CampusFlow.Data
{
    public class CampusContext : DbContext
    {
        public CampusContext(DbContextOptions<CampusContext> options) : base(options)
        {
        }

        public DbSet<Class> Classes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<TimeSlot> TimeSlot { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<ScheduleDate> ScheduleDates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Semester)
                .WithMany(s => s.Schedules)
                .HasForeignKey(s => s.SemesterId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Class>()
                .HasOne(c => c.Semester)
                .WithMany(s => s.Classes)
                .HasForeignKey(c => c.SemesterId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ScheduleDate>().ToTable("ScheduleDate");
        }
    }
}
