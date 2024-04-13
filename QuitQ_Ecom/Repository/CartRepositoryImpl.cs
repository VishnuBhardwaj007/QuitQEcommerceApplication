using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;
using Microsoft.Extensions.Logging;

namespace QuitQ_Ecom.Repository
{
    public class CartRepositoryImpl : ICart
    {
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CartRepositoryImpl> _logger;

        public CartRepositoryImpl(QuitQEcomContext quitQEcomContext, IMapper mapper, ILogger<CartRepositoryImpl> logger)
        {
            _context = quitQEcomContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<CartDTO>> GetUserCartItems(int userId)
        {
            try
            {
                var cartItems = await _context.Carts
                    .Where(c => c.UserId == userId)
                    .ToListAsync();

                return _mapper.Map<List<CartDTO>>(cartItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get user cart items.");
                throw;
            }
        }

        public async Task<CartDTO> AddNewProductToCart(CartDTO cartItem)
        {
            try
            {
                //if (cartItem.Quantity == 0)
                //{
                //    cartItem.Quantity = 1;
                //}
                cartItem.Quantity = 1;
                var cart = _mapper.Map<Cart>(cartItem);
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
                return _mapper.Map<CartDTO>(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add new product to cart.");
                throw;
            }
        }

        public async Task<bool> IncreaseProductQuantity(int cartItemId)
        {
            try
            {
                var cartItem = await _context.Carts.FindAsync(cartItemId);
                if (cartItem == null)
                    return false;

                cartItem.Quantity++;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to increase product quantity in cart.");
                throw;
            }
        }

        public async Task<bool> DecreaseProductQuantity(int cartItemId)
        {
            try
            {
                var cartItem = await _context.Carts.FindAsync(cartItemId);
                if (cartItem == null)
                    return false;

                cartItem.Quantity--;
                if (cartItem.Quantity <= 0)
                {
                    _context.Carts.Remove(cartItem);
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to decrease product quantity in cart.");
                throw;
            }
        }

        public async Task<bool> RemoveProductFromCart(int userId, int productId)
        {
            try
            {
                var cartItem = _context.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
                if (cartItem != null)
                {
                    _context.Carts.Remove(cartItem);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove product from cart.");
                throw;
            }
        }

        public async Task<decimal> GetTotalCartCost(int userId)
        {
            try
            {
                var cartItems = _context.Carts.Where(c => c.UserId == userId).ToList();
                decimal totalCost = 0;

                foreach (var cartItem in cartItems)
                {
                    var product = await _context.Products.FindAsync(cartItem.ProductId);
                    if (product != null)
                    {
                        totalCost += product.Price * cartItem.Quantity;
                    }
                }

                return totalCost;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate total cart cost.");
                throw;
            }
        }

        public async Task<bool> RemoveCartItemsOfUser(int userId)
        {
            try
            {
                var cartItems = await _context.Carts.Where(c => c.UserId == userId).ToListAsync();
                if (cartItems != null && cartItems.Any())
                {
                    _context.Carts.RemoveRange(cartItems);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove cart items of user.");
                throw;
            }
        }
    }
}
