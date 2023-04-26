using System.ComponentModel.DataAnnotations;

namespace CampusFlow.Models
{
    public class Subject
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<StudentSchedule> Schedules { get; set; }
    }
}
