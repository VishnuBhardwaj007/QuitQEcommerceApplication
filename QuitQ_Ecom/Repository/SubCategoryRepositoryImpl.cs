using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;

namespace QuitQ_Ecom.Repository
{
    public class SubCategoryRepositoryImpl : ISubCategory
    {
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<SubCategoryRepositoryImpl> _logger;

        public SubCategoryRepositoryImpl(QuitQEcomContext quitQEcomContext, IMapper mapper, ILogger<SubCategoryRepositoryImpl> logger)
        {
            _context = quitQEcomContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SubCategoryDTO> AddSubCategory(SubCategoryDTO subCategoryDTO)
        {
            try
            {
                var subCategory = _mapper.Map<SubCategory>(subCategoryDTO);
                await _context.SubCategories.AddAsync(subCategory);
                await _context.SaveChangesAsync();
                return _mapper.Map<SubCategoryDTO>(subCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding subcategory: {Message}", ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<bool> DeleteSubCategory(int subCategoryId)
        {
            try
            {
                var subCategory = await _context.SubCategories.FindAsync(subCategoryId);
                if (subCategory == null)
                    return false;
                _context.SubCategories.Remove(subCategory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting subcategory with ID {SubCategoryId}: {Message}", subCategoryId, ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<List<SubCategoryDTO>> GetAllSubCategories()
        {
            try
            {
                var subCategories = await _context.SubCategories.ToListAsync();
                return _mapper.Map<List<SubCategoryDTO>>(subCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all subcategories: {Message}", ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<SubCategoryDTO> GetSubCategoryById(int subCategoryId)
        {
            try
            {
                var subCategory = await _context.SubCategories.FindAsync(subCategoryId);
                return _mapper.Map<SubCategoryDTO>(subCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving subcategory with ID {SubCategoryId}: {Message}", subCategoryId, ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<SubCategoryDTO> UpdateSubCategory(SubCategoryDTO subCategoryDTO)
        {
            try
            {
                var subCategory = await _context.SubCategories.FindAsync(subCategoryDTO.SubCategoryId);
                if (subCategory == null)
                    throw new Exception("SubCategory not found");

                // Map updated data from DTO to entity
                _mapper.Map(subCategoryDTO, subCategory);

                _context.SubCategories.Update(subCategory);
                await _context.SaveChangesAsync();

                // Map the updated entity back to DTO
                return _mapper.Map<SubCategoryDTO>(subCategory);
            }
            catch (Exception ex)
            {
                // Log exception
                _logger.LogError(ex, "Error occurred while updating subcategory: {Message}", ex.Message);
                // Re-throw the exception to propagate it further
                throw;
            }
        }
    }
}
