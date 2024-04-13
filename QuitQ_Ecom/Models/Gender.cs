using System;
using System.Collections.Generic;

namespace QuitQ_Ecom.Models
{
    public partial class Gender
    {
        public Gender()
        {
            Users = new HashSet<User>();
        }

        public int GenderId { get; set; }
        public string GenderName { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
