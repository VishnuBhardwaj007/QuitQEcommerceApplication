using System;
using System.Collections.Generic;

namespace QuitQ_Ecom.Models
{
    public partial class Status
    {
        public Status()
        {
            Products = new HashSet<Product>();
            UserAddresses = new HashSet<UserAddress>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<UserAddress> UserAddresses { get; set; }
    }
}
