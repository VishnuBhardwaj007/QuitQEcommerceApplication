using Microsoft.AspNetCore.Mvc;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Data.SqlClient.Server;
using QuitQ_Ecom.Models;
using Microsoft.AspNetCore.Authorization;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/brands")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrand _brandRepo;
        private readonly ILogger<BrandsController> _logger;

        public BrandsController(IBrand brandRepo, ILogger<BrandsController> logger)
        {
            _brandRepo = brandRepo;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllBrands()
        {
            try
            {
                var brands = await _brandRepo.GetAllBrands();
                return Ok(brands);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving brands: {ex.Message}");
                return BadRequest($"An error occurred while retrieving brands: {ex.Message}");
            }
        }

        [HttpGet("{brandId}")]
        public async Task<IActionResult> GetBrandById(int brandId)
        {
            try
            {
                var brand = await _brandRepo.GetBrandById(brandId);
                if (brand == null)
                    return NotFound($"Brand with ID {brandId} not found.");

                return Ok(brand);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the brand: {ex.Message}");
                return BadRequest($"An error occurred while retrieving the brand: {ex.Message}");
            }
        }

        [HttpPost("")]
        [Authorize(Roles = "seller, admin")]
        public async Task<IActionResult> AddBrand([FromForm] BrandDTO formData)
        {
            try
            {
                var file = Request.Form.Files[0]; // Access the uploaded file

                // Check if the file is not empty
                if (file == null || file.Length == 0)
                    return BadRequest("File is empty");

                // Construct the file path for saving (you can adjust the path as needed)
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", uniqueFileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                formData.BrandLogo = filePath;

                var returnedObj = await _brandRepo.AddBrand(formData);
                if (returnedObj == null)
                {
                    return NotFound();
                }
                return Ok("Brand added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        [HttpPut("{brandId}")]
        [Authorize(Roles = "seller, admin")]
        public async Task<IActionResult> UpdateBrand([FromRoute] int brandId, [FromForm] BrandDTO brandDTO)
        {
            try
            {
                var file = Request.Form.Files[0]; // Access the uploaded file

                // Check if the file is not empty
                if (file == null || file.Length == 0)
                    return BadRequest("File is empty");

                // Construct the file path for saving (you can adjust the path as needed)
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file.FileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                brandDTO.BrandLogo = filePath;

                var updatedBrand = await _brandRepo.UpdateBrand(brandId, brandDTO);
                if (updatedBrand == null)
                {
                    return NotFound();
                }
                return Ok("Brand updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        [HttpDelete("{brandId}")]
        [Authorize(Roles = "seller, admin")]
        public async Task<IActionResult> DeleteBrand(int brandId)
        {
            try
            {
                var deleted = await _brandRepo.DeleteBrand(brandId);
                if (!deleted)
                    return NotFound($"Brand with ID {brandId} not found.");

                return Ok($"Brand with ID {brandId} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the brand: {ex.Message}");
                return BadRequest($"An error occurred while deleting the brand: {ex.Message}");
            }
        }
    }
}
