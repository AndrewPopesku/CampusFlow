using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace CampusFlow.Models
{
    public class Group
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
