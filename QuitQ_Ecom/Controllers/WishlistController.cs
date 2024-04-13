using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/wishlist")]
    [ApiController]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlist _wishListRepo;
        private readonly ILogger<WishlistController> _logger;

        public WishlistController(IWishlist wishListRepo, ILogger<WishlistController> logger)
        {
            _wishListRepo = wishListRepo;
            _logger = logger;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserWishList(int userId)
        {
            try
            {
                var wishList = await _wishListRepo.GetUserWishList(userId);
                return Ok(wishList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user wish list for user ID {userId}: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AddToWishList([FromBody] WishListDTO wishListDTO)
        {
            try
            {
                var addedWishList = await _wishListRepo.AddToWishList(wishListDTO);
                return Ok(addedWishList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding to wishlist: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveFromWishList(int userId, int productId)
        {
            try
            {
                var isRemoved = await _wishListRepo.RemoveFromWishList(userId, productId);
                if (isRemoved)
                {
                    return Ok("Item removed from wishlist");
                }
                return NotFound("Item not found in wishlist");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while removing item from wishlist: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
