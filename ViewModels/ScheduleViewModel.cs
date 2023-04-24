using CampusFlow.Models;

namespace CampusFlow.ViewModels
{
    public class ScheduleViewModel
    {
        public List<StudentSchedule> Schedules { get; set; }
        public TimeSlot TimeSlot { get; set; }

        public ScheduleViewModel(List<StudentSchedule> sch, TimeSlot ts)
        {
            Schedules = sch;
            TimeSlot = ts;
        }
        public static List<DayOfWeek> Days
        {
            get
            {
                var daylist = new List<DayOfWeek>()
                {
                    DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday
                };
                return daylist;
            }
        }
    }
}
