﻿using System;
using System.Collections.Generic;

namespace QuitQ_Ecom.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int? OrderId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string? TransactionId { get; set; }
        public string? PaymentStatus { get; set; }

        public virtual Order? Order { get; set; }
    }
}
