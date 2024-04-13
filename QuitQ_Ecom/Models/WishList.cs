using System;
using System.Collections.Generic;

namespace QuitQ_Ecom.Models
{
    public partial class WishList
    {
        public int WishListId { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }

        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
    }
}
