using Report_A_Crime.Models.Dtos;
using Report_A_Crime.Models.Entities;
using static Report_A_Crime.Models.Dtos.RoleDto;

namespace Report_A_Crime.Models.Services.Interface
{
    public interface IRoleService
    {
        public Task<RoleDto> CreateRoleAsync(RoleRequestModel roleModel);
        public Task<bool> DeleteRoleAsync(Guid roleId);
        public Task<RoleDto> GetRoleByIdAsync(RoleDto role);
    }
}
