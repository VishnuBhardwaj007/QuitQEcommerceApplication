using QuitQ_Ecom.DTOs;

namespace QuitQ_Ecom.Repository
{
    public interface IOrder
    {
        //Task<string> PlaceOrder(int UserId);
        Task<Dictionary<bool, string>> PlaceOrder(int UserId,string paymentType);
        Task<List<OrderDTO>> ViewAllOrdersByUserId(int userId);

        Task<OrderDTO> ViewOrderByOrderId(int orderId);

        Task<List<OrderDTO>> ViewOrdersBySellerId(int sellerId);



    }
}
