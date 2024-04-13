using QuitQ_Ecom.DTOs;

namespace QuitQ_Ecom.Repository
{
    public interface ICart
    {
        Task<List<CartDTO>> GetUserCartItems(int userId);
        Task<CartDTO> AddNewProductToCart(CartDTO cartItem);
        Task<bool> IncreaseProductQuantity(int cartItemId);
        Task<bool> DecreaseProductQuantity(int cartItemId);
        Task<bool> RemoveProductFromCart(int userId, int productId);
        Task<decimal> GetTotalCartCost(int userId);

        Task<bool> RemoveCartItemsOfUser(int userId);
    }
}
