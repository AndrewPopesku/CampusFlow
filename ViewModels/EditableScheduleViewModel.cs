using CampusFlow.Models;

namespace CampusFlow.ViewModels
{
    public class EditableScheduleViewModel
    {
        public TimeSlot TimeSlot { get; set; }
        public List<Schedule> Schedules { get; set; }

        public EditableScheduleViewModel(List<Schedule> schedules, TimeSlot timeSlot)
        {
            Schedules = schedules;
            TimeSlot = timeSlot;
        }
        public static List<DayOfWeek> Days
        {
            get => new List<DayOfWeek>()
            {
                DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, 
                DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
            };
        }
    }
}
