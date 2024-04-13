using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;
using QuitQ_Ecom.Repository;

public class BrandRepositoryImpl : IBrand
{
    private readonly QuitQEcomContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<BrandRepositoryImpl> _logger;

    public BrandRepositoryImpl(QuitQEcomContext quitQEcomContext, IMapper mapper, ILogger<BrandRepositoryImpl> logger)
    {
        _context = quitQEcomContext;
        _mapper = mapper;
        _logger = logger; // Injected logger
    }

    public async Task<BrandDTO> AddBrand(BrandDTO brandDTO)
    {
        try
        {
            var brand = _mapper.Map<Brand>(brandDTO);
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            return _mapper.Map<BrandDTO>(brand);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding brand: {Message}", ex.Message);
            throw; // Re-throw the exception to propagate it further
        }
    }
    public async Task<bool> DeleteBrand(int brandId)
    {
        try
        {
            var brand = await _context.Brands.FindAsync(brandId);
            if (brand == null)
                return false;

            // Check if there are dependent records
            var dependentRecords = await _context.Products.Where(e => e.BrandId == brandId).ToListAsync();
            if (dependentRecords.Any())
            {
                // Delete dependent records first
                _context.Products.RemoveRange(dependentRecords);
            }

            // Remove the brand
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting brand with ID {BrandId}: {Message}", brandId, ex.Message);
            throw; // Re-throw the exception to propagate it further
        }
    }


    /* public async Task<bool> DeleteBrand(int brandId)
     {
         try
         {
             var brand = await _context.Brands.FindAsync(brandId);
             if (brand == null)
                 return false;
             _context.Brands.Remove(brand);
             await _context.SaveChangesAsync();
             return true;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "Error occurred while deleting brand with ID {BrandId}: {Message}", brandId, ex.Message);
             throw; // Re-throw the exception to propagate it further
         }
     }
    */

    public async Task<List<BrandDTO>> GetAllBrands()
    {
        try
        {
            var brands = await _context.Brands.ToListAsync();
            return _mapper.Map<List<BrandDTO>>(brands);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all brands: {Message}", ex.Message);
            throw; // Re-throw the exception to propagate it further
        }
    }

    public async Task<BrandDTO> GetBrandById(int brandId)
    {
        try
        {
            var brand = await _context.Brands.FindAsync(brandId);
            return _mapper.Map<BrandDTO>(brand);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving brand with ID {BrandId}: {Message}", brandId, ex.Message);
            throw; // Re-throw the exception to propagate it further
        }
    }

    public async Task<BrandDTO> UpdateBrand(int brandId, BrandDTO brandDTO)
    {
        try
        {
            var brand = await _context.Brands.FindAsync(brandId);
            if (brand == null)
            {
                return null;
            }

            // Update brand properties
            brand.BrandName = brandDTO.BrandName; // Update other properties as needed
            brand.BrandLogo = brandDTO.BrandLogo; // Update brand logo

            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
            return _mapper.Map<BrandDTO>(brand);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating brand with ID {BrandId}: {Message}", brandId, ex.Message);
            throw; // Re-throw the exception to propagate it further
        }
    }
}
