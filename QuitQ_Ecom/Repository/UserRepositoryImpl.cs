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
    public class UserRepositoryImpl : IUser
    {
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UserRepositoryImpl> _logger;

        public UserRepositoryImpl(QuitQEcomContext quitQEcomContext, IMapper mapper, ILogger<UserRepositoryImpl> logger)
        {
            _context = quitQEcomContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDTO> AddUser(UserDTO userDto)
        {
            try
            {
                if (userDto == null)
                {
                    throw new ArgumentNullException(nameof(userDto), "UserDTO cannot be null");
                }

                // Map UserDTO to User entity (assuming AutoMapper or similar mapping library is used)
                var userEntity = _mapper.Map<UserDTO, User>(userDto);

                // Add the user entity to the database context
                await _context.Users.AddAsync(userEntity);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Return the added user entity
                return _mapper.Map<UserDTO>(userEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user: {Message}", ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<UserDTO> DeleteUserById(int userId)
        {
            try
            {
                var userobj = await _context.Users.FindAsync(userId);
                if (userobj == null)
                {
                    return null;
                }
                else
                {
                    _context.Users.Remove(userobj);
                    await _context.SaveChangesAsync();
                    return _mapper.Map<UserDTO>(userobj);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user with ID {UserId}: {Message}", userId, ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<List<UserDTO>> GetUsersByUserType(int usertypeId)
        {
            try
            {
                var users = await _context.Users.Where(x => x.UserTypeId == usertypeId).ToListAsync();
                if (users == null)
                {
                    return null;
                }
                return _mapper.Map<List<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users by user type with ID {UserTypeId}: {Message}", usertypeId, ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            try
            {
                var objs = await _context.Users.ToListAsync();
                if (objs == null)
                    return null;
                return _mapper.Map<List<UserDTO>>(objs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all users: {Message}", ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            try
            {
                var UserObj = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
                return _mapper.Map<UserDTO>(UserObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user with ID {UserId}: {Message}", id, ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }
    }
}
