using QuitQ_Ecom.DTOs;

namespace QuitQ_Ecom.Repository
{
    public interface ISubCategory
    {
        Task<SubCategoryDTO> AddSubCategory(SubCategoryDTO subCategoryDTO);
        Task<List<SubCategoryDTO>> GetAllSubCategories();
        Task<SubCategoryDTO> GetSubCategoryById(int subCategoryId);
        Task<SubCategoryDTO> UpdateSubCategory(SubCategoryDTO subCategoryDTO);
        Task<bool> DeleteSubCategory(int subCategoryId);
    }
}
