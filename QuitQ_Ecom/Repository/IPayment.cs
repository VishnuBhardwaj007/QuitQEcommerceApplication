using QuitQ_Ecom.DTOs;

namespace QuitQ_Ecom.Repository
{
    public interface IPayment
    {
        Task<PaymentDTO> AddNewPayment(OrderDTO order,string paymentType);

        //Task<string> PaymentHandle();
    }
}
