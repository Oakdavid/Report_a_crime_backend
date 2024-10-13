using System.ComponentModel.DataAnnotations;

namespace Report_A_Crime.Models.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [EmailAddress]
        public string? Email { get; set; } 
        [Phone]
        public string? Password { get; set; }
        public string? HashSalt { get; set; }
        public string? PhoneNumber { get; set; }    // validation
        public bool? IsAnonymous { get; set; }
        public bool KycStatus { get; set; }
        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
        public ICollection<SharedWithUs> SharedWithUs { get; set; } = new HashSet<SharedWithUs>();
        public ICollection<RequestAService> RequestAServices { get; set; } = new HashSet<RequestAService>();
        public Guid RoleId { get; set; }
        public Role? Role { get; set; } = default!;
        

    }
}
