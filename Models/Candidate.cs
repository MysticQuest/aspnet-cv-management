using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CvManagementApp.Models
{
    public class Candidate : Entity
    {
        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile number must be 10 digits.")]
        public string? Mobile { get; set; }

        public ICollection<Degree>? Degrees { get; set; } = new List<Degree>();

        public byte[]? CV { get; set; }
        public string? CVFileName { get; set; }
        public string? CVMimeType { get; set; }
    }
}
