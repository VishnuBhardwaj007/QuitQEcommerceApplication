using AutoMapper;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;
using Razorpay.Api;

namespace QuitQ_Ecom.Repository
{
    public class PaymentRepositoryImpl:IPayment
    {
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;


        private readonly ILogger<PaymentRepositoryImpl> _logger; // Add ILogger

        public PaymentRepositoryImpl(QuitQEcomContext quitQEcomContext, IMapper mapper, ILogger<PaymentRepositoryImpl> logger) // Add ILogger to constructor
        {
            _context = quitQEcomContext;
            _mapper = mapper;
            _logger = logger;
        }


        //this method is to create a new entry for the cod payment
        public async Task<PaymentDTO> AddNewPayment(OrderDTO order,string paymentType)
        {
            try
            {
                string paymentStatus;
                Models.Payment paymentobj;
                if (paymentType == "cod")
                {
                    paymentStatus = "pending";
                    paymentobj = new Models.Payment()
                    {
                        OrderId = order.OrderId,
                        Amount = order.TotalAmount,
                        PaymentMethod = paymentType,
                        PaymentStatus = paymentStatus
                    };
                }
                else
                {
                    paymentStatus = "completed";
                    paymentobj = new Models.Payment()
                    {
                        OrderId = order.OrderId,
                        Amount = order.TotalAmount,
                        PaymentMethod = paymentType,
                        PaymentStatus = paymentStatus,
                        PaymentDate = DateTime.Now
                    };
                }
                
                await _context.Payments.AddAsync(paymentobj);
                await _context.SaveChangesAsync();
                return _mapper.Map<PaymentDTO>(paymentobj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding new payment for order ID {order.OrderId}: {ex.Message}");
                return null;
            }
        }


        //you need updatepayment to set the fields of paymentdate,transactionid,paymentstatus, when the user gets the order
    }
}
