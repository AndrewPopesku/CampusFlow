namespace CampusFlow.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }
        public virtual ClassCycle ClassCycle { get; set; }
    }
}
