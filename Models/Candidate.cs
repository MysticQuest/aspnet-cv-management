namespace CvManagementApp.Models
{
    public class Candidate : Entity
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public List<Degree> Degrees { get; set; } = new List<Degree>();

        public byte[] CV { get; set; }
    }
}
