using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;

namespace QuitQ_Ecom.Repository
{
    public interface IOrderItem
    {
        Task<bool> AddNewOrderItem(List<CartDTO> cartItems, OrderDTO orderObj);
    }
}
