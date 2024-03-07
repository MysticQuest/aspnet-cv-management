using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CvManagementApp.Models
{
    public class Candidate : Entity
    {
        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string? Mobile { get; set; }

        public ICollection<Degree>? Degrees { get; set; } = new List<Degree>();

        public byte[]? CV { get; set; }
        public string? CVFileName { get; set; }
        public string? CVMimeType { get; set; }
    }
}
