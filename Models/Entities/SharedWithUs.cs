using System.ComponentModel.DataAnnotations;

namespace Report_A_Crime.Models.Entities
{
    public class SharedWithUs
    {
      
        public Guid ShareWithUsId { get; set; }
        public string ShareWithUsDescription { get; set; } = default!;
        public string ActivityType { get; set; } = default!;
        public Guid ReportId { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
