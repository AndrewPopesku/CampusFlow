using CampusFlow.Models;

namespace CampusFlow.ViewModels
{
    public class ScheduleViewModel
    {
        public List<Class> Classes { get; set; }
        public TimeSlot TimeSlot { get; set; }

        public ScheduleViewModel(List<Class> sch, TimeSlot ts)
        {
            Classes = sch;
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
