using Microsoft.AspNetCore.Mvc;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.AspNetCore.Authorization;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategory _categoryRepo;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategory categoryRepo, ILogger<CategoriesController> logger)
        {
            _categoryRepo = categoryRepo;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryRepo.GetAllCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving categories: {ex.Message}");
                return BadRequest($"An error occurred while retrieving categories: {ex.Message}");
            }
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            try
            {
                var category = await _categoryRepo.GetCategoryById(categoryId);
                if (category == null)
                    return NotFound($"Category with ID {categoryId} not found.");

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the category: {ex.Message}");
                return BadRequest($"An error occurred while retrieving the category: {ex.Message}");
            }
        }

        [HttpPost("")]
        [Authorize(Roles = "seller, admin")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                var addedCategory = await _categoryRepo.AddCategory(categoryDTO);
                Log.Information("Added Category is =>{@addedCategory}", addedCategory);
                return CreatedAtAction(nameof(GetCategoryById), new { categoryId = addedCategory.CategoryId }, addedCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding the category: {ex.Message}");
                return BadRequest($"An error occurred while adding the category: {ex.Message}");
            }
        }

        [HttpPut("{categoryId}")]
        [Authorize(Roles = "seller, admin")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                if (categoryId != categoryDTO.CategoryId)
                    return BadRequest("Category ID mismatch.");

                var updatedCategory = await _categoryRepo.UpdateCategory(categoryDTO);
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the category: {ex.Message}");
                return BadRequest($"An error occurred while updating the category: {ex.Message}");
            }
        }

        [HttpDelete("{categoryId}")]
        [Authorize(Roles = "seller, admin")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                var deleted = await _categoryRepo.DeleteCategory(categoryId);
                if (!deleted)
                    return NotFound($"Category with ID {categoryId} not found.");

                return Ok($"Category with ID {categoryId} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the category: {ex.Message}");
                return BadRequest($"An error occurred while deleting the category: {ex.Message}");
            }
        }

        [HttpGet("{categoryId}/subcategories")]
        public async Task<IActionResult> GetSubcategoriesByCategory(int categoryId)
        {
            try
            {
                var subcategories = await _categoryRepo.GetSubCategoriesByCategoryId(categoryId);
                return Ok(subcategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving subcategories: {ex.Message}");
                return BadRequest($"An error occurred while retrieving subcategories: {ex.Message}");
            }
        }

        [HttpGet("{categoryId}/products")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _categoryRepo.GetProductsByCategory(categoryId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving products: {ex.Message}");
                return BadRequest($"An error occurred while retrieving products: {ex.Message}");
            }
        }
    }
}
