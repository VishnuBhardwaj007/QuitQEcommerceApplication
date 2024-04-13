using Microsoft.AspNetCore.Mvc;
using QuitQ_Ecom.Repository;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/order")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _orderRepo;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrder order, ILogger<OrderController> logger)
        {
            _orderRepo = order;
            _logger = logger;
        }

        [HttpGet("all/{userId:int}")]
        public async Task<IActionResult> GetOrdersOfUser(int userId)
        {
            try
            {
                var res = await _orderRepo.ViewAllOrdersByUserId(userId);
                if (res == null)
                {
                    _logger.LogInformation($"No orders found for user with ID {userId}");
                    return NoContent();
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching orders for user with ID {userId}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("sellerOrders/{sellerId:int}")]
        public async Task<IActionResult> Orders(int sellerId)
        {
            try
            {
                var orders = await _orderRepo.ViewOrdersBySellerId(sellerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching orders for seller with ID {sellerId}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
