using System;
using System.Collections.Generic;

namespace QuitQ_Ecom.Models
{
    public partial class City
    {
        public City()
        {
            Stores = new HashSet<Store>();
            UserAddresses = new HashSet<UserAddress>();
        }

        public int CityId { get; set; }
        public string CityName { get; set; } = null!;
        public int? StateId { get; set; }

        public virtual State? State { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
        public virtual ICollection<UserAddress> UserAddresses { get; set; }
    }
}
