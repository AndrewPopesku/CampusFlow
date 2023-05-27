using CampusFlow.Models;

namespace CampusFlow.ViewModels
{
    public class ScheduleViewModel
    {
        public string ClassName { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public string TeacherName { get; set; }
        public string ClassType { get; set; }
        public int ScheduleDateId { get; set; }
    }
}
