using QuitQ_Ecom.DTOs;

namespace QuitQ_Ecom.Repository
{
    public interface IBrand
    {
        Task<BrandDTO> AddBrand(BrandDTO brandDTO);
        Task<bool> DeleteBrand(int brandId);
        Task<List<BrandDTO>> GetAllBrands();
        Task<BrandDTO> GetBrandById(int brandId);
        Task<BrandDTO> UpdateBrand(int brandId, BrandDTO brandDTO);
    }
}
