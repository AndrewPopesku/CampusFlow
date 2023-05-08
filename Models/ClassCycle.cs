using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusFlow.Models
{
    public class ClassCycle
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int? ClassId { get; set; }
        public int SemesterId { get; set; }

        public virtual Class Class { get; set; }
        public virtual Semester Semester { get; set; }

        public ClassCycle(DateTime date, int? classId, int semesterId)
        {
            Date = date;
            ClassId = classId;
            SemesterId = semesterId;
        }
    }
}
