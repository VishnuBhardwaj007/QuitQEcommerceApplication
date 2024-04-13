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
    public class UserAddressRepositoryImpl : IUserAddress
    {
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UserAddressRepositoryImpl> _logger;

        public UserAddressRepositoryImpl(QuitQEcomContext quitQEcomContext, IMapper mapper, ILogger<UserAddressRepositoryImpl> logger)
        {
            _context = quitQEcomContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserAddressDTO> GetActiveUserAddressByUserId(int userId)
        {
            try
            {
                var userAddressobj = await _context.UserAddresses.FirstOrDefaultAsync(x => x.UserId == userId && x.StatusId == 1);
                return _mapper.Map<UserAddressDTO>(userAddressobj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving active user address for user ID {UserId}: {Message}", userId, ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<UserAddressDTO> AddUserAddress(UserAddressDTO userAddressDTO)
        {
            try
            {
                var userAddress = _mapper.Map<UserAddress>(userAddressDTO);
                _context.UserAddresses.Add(userAddress);
                await _context.SaveChangesAsync();
                return _mapper.Map<UserAddressDTO>(userAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user address: {Message}", ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<bool> DeleteUserAddress(int userAddressId)
        {
            try
            {
                var userAddress = await _context.UserAddresses.FindAsync(userAddressId);
                if (userAddress == null)
                    return false;

                _context.UserAddresses.Remove(userAddress);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user address with ID {UserAddressId}: {Message}", userAddressId, ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<List<UserAddressDTO>> GetUserAddressesByUserId(int userId)
        {
            try
            {
                var userAddresses = await _context.UserAddresses.Where(u => u.UserId == userId).ToListAsync();
                return _mapper.Map<List<UserAddressDTO>>(userAddresses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user addresses for user ID {UserId}: {Message}", userId, ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<UserAddressDTO> UpdateUserAddress(int userAddressId, UserAddressDTO userAddressDTO)
        {
            try
            {
                var userAddress = await _context.UserAddresses.FindAsync(userAddressId);
                if (userAddress == null)
                    return null;

                userAddress = _mapper.Map(userAddressDTO, userAddress);
                await _context.SaveChangesAsync();
                return _mapper.Map<UserAddressDTO>(userAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user address: {Message}", ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }
    }
}
