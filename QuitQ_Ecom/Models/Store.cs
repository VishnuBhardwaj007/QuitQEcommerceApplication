using System;
using System.Collections.Generic;

namespace QuitQ_Ecom.Models
{
    public partial class Store
    {
        public Store()
        {
            Products = new HashSet<Product>();
        }

        public int StoreId { get; set; }
        public int? SellerId { get; set; }
        public string? StoreName { get; set; }
        public string? Description { get; set; }
        public string StoreFullAddress { get; set; } = null!;
        public int? CityId { get; set; }
        public string ContactNumber { get; set; } = null!;
        public string? StoreLogo { get; set; }

        public virtual City? City { get; set; }
        public virtual User? Seller { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
