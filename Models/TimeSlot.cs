using System.ComponentModel.DataAnnotations;

namespace CampusFlow.Models
{
    public class TimeSlot
    {
        public int TimeSlotId { get; set; }
        public int ClassNumber { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public virtual ICollection<Class> Schedules { get; set; }
    }
}
