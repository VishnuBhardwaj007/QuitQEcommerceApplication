using QuitQ_Ecom.DTOs;
using System;
using System.Collections.Generic;

namespace QuitQ_Ecom.Models
{
    public partial class OrderItem
    {
        public int OrderItemId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int Quantity { get; set; }

        //public ProductDTO? product { get; set; }
        

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
