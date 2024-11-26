using Microsoft.AspNetCore.Mvc;
using Report_A_Crime.Models.Dto;
using Report_A_Crime.Models.Services.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Report_A_Crime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser([FromQuery]string email)
        {
            var getUser = await _userService.GetUserAsync(email);
            if(getUser == null)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = "User not found",
                    StatusCode = 400
                });
            }
            return Ok(new
            {
                Status = true,
                Message = "User found",
                Enail = getUser.Email,  // i might remove this
            });
        }
        
        [HttpPost("SignUp")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestModel model)
        {
            var createUser = await _userService.CreateUserAsync(model);
            if(createUser.Status)
            {
                return Ok( new
                {
                    Status = true,
                    StatusCode = 200,
                    Message = createUser.Message,
                });
            }

            return BadRequest(new
            {
                Status = false,
                Message = createUser.Message,
                StatusCode = 400,
            });

        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginWithEmailAndPassword([FromBody] LogInWithEmailAndPassword login)
        {
            try
            {
                var userLogin = await _userService.LogInWithEmailAndPasswordOrNameAsync(login);
                if (userLogin.Status)
                {
                    return Ok(new
                    {
                        Status = true,
                        StatusCode = 200,
                        Message = userLogin.Message,
                        Token = userLogin.Token
                    });
                }
                return StatusCode(500, new
                {
                    Status = false,
                    Message = userLogin.Message,
                    StatusCode = 500,
                });
            }
            catch (System.Exception ex)
            {
                 Console.WriteLine("Exception caught: " + ex.Message);    // Temporary logging to console
                 Console.WriteLine("Stack Trace: " + ex.StackTrace);

                return StatusCode(500, new
                {
                    Status = false,
                    Message = "An internal server error occurred. Please try again later",
                    StatusCode = 500
                });
            }
        }


        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var getAllUsers = await _userService.GetAllUsersAsync();
            if(getAllUsers != null)
            return Ok(getAllUsers);
            else
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = "No users found",
                    StatusCode = 500,
                });
            }

        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync(string userEmail, string currentPassword, string newPassword)
        {
            var password = await _userService.ChangePasswordAsync(userEmail, currentPassword, newPassword);
            if(password != null)
            {
                return Ok(new
                {
                    Message = password.Message
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = password.Message,
                });
            }
        }
    }
}
