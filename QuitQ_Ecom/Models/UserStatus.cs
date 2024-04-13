using System;
using System.Collections.Generic;

namespace QuitQ_Ecom.Models
{
    public partial class UserStatus
    {
        public UserStatus()
        {
            Users = new HashSet<User>();
        }

        public int UserStatusId { get; set; }
        public string UserStatus1 { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
