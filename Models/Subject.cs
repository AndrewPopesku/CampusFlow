namespace Schedule.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<StudentSchedule> Schedules { get; set; }
    }
}
