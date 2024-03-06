using System.ComponentModel.DataAnnotations;

namespace CvManagementApp.Models
{
    public class Degree : Entity
    {
        [Required]
        public string Name { get; set; }
    }
}
