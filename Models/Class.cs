using System.ComponentModel.DataAnnotations;

namespace CampusFlow.Models
{
    public enum ClassType
    {
        Lecture = 1,
        Practise,
        LaboratoryWork
    }

    public enum WeekType
    {
        Odd,
        Even
    }

    public class Class
    {
        public int Id { get; set; }
        public ClassType ClassType { get; set; }
        public WeekType WeekType { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public string? Location { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public int GroupId { get; set; }
        public int TimeSlotId { get; set; }

        public Teacher Teacher { get; set; }
        public Group Group { get; set; }
        public Subject Subject { get; set; }
        public TimeSlot TimeSlot { get; set; }
    }
}
