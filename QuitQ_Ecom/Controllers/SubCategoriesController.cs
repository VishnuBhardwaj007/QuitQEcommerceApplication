using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.Threading.Tasks;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/subcategories")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {
        private readonly ISubCategory _subCategoryRepository;
        private readonly ILogger<SubCategoriesController> _logger;

        public SubCategoriesController(ISubCategory subCategoryRepository, ILogger<SubCategoriesController> logger)
        {
            _subCategoryRepository = subCategoryRepository;
            _logger = logger;
        }

        [HttpPost("")]
        [Authorize(Roles = "seller admin")]
        public async Task<IActionResult> AddSubCategory([FromBody] SubCategoryDTO subCategoryDTO)
        {
            try
            {
                var addedSubCategory = await _subCategoryRepository.AddSubCategory(subCategoryDTO);
                return Ok(addedSubCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding subcategory: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllSubCategories()
        {
            try
            {
                var subCategories = await _subCategoryRepository.GetAllSubCategories();
                return Ok(subCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting all subcategories: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubCategoryById(int id)
        {
            try
            {
                var subCategory = await _subCategoryRepository.GetSubCategoryById(id);
                if (subCategory == null)
                    return NotFound();
                return Ok(subCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting subcategory by ID: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "seller admin")]
        public async Task<IActionResult> UpdateSubCategory(int id, [FromBody] SubCategoryDTO subCategoryDTO)
        {
            try
            {
                subCategoryDTO.SubCategoryId = id;
                var updatedSubCategory = await _subCategoryRepository.UpdateSubCategory(subCategoryDTO);
                return Ok(updatedSubCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating subcategory: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            try
            {
                var result = await _subCategoryRepository.DeleteSubCategory(id);
                if (!result)
                    return NotFound();
                return Ok("Subcategory deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting subcategory: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
