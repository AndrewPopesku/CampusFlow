using System.ComponentModel.DataAnnotations;

namespace CampusFlow.Models
{
    public enum SemesterType
    {
        Fall,
        Spring,
        Summer
    }

    public class Semester
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public SemesterType SemesterType { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
    }
}
