using Report_A_Crime.Models.Dtos;
using Report_A_Crime.Models.Entities;
using Report_A_Crime.Models.Repositories.Interface;
using Report_A_Crime.Models.Repositories.Interphase;
using Report_A_Crime.Models.Services.Interface;

namespace Report_A_Crime.Models.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<RoleDto> CreateRoleAsync(RoleRequestModel roleModel)
        {
            var roleExist=  await _roleRepository.RoleExistAsync( r => r.RoleName == roleModel.RoleName );
            if (roleExist)
            {
                return new RoleDto
                {
                    RoleName = roleModel.RoleName,
                    Message = "Role already exist",
                    Status = false
                };
            }

            var newRole = new Role
            {
                RoleName = roleModel.RoleName,
            };

            await _roleRepository.CreateAsync( newRole );
            await _unitOfWork.SaveChangesAsync();

            return new RoleDto
            {
                RoleId = newRole.RoleId,
                RoleName = newRole.RoleName,
                Message = "Role created successfully",
                Status = true
            };
        }

        public Task<bool> DeleteRoleAsync(Guid roleId)
        {
            throw new NotImplementedException();
        }

        public Task<RoleDto> GetRoleByIdAsync(RoleDto role)
        {
            throw new NotImplementedException();
        }
    }
}
