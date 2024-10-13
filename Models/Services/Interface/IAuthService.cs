using Report_A_Crime.Models.Dto;
using Report_A_Crime.Models.Entities;
using System.Security.Claims;

namespace Report_A_Crime.Models.Services.Interface
{
    public interface IAuthService
    {
        Task<string> Authenticate(string email, string password);
        //Task <string> GenerateToken(UserDto user);
        ClaimsPrincipal ValidateToken(string token);
    }
}
