using QuitQ_Ecom.Models;

namespace QuitQ_Ecom.DTOs
{
    public class OrderItemDTO
    {
        public int OrderItemId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int Quantity { get; set; }
        
        
    }
}
