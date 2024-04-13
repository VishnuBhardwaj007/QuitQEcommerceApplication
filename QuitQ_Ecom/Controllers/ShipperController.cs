using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.Threading.Tasks;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/shipment")]
    [Authorize]
    [ApiController]
    public class ShipperController : ControllerBase
    {
        private readonly IShipper _shipperRepo;
        private readonly ILogger<ShipperController> _logger;

        public ShipperController(IShipper shipper, ILogger<ShipperController> logger)
        {
            _shipperRepo = shipper;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var ListOfShipperObj = await _shipperRepo.GetAllItems();
                if (ListOfShipperObj == null)
                {
                    return NotFound();
                }
                return Ok(ListOfShipperObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting all items: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{shipId:int}")]
        public async Task<IActionResult> GetShipItemById([FromRoute] int shipId)
        {
            try
            {
                var shipperObj = await _shipperRepo.GetShipperItemById(shipId);
                if (shipperObj == null)
                {
                    return NotFound();
                }
                return Ok(shipperObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting ship item by ID: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("generateotp/{shipId:int}")]
        public async Task<IActionResult> GenerateOtp(int shipId)
        {
            try
            {
                var otpStatus = await _shipperRepo.GenerateOtpAtCustomer(shipId);
                if (otpStatus == null)
                {
                    return NotFound();
                }
                return Ok(otpStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while generating OTP: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("validateotp")]
        public async Task<IActionResult> ValidateOtp([FromBody] ShipperDTO data)
        {
            try
            {
                var res = await _shipperRepo.ValidateOtp(data.ShipperId, data.ShipperName);
                if (res == null)
                {
                    return NotFound();
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while validating OTP: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-delivery-status")]
        public async Task<IActionResult> UpdateDeliveryStatus([FromBody] DeliverDTO data)
        {
            try
            {
                var res = await _shipperRepo.UpdateShipperOrderStatusByOrderId(data.OrderId, data.OrderStatus);
                if (res == null)
                    return NotFound();
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating delivery status: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
