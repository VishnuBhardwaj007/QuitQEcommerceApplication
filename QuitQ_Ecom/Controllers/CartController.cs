using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.Threading.Tasks;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICart _cartRepo;
        private readonly ILogger<CartController> _logger;

        public CartController(ICart cartRepo, ILogger<CartController> logger)
        {
            _cartRepo = cartRepo;
            _logger = logger;
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUserCartItems(int userId)
        {
            try
            {
                var cartItems = await _cartRepo.GetUserCartItems(userId);
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user cart items: {Message}", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProductToCart([FromBody] CartDTO cartItem)
        {
            try
            {
                var addedCartItem = await _cartRepo.AddNewProductToCart(cartItem);
                return Ok(addedCartItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding product to cart: {Message}", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("increase-quantity/{cartItemId:int}")]
        public async Task<IActionResult> IncreaseProductQuantity(int cartItemId)
        {
            try
            {
                var success = await _cartRepo.IncreaseProductQuantity(cartItemId);
                if (success)
                    return Ok("Product quantity increased successfully");
                else
                    return NotFound("Cart item not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while increasing product quantity: {Message}", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("decrease-quantity/{cartItemId:int}")]
        public async Task<IActionResult> DecreaseProductQuantity(int cartItemId)
        {
            try
            {
                var success = await _cartRepo.DecreaseProductQuantity(cartItemId);
                if (success)
                    return Ok("Product quantity decreased successfully");
                else
                    return NotFound("Cart item not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while decreasing product quantity: {Message}", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("totalcost/{userId}")]
        public async Task<IActionResult> GetCartTotalCost(int userId)
        {
            try
            {
                decimal totalCost = await _cartRepo.GetTotalCartCost(userId);
                return Ok(totalCost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting cart total cost: {Message}", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
