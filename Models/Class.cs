using System.ComponentModel.DataAnnotations;

namespace CampusFlow.Models
{
    public enum ClassType
    {
        Lecture,
        Lab,
        Practise
    }

    public class Class
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        public int? SemesterId { get; set; }
        public ClassType ClassType { get; set; }
        public int TeacherId { get; set; }

        public virtual Semester? Semester { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
