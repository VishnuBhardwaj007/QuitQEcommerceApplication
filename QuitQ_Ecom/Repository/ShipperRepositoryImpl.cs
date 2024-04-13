using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;
using System;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Logging; // Add this namespace for ILogger

namespace QuitQ_Ecom.Repository
{
    public class ShipperRepositoryImpl : IShipper
    {
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ShipperRepositoryImpl> _logger; // Add ILogger

        public ShipperRepositoryImpl(QuitQEcomContext quitQEcomContext, IMapper mapper, ILogger<ShipperRepositoryImpl> logger) // Add ILogger to constructor
        {
            _context = quitQEcomContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> GenerateOtpAtCustomer(int shipperId)
        {
            try
            {
                var shipperobj = await _context.Shippers.FindAsync(shipperId);
                if (shipperobj == null)
                    return false;

                Random rand = new Random();
                int otp = rand.Next(100000, 999999);

                shipperobj.ShipperName = otp.ToString();

                _context.Shippers.Update(shipperobj);
                await _context.SaveChangesAsync();

                var orderObj = await _context.Orders.FindAsync(shipperobj.OrderId);

                var user = await _context.Users
                    .Where(u => u.UserId == orderObj.UserId)
                    .FirstOrDefaultAsync();

                if (user == null)
                    return false;

                string userEmail = user.Email;
                string subject = "Your OTP";
                string body = "Your OTP is: " + otp;

                

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while generating OTP for shipper ID {shipperId}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ShipperDTO>> GetAllItems()
        {
            try
            {
                var obj = await _context.Shippers.ToListAsync();
                if (obj == null)
                {
                    return null;
                }
                return _mapper.Map<List<ShipperDTO>>(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving all shippers: {ex.Message}");
                return null;
            }
        }

        public async Task<ShipperDTO> GetShipperItemById(int id)
        {
            try
            {
                var shipperObj = await _context.Shippers.FindAsync(id);
                if (shipperObj == null)
                {
                    return null;
                }

                return _mapper.Map<ShipperDTO>(shipperObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving shipper by ID {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateShipperOrderStatusByOrderId(int id, string deleiveryStatus)
        {
            try
            {
                var orderObj = await _context.Orders.FindAsync(id);
                if (orderObj == null)
                    return false;

                orderObj.OrderStatus = deleiveryStatus;
                _context.Update(orderObj);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating shipper order status for order ID {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ValidateOtp(int shipperid, string otp)
        {
            int flag = 0;
            string previousState;
            try
            {
                var shipperObj = await _context.Shippers.FindAsync(shipperid);
                if (shipperObj == null)
                    return false;

                if (shipperObj.ShipperName == otp.ToString())
                {
                    var orderObj = await _context.Orders.FindAsync(shipperObj.OrderId);
                    if (orderObj == null)
                        return false;

                    try
                    {
                        previousState = orderObj.OrderStatus;
                        orderObj.OrderStatus = "delivered";
                        _context.Update(orderObj);
                        await _context.SaveChangesAsync();
                        //here change the payments to completed

                        
                        flag = 1;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error updating order status to 'delivered' for order ID {orderObj.OrderId}: {ex.Message}");
                        return false;
                    }

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                if (flag == 1)
                {
                    // Handle changing order status back to previous state
                }
                _logger.LogError(ex, $"Error validating OTP for shipper ID {shipperid}: {ex.Message}");
                return false;
            }
        }
    }
}
