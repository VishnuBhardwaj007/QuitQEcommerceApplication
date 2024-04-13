using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/user-addresses")]
    [ApiController]
    [Authorize]
    public class UserAddressController : ControllerBase
    {
        private readonly IUserAddress _userAddressRepo;
        private readonly ILogger<UserAddressController> _logger;

        public UserAddressController(IUserAddress userAddressRepo, ILogger<UserAddressController> logger)
        {
            _userAddressRepo = userAddressRepo;
            _logger = logger;
        }

        [HttpPost("")]

        public async Task<IActionResult> AddUserAddress([FromBody] UserAddressDTO userAddressDTO)
        {
            try
            {
                var addedUserAddress = await _userAddressRepo.AddUserAddress(userAddressDTO);
                return CreatedAtAction(nameof(AddUserAddress), new { userAddressId = addedUserAddress.UserAddressId }, addedUserAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding user address: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPut("{userAddressId}")]
        public async Task<IActionResult> UpdateUserAddress(int userAddressId, [FromBody] UserAddressDTO userAddressDTO)
        {
            try
            {
                var updatedUserAddress = await _userAddressRepo.UpdateUserAddress(userAddressId, userAddressDTO);
                if (updatedUserAddress == null)
                {
                    return NotFound("User Address not found");
                }
                return Ok(updatedUserAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating user address: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete("{userAddressId}")]
        public async Task<IActionResult> DeleteUserAddress(int userAddressId)
        {
            try
            {
                var deleted = await _userAddressRepo.DeleteUserAddress(userAddressId);
                if (!deleted)
                {
                    return NotFound("User Address not found");
                }
                return Ok("User Address deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user address: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserAddressesByUserId(int userId)
        {
            try
            {
                var userAddresses = await _userAddressRepo.GetUserAddressesByUserId(userId);
                return Ok(userAddresses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user addresses by user ID: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
