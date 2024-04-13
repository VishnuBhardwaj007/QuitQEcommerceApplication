using System;
using System.Collections.Generic;

namespace QuitQ_Ecom.Models
{
    public partial class UserType
    {
        public UserType()
        {
            Users = new HashSet<User>();
        }

        public int UserTypeId { get; set; }
        public string UserType1 { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
