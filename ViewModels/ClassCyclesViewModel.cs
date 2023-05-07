using CampusFlow.Models;

namespace CampusFlow.ViewModels
{
    public class ClassCycleViewModel
    {
        public List<ClassCycle> Classes { get; set; }
        public TimeSlot TimeSlot { get; set; }

        public ClassCycleViewModel(List<ClassCycle> sch, TimeSlot ts)
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
