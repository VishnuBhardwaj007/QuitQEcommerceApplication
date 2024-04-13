using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.Threading.Tasks;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICity _cityRepo;
        private readonly ILogger<CityController> _logger;

        public CityController(ICity cityRepo, ILogger<CityController> logger)
        {
            _cityRepo = cityRepo;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllCities()
        {
            try
            {
                var cities = await _cityRepo.GetAllCities();
                return Ok(cities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all cities: {Message}", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{cityId}")]
        public async Task<IActionResult> GetCityById(int cityId)
        {
            try
            {
                var city = await _cityRepo.GetCityById(cityId);
                if (city == null)
                {
                    return NotFound("City not found");
                }
                return Ok(city);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting city by ID {CityId}: {Message}", cityId, ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddCity([FromBody] CityDTO cityDTO)
        {
            try
            {
                var addedCity = await _cityRepo.AddCity(cityDTO);
                return CreatedAtAction(nameof(GetCityById), new { cityId = addedCity.CityId }, addedCity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding city: {Message}", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{cityId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCityState(int cityId, int stateId)
        {
            try
            {
                var updatedCity = await _cityRepo.UpdateCityState(cityId, stateId);
                if (updatedCity == null)
                {
                    return NotFound("City or State not found");
                }
                return Ok(updatedCity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating city: {Message}", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{cityId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCity(int cityId)
        {
            try
            {
                var deleted = await _cityRepo.DeleteCity(cityId);
                if (!deleted)
                {
                    return NotFound("City not found");
                }
                return Ok("City deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting city: {Message}", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
