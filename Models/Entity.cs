using System.ComponentModel.DataAnnotations;

namespace CvManagementApp.Models
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
