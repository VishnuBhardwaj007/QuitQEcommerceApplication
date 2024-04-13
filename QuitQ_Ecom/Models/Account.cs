using System;
using System.Collections.Generic;

namespace QuitQ_Ecom.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public int? UserId { get; set; }
        public string AccountNumber { get; set; } = null!;
        public string BankName { get; set; } = null!;
        public string BranchName { get; set; } = null!;
        public string AccountHolderName { get; set; } = null!;
        public string AccountType { get; set; } = null!;
        public string IfscCode { get; set; } = null!;

        public virtual User? User { get; set; }
    }
}
