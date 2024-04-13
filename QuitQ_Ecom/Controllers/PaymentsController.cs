using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;
using QuitQ_Ecom.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/payments")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPayment _paymentRepo;
        private readonly IOrder _orderRepo;
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;

        public PaymentsController(IPayment payment, IOrder order, QuitQEcomContext quitQEcomContext, IMapper mapper)
        {
            _paymentRepo = payment;
            _orderRepo = order;
            _context = quitQEcomContext;
            _mapper = mapper;
        }

        [HttpGet("cod/{userId:int}")]
        public async Task<IActionResult> PaymentHandle(int userId)
        {
            try
            {
                var res = await _orderRepo.PlaceOrder(userId, "cod");
                return Ok(res.Values.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("cartitems/{UserId:int}")]
        public async Task<IActionResult> GetAllCartItems(int UserId)
        {
            try
            {
                var cartitems = _context.Carts.Where(x => x.UserId == UserId).ToList();
                return Ok(_mapper.Map<List<CartDTO>>(cartitems));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
