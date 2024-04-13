using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuitQ_Ecom.Models
{
    public partial class Shipper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShipperId { get; set; }

            public string? ShipperName { get; set; }
            public int OrderId { get; set; }

            // Navigation property
            public virtual Order Order { get; set; }
        
    }
}
