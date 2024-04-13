using QuitQ_Ecom.DTOs;

namespace QuitQ_Ecom.Repository
{
    public interface IProduct
    {
        Task<ProductDTO> AddNewProduct(ProductDTO productDTO,List<ProductDetailDTO> productDetailDTOs);
        Task<List<ProductDTO>> CheckQuantityOfProducts(List<CartDTO> cartItems);
        Task<bool> DeleteProductByID(int id);
        Task<ProductDTO> GetProductById(int productId);
        Task<List<ProductDTO>> GetProductsBySubCategory(int SubcategoryId);
        Task<ProductDTO> UpdateProduct(int productId, ProductDTO formData, List<ProductDetailDTO> listproductdetaildtos);
        Task<bool> UpdateQuantitiesOfProducts(List<CartDTO> cartItems);

        Task<List<ProductDTO>> SearchProducts(string query);
    }
}
