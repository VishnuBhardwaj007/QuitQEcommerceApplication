using Newtonsoft.Json;

namespace QuitQ_Ecom.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public string ShippingAddress { get; set; }

        [JsonIgnore]
        public List<OrderItemDTO>? orderItemListDTOs { get; set; }
    }
}
