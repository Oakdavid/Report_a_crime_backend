using Report_A_Crime.Models.Dto;
using Report_A_Crime.Models.Entities;

namespace Report_A_Crime.Models.Services.Interface
{
    public interface IUserService
    {
        
        public Task<UserDto> CreateUserAsync(UserRequestModel model);
        public Task<UserDto> LogInWithEmailAndPasswordOrNameAsync(LogInWithEmailAndPassword login);
        public Task<UserDto> ForgottenPassword(string password, string confirmPassword);
        public Task<UserDto> GetUserAsync(string email);
        public Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> ChangePasswordAsync(string email, string currentPassword, string newPassword);
    }
}
