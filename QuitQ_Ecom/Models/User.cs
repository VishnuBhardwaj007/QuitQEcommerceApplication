using System;
using System.Collections.Generic;

namespace QuitQ_Ecom.Models
{
    public partial class User
    {
        public User()
        {
            Accounts = new HashSet<Account>();
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
            Stores = new HashSet<Store>();
            UserAddresses = new HashSet<UserAddress>();
            WishLists = new HashSet<WishList>();
        }

        public int UserId { get; set; }
        public int UserTypeId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string ContactNumber { get; set; } = null!;
        public int? GenderId { get; set; }
        public int? UserStatusId { get; set; }

        public virtual Gender? Gender { get; set; }
        public virtual UserStatus? UserStatus { get; set; }
        public virtual UserType? UserType { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
        public virtual ICollection<UserAddress> UserAddresses { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
    }
}
