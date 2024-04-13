using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/users")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        private readonly IUser _userRepo;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUser user, ILogger<UsersController> logger)
        {
            _userRepo = user;
            _logger = logger;
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] UserDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("enter the required values");
            }
            try
            {
                string hashedPassword = HashPassword(user.Password);
                user.Password = hashedPassword;

                user.UserStatusId = 1;
                var userCreated = await _userRepo.AddUser(user);
                if (userCreated == null)
                {
                    _logger.LogInformation("Cannot create the user.");
                    return NotFound("Cannot create the User");
                }
                return Ok(userCreated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while registering user: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var userobj = await _userRepo.GetAllUsersAsync();
                if (userobj == null)
                {
                    _logger.LogInformation("No users found.");
                    return NotFound("No users");
                }

                // If the users were successfully retrieved
                return Ok(userobj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting all users: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("usertype/{usertypeId:int}")]
        public async Task<IActionResult> GetUserByUserType(int usertypeId)
        {
            try
            {
                var users = await _userRepo.GetUsersByUserType(usertypeId);
                if (users == null)
                {
                    _logger.LogInformation($"No users found for user type ID: {usertypeId}");
                    return NoContent();
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting users by user type: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete("{userId:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUserById(int userId)
        {
            try
            {
                var userobj = await _userRepo.DeleteUserById(userId);
                if (userobj == null)
                {
                    _logger.LogInformation($"User with ID {userId} not found.");
                    return NotFound("User not found.");
                }

                // If the user was successfully deleted
                return Ok("User deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user by ID: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var userobj = await _userRepo.GetUserByIdAsync(userId);
                if (userobj == null)
                {
                    _logger.LogInformation($"User with ID {userId} not found.");
                    return NotFound("User not found.");
                }

                // If the user was successfully retrieved
                return Ok(userobj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user by ID: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
