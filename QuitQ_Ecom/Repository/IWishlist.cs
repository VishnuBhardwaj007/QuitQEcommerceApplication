using QuitQ_Ecom.DTOs;

namespace QuitQ_Ecom.Repository
{
    public interface IWishlist
    {
        Task<List<WishListDTO>> GetUserWishList(int userId);
        Task<WishListDTO> AddToWishList(WishListDTO wishListDTO);
        Task<bool> RemoveFromWishList(int userId, int productId);
    }
}
