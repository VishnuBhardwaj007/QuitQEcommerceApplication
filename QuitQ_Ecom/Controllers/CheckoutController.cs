using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuitQ_Ecom.Models;
using Razorpay.Api;
using Newtonsoft;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api.Errors;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using Newtonsoft.Json.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Collections.Generic;

namespace QuitQ_Ecom.Controllers
{

    [Route("api/checkout")]
    [ApiController]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly string apiKey = "rzp_live_eSbRcu4kQn8Sm3";
        private readonly string apiSecret = "AJcGwBS3OuidpfkPs5effJqW";
        private readonly IOrder _orderRepo;
        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(IOrder orderRepo, ILogger<CheckoutController> logger)
        {
            _orderRepo = orderRepo;
            _logger = logger;
        }

        [HttpPost("create-order")]
        [Authorize]
        public IActionResult CreateOrder([FromBody] ra data)
        {
            try
            {
                int userId = data.userId;

                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", data.amount * 100); // Razorpay expects amount in paisa/cent
                options.Add("currency", data.currency);
                options.Add("payment_capture", 1); // Auto capture payment

                RazorpayClient client = new RazorpayClient(apiKey, apiSecret);
                Razorpay.Api.Order order = client.Order.Create(options);
                string orderId = order["id"].ToString();
                var key = apiKey;
                var amount = data.amount;
                var currency = data.currency;
                var name = "nihalpaymentsystem";
                var callbackurl = $"https://localhost:7036/api/checkout/verify-payment/?userId={userId}";

                return Ok(new { order_id = orderId, amount = amount, key = key, currency = currency, name = name, callback_url = callbackurl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating order: {Message}", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("verify-payment")]
        public async Task<IActionResult> VerifyPayment()
        {
            try
            {
                int userId = int.Parse(Request.Query["userId"]);
                Dictionary<string, string> attributes = new Dictionary<string, string>();

                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    string requestBody = await reader.ReadToEndAsync();

                    var formData = ParseFormData(requestBody);

                    string razorpayOrderId = formData["razorpay_order_id"];
                    string razorpayPaymentId = formData["razorpay_payment_id"];
                    string razorpaySignature = formData["razorpay_signature"];

                    attributes.Add("razorpay_order_id", razorpayOrderId);
                    attributes.Add("razorpay_payment_id", razorpayPaymentId);
                    attributes.Add("razorpay_signature", razorpaySignature);
                }

                Utils.verifyPaymentSignature(attributes);

                var res = await _orderRepo.PlaceOrder(userId, "online");

                return Ok(res);
            }
            catch (SignatureVerificationError ex)
            {
                _logger.LogError(ex, "Error occurred while verifying payment: {Message}", ex.Message);
                return BadRequest("Payment verification failed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred: {Message}", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private Dictionary<string, string> ParseFormData(string formData)
        {
            var pairs = formData.Split('&');
            var dict = new Dictionary<string, string>();
            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=');
                dict.Add(Uri.UnescapeDataString(keyValue[0]), Uri.UnescapeDataString(keyValue[1]));
            }
            return dict;
        }

        private string GetUserFromToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token), "JWT token is null or empty");
            }

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            var userId = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User ID claim is missing or empty");
            }

            return userId;
        }
    }
}
