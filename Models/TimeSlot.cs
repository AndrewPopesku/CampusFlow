using System.ComponentModel.DataAnnotations;

namespace CampusFlow.Models
{
    public class TimeSlot
    {
        public int TimeSlotId { get; set; }
        public int ClassNumber { get; set; }
        [DisplayFormat(DataFormatString = "hh:mm")]
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public virtual ICollection<StudentSchedule> Schedules { get; set; }
    }
}
