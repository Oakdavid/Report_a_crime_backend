using Report_A_Crime.Models.Dto;
using Report_A_Crime.Models.Entities;

namespace Report_A_Crime.Models.Dtos
{
    public class RoleDto : BaseResponse
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = default!;
        public ICollection<UserDto> Users { get; set; }

    }
    public class RoleRequestModel
    {
       public string RoleName { get; set; }

    }


}
