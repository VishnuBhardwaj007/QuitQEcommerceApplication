using QuitQ_Ecom.DTOs;

namespace QuitQ_Ecom.Repository
{
    public interface ICategory
    {
        Task<List<CategoryDTO>> GetAllCategories();

        Task<List<SubCategoryDTO>> GetSubCategoriesByCategoryId(int categoryId);

        Task<CategoryDTO> AddCategory(CategoryDTO categoryDTO);
        
        Task<CategoryDTO> GetCategoryById(int categoryId);
        Task<CategoryDTO> UpdateCategory(CategoryDTO categoryDTO);
        Task<bool> DeleteCategory(int categoryId);
        
        Task<List<ProductDTO>> GetProductsByCategory(int categoryId);


    }
}
