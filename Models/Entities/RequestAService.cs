using Report_A_Crime.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Report_A_Crime.Models.Entities
{
    public class RequestAService
    {
        
        public Guid RequestServiceId { get; set; }
        public string ActivityType { get; set; } = default!;
        public string? Description { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid ReportId { get; set; }   // optional
        public Report? Reports { get; set; }   // optional
    }
}
