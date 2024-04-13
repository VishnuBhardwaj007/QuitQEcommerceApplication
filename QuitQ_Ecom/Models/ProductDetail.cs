using System;
using System.Collections.Generic;

namespace QuitQ_Ecom.Models
{
    public partial class ProductDetail
    {
        public int ProductDetailId { get; set; }
        public int? ProductId { get; set; }
        public string Attribute { get; set; } = null!;
        public string Value { get; set; } = null!;

        public virtual Product? Product { get; set; }
    }
}
