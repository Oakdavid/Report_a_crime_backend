using Report_A_Crime.Models.Dtos;
using Report_A_Crime.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Report_A_Crime.Models.Dto
{

    public class UserDto : BaseResponse
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
       // public UserDto Data { get; set; }
        public string? RoleName { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; } 
        public bool? IsAnonymous { get; set; }
        public string? Token { get; set; }
        public ICollection<ReportDto> Reports { get; set; } = new List<ReportDto>();
        public ICollection<SharedWithUs> SharedWithUs { get; set; } = new HashSet<SharedWithUs>();
        public ICollection<RequestAService> RequestAServices { get; set; } = new HashSet<RequestAService>();
    }
    public class UserRequestModel
    {
        public string? UserName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsAnonymous { get; set; }
    }

    public class LogInWithEmailAndPassword
    {
        public string? EmailOrUserName { get; set; }
        public string? Password { get; set; }
    }
}
