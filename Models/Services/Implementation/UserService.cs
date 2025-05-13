using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Report_A_Crime.Exception;
using Report_A_Crime.Models.Dto;
using Report_A_Crime.Models.Dtos;
using Report_A_Crime.Models.Entities;
using Report_A_Crime.Models.Repositories.Interface;
using Report_A_Crime.Models.Repositories.Interphase;
using Report_A_Crime.Models.Services.Interface;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Report_A_Crime.Models.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _configuration;
       // private readonly IHttpContextAccessor _httpcontextAccessor;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IRoleRepository roleRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
            _configuration = configuration;
           // _httpcontextAccessor = httpcontextAccessor;
        }
        public async Task<UserDto> CreateUserAsync(UserRequestModel model)
        {
         //   var hashed = BCrypt.Net.BCrypt.HashPassword("Admin123");


            if (!IsValidEmail(model.Email))
            {
                return new UserDto
                {
                    Message = "Invalid email format",
                    Status = false,
                    Data = null
                };
            }


            var userExist = await _userRepository.UserExistAsync(u => u.Email.ToLower()== model.Email.ToLower()|| u.UserName == model.UserName);
            if (userExist)
            {
                return new UserDto
                {
                    Message = "User already Exist",
                    Status = false,
                    Data = null
                };
            }
            
            var adminRole = await _roleRepository.GetRoleAsync(r => r.RoleName == "Admin");

            if(adminRole == null)
            {
                adminRole = new Role
                {
                    RoleId = Guid.NewGuid(),
                    RoleName = "Admin"
                };
                await _roleRepository.CreateAsync(adminRole);
            }

            var userRole = await _roleRepository.GetRoleAsync(r => r.RoleName == "User");

            if (userRole == null)
            {
                userRole = new Role
                {
                    RoleId = Guid.NewGuid(),
                    RoleName = "User"
                };
                await _roleRepository.CreateAsync(userRole);
            }

            Role assignedRole;
            var isAdminExist = await _userRepository.GetUserAsync(u => u.Role.RoleName == "Admin");

            if(isAdminExist == null)
            {
                assignedRole = adminRole;
            }
            else
            {
                assignedRole = userRole;
            }

            var newUser = new User
            {
                Role = assignedRole,
                RoleId = assignedRole.RoleId,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                PhoneNumber = model.PhoneNumber,
                IsAnonymous = model.IsAnonymous,
            };
            await  _userRepository.CreateUserAsync(newUser);
            await _unitOfWork.SaveChangesAsync();

            return new UserDto
            {
                Message = "User creation Successful",
                UserId = newUser.UserId,
                RoleId = newUser.RoleId,
                UserName = newUser.UserName,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber,
                IsAnonymous = newUser.IsAnonymous,
                Reports = newUser.Reports.Select(r => new ReportDto
                {
                    ReportId = r.ReportId,
                    ReportDescription = r.ReportDescription,
                    DateOccurred = r.DateOccurred,
                    CreatedAt = r.CreatedAt,
                    Location = r.Location,
                }).ToList(),
                RequestAServices = newUser.RequestAServices,
                SharedWithUs = newUser.SharedWithUs,
                Status = true
            };
        }

        public async Task<UserDto> GetUserAsync(string email)
        {
            var getUser = await _userRepository.GetUserAsync(u => u.Email == email);
            if(getUser != null)
            {
                return new UserDto
                {
                    UserId = getUser.UserId,
                    RoleId = getUser.RoleId,
                    Email = getUser.Email,
                    FirstName = getUser.FirstName,
                    LastName = getUser.LastName,
                    Password = getUser.Password,
                    PhoneNumber = getUser.PhoneNumber,
                    IsAnonymous = getUser.IsAnonymous,
                    Reports = getUser.Reports.Select(r => new ReportDto
                    {
                        ReportId = r.ReportId,
                        ReportDescription = r.ReportDescription,
                        DateOccurred = r.DateOccurred,
                        CreatedAt = r.CreatedAt,
                        Location = r.Location,
                    }).ToList(),
                    RequestAServices = getUser.RequestAServices,
                    SharedWithUs = getUser.SharedWithUs,
                    Message = "User Found",
                    Status = true,
                };
            }
             
            else
            {
                return null;
            }
        }

        public async Task<UserDto> LogInWithEmailAndPasswordOrNameAsync(LogInWithEmailAndPassword login)
        {
            var userLogin = await _userRepository.GetUserAsync(u => u.Email.ToLower() == login.EmailOrUserName.ToLower() || u.UserName.ToLower() == login.EmailOrUserName.ToLower());

            if (userLogin == null)
            {
                return new UserDto
                {
                    Message = "User not found",
                    Status = false
                };
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(login.Password, userLogin.Password);

            if (!isValidPassword)
            {
                return new UserDto
                {
                    Message = "Username or password is incorrect. Please enter valid credentials.",
                    Status = false
                };
            }

            var role = await _roleRepository.GetRoleAsync(r => r.RoleId == userLogin.RoleId);
            if (role == null)
            {
                return new UserDto
                {
                    Message = "Role not found",
                    Status = false
                };
            }

            var token = await GenerateTokenAsync(
                new UserDto
                {
                    UserId = userLogin.UserId,
                    UserName = userLogin.UserName,
                    Email = userLogin.Email
                },
                new RoleDto
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName
                }
            );

            return new UserDto
            {
                Email = userLogin.Email,
                UserName = userLogin.UserName,
                Token = token,
                Message = "Login successful",
                Status = true
            };
        }


        private async Task<string> GenerateTokenAsync(UserDto user, RoleDto role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:key"]);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role.RoleName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = issuer,
                Audience = audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<string> GetRoleNameAsync(Guid roleId)
        {
            var role = await _roleRepository.GetRoleAsync(x => x.RoleId == roleId);
            return role?.RoleName?? "unknown";
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
           var getAllUsers = await _userRepository.GetAllUsersAsync();
            if(getAllUsers != null && getAllUsers.Any())
            {
                var userDto = getAllUsers.Select(u => new UserDto
                {
                    UserId = u.UserId,
                    RoleId = u.RoleId,
                    Email = u.Email,
                    UserName = u.UserName,
                    PhoneNumber = u.PhoneNumber,
                    Status = true,
                    Message = "User found",

                }).ToList();

                return userDto;
            }
            else if (getAllUsers == null)
            {
                return new List<UserDto>
                {
                    new UserDto
                    {
                        Message = "Failed to retrieve user",
                        Status = false,
                    }
                };
            }
            else
            {
               return new List<UserDto> {new UserDto
                {
                    Message = "No users found",
                    Status = false,
                }};
            }

          
                    
        }

        public async Task<UserDto> ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetUserAsync( u => u.Email == email );

            if(user == null)
                return new UserDto
                {
                    Message = "User email does not exist"
                };

            bool validPassword = BCrypt.Net.BCrypt.Verify(currentPassword, user.Password);
            if(!validPassword)
            {
                return new UserDto
                {
                    Message = "Current password is incorrect"
                };
            }

            if(BCrypt.Net.BCrypt.Verify(newPassword, user.Password))
            {
                return new UserDto
                {
                    Message = "New password cannot be the same as the current password"
                };
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return new UserDto
            {
                Email = user.Email,
                Message = "Password changed successfully",
            };
        }
        public Task<UserDto> ForgottenPassword(string password, string confirmPassword)
        {
            throw new NotImplementedException();
        }
            
        private bool IsValidEmail(string email)
        {
           var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
           var regex = new Regex(emailPattern);
            return regex.IsMatch(email);
        }

        

        private bool VerifyPassword(string currentPassword, string storedHashedPassword)
        {
           return BCrypt.Net.BCrypt.Verify(currentPassword, storedHashedPassword);
        }
    }
}
