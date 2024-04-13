using AutoMapper;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;
using Microsoft.Extensions.Logging;

namespace QuitQ_Ecom.Repository
{
    public class OrderItemRepositoryImpl : IOrderItem
    {
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderItemRepositoryImpl> _logger;

        public OrderItemRepositoryImpl(QuitQEcomContext quitQEcomContext, IMapper mapper, ILogger<OrderItemRepositoryImpl> logger)
        {
            _context = quitQEcomContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> AddNewOrderItem(List<CartDTO> cartItems, OrderDTO orderObj)
        {
            try
            {
                List<OrderItem> orderItems = new List<OrderItem>();

                foreach (var item in cartItems)
                {
                    var orderItemobj = new OrderItem()
                    {
                        OrderId = orderObj.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                    };
                    await _context.OrderItems.AddAsync(orderItemobj);
                    orderItems.Add(orderItemobj);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new order items.");
                throw;
            }
        }
    }
}
