using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace QuitQ_Ecom.Repository
{
    public class CategoryRepositoryImpl : ICategory
    {
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryRepositoryImpl> _logger;

        public CategoryRepositoryImpl(QuitQEcomContext quitQEcomContext, IMapper mapper, ILogger<CategoryRepositoryImpl> logger)
        {
            _context = quitQEcomContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoryDTO> AddCategory(CategoryDTO categoryDTO)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDTO);
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return _mapper.Map<CategoryDTO>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding category: {Message}", ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category == null)
                    return false;
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category with ID {CategoryId}: {Message}", categoryId, ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                return _mapper.Map<List<CategoryDTO>>(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all categories: {Message}", ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<CategoryDTO> GetCategoryById(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                return _mapper.Map<CategoryDTO>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving category with ID {CategoryId}: {Message}", categoryId, ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<List<ProductDTO>> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _context.Products.Where(p => p.SubCategory.CategoryId == categoryId).ToListAsync();
                return _mapper.Map<List<ProductDTO>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving products for category with ID {CategoryId}: {Message}", categoryId, ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<List<SubCategoryDTO>> GetSubcategoriesByCategory(int categoryId)
        {
            try
            {
                var subcategories = await _context.SubCategories.Where(sc => sc.CategoryId == categoryId).ToListAsync();
                return _mapper.Map<List<SubCategoryDTO>>(subcategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving subcategories for category with ID {CategoryId}: {Message}", categoryId, ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<CategoryDTO> UpdateCategory(CategoryDTO categoryDTO)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryDTO.CategoryId);
                if (category == null)
                    throw new Exception("Category not found");
                category.CategoryName = categoryDTO.CategoryName;
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return _mapper.Map<CategoryDTO>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category: {Message}", ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<List<SubCategoryDTO>> GetSubCategoriesByCategoryId(int categoryId)
        {
            try
            {
                var subcategories = await _context.SubCategories.Where(x => x.CategoryId == categoryId).ToListAsync();
                if (subcategories != null)
                {
                    return _mapper.Map<List<SubCategoryDTO>>(subcategories);
                }
                return new List<SubCategoryDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving subcategories for category with ID {CategoryId}: {Message}", categoryId, ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }
    }
}
