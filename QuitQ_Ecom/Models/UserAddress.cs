using System;
using System.Collections.Generic;

namespace QuitQ_Ecom.Models
{
    public partial class UserAddress
    {
        public int UserAddressId { get; set; }
        public int? UserId { get; set; }
        public string DoorNumber { get; set; } = null!;
        public string? ApartmentName { get; set; }
        public string? Landmark { get; set; }
        public string Street { get; set; } = null!;
        public int CityId { get; set; }
        public string PostalCode { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public int? StatusId { get; set; }

        public virtual City City { get; set; } = null!;
        public virtual Status? Status { get; set; }
        public virtual User? User { get; set; }
    }
}
