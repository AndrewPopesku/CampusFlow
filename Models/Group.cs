using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace CampusFlow.Models
{
    public class Group
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        [Required]
        public int GroupNumber { get; set; }
        public string Subgroup { get; set; }
        public string Name
        {
            get => GroupNumber + "-" + Subgroup;
        }
    }
}
