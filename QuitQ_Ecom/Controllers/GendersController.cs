using Microsoft.AspNetCore.Mvc;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/genders")]
    [ApiController]
    public class GendersController : ControllerBase
    {
        private readonly IGender _genderRepo;
        private readonly ILogger<GendersController> _logger;

        public GendersController(IGender genderRepo, ILogger<GendersController> logger)
        {
            _genderRepo = genderRepo;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllGenders()
        {
            try
            {
                var genders = await _genderRepo.GetAllGenders();
                return Ok(genders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving genders: {ex.Message}");
                return BadRequest($"An error occurred while retrieving genders: {ex.Message}");
            }
        }

        [HttpGet("{genderId:int}")]
        public async Task<IActionResult> GetGenderById(int genderId)
        {
            try
            {
                var gender = await _genderRepo.GetGenderById(genderId);
                if (gender == null)
                {
                    _logger.LogError($"Gender with ID {genderId} not found.");
                    return NotFound($"Gender with ID {genderId} not found.");
                }

                return Ok(gender);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the gender: {ex.Message}");
                return BadRequest($"An error occurred while retrieving the gender: {ex.Message}");
            }
        }

        [HttpPost("")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddGender([FromBody] GenderDTO genderDTO)
        {
            try
            {
                var addedGender = await _genderRepo.AddGender(genderDTO);
                return CreatedAtAction(nameof(GetGenderById), new { genderId = addedGender.GenderId }, addedGender);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding the gender: {ex.Message}");
                return BadRequest($"An error occurred while adding the gender: {ex.Message}");
            }
        }

        [HttpPut("{genderId:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateGender(int genderId, [FromBody] GenderDTO genderDTO)
        {
            try
            {
                if (genderId != genderDTO.GenderId)
                {
                    _logger.LogError("Gender ID mismatch.");
                    return BadRequest("Gender ID mismatch.");
                }

                var updatedGender = await _genderRepo.UpdateGender(genderDTO);
                return Ok(updatedGender);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the gender: {ex.Message}");
                return BadRequest($"An error occurred while updating the gender: {ex.Message}");
            }
        }

        [HttpDelete("{genderId:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteGender(int genderId)
        {
            try
            {
                var deleted = await _genderRepo.DeleteGender(genderId);
                if (!deleted)
                {
                    _logger.LogError($"Gender with ID {genderId} not found.");
                    return NotFound($"Gender with ID {genderId} not found.");
                }

                return Ok($"Gender with ID {genderId} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the gender: {ex.Message}");
                return BadRequest($"An error occurred while deleting the gender: {ex.Message}");
            }
        }
    }
}
