using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuitQ_Ecom.Repository
{
    public class WishlistRepositoryImpl : IWishlist
    {
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<WishlistRepositoryImpl> _logger;

        public WishlistRepositoryImpl(QuitQEcomContext context, IMapper mapper, ILogger<WishlistRepositoryImpl> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<WishListDTO> AddToWishList(WishListDTO wishListDTO)
        {
            try
            {
                var wishList = _mapper.Map<WishList>(wishListDTO);
                _context.WishLists.Add(wishList);
                await _context.SaveChangesAsync();
                return _mapper.Map<WishListDTO>(wishList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding to wishlist: {Message}", ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<List<WishListDTO>> GetUserWishList(int userId)
        {
            try
            {
                var userWishList = await _context.WishLists
                    .Where(w => w.UserId == userId)
                    .Include(w => w.Product)
                    .ToListAsync();

                return _mapper.Map<List<WishListDTO>>(userWishList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user's wishlist: {Message}", ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<bool> RemoveFromWishList(int userId, int productId)
        {
            try
            {
                var wishListItem = await _context.WishLists.FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);
                if (wishListItem != null)
                {
                    _context.WishLists.Remove(wishListItem);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing from wishlist: {Message}", ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }
    }
}
