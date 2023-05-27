using System.ComponentModel.DataAnnotations;

namespace CampusFlow.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public string FullName
        {
            get => FirstName + " " + LastName;
        }
    }
}
