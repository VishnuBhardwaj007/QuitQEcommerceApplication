using QuitQ_Ecom.DTOs;

namespace QuitQ_Ecom.Repository
{
    public interface IShipper
    {
       public Task<List<ShipperDTO> > GetAllItems();

        public Task<ShipperDTO> GetShipperItemById(int id);

        public Task<bool> UpdateShipperOrderStatusByOrderId(int id,string deleiveryStatus);

        public Task<bool> GenerateOtpAtCustomer(int shipperId);

        public Task<bool> ValidateOtp(int shipperid,string otp);



    }
}
