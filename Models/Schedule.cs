using System.ComponentModel.DataAnnotations;

namespace CampusFlow.Models
{
    public enum WeekType
    {
        Odd,
        Even
    }
    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int TimeSlotId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public WeekType WeekType { get; set; }

        public int? SemesterId { get; set; }
        public int GroupId { get; set; }

        public virtual Class Class { get; set; }
        public virtual TimeSlot TimeSlot { get; set; }
        public virtual Semester? Semester { get; set; }
        public virtual Group Group { get; set; }
        public virtual ICollection<ScheduleDate> ScheduleDates { get; set; }

    }
}
