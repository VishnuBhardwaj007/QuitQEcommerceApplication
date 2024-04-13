using System;
using System.Collections.Generic;

namespace QuitQ_Ecom.Models
{
    public partial class Image
    {
        public int ImageId { get; set; }
        public int? ProductId { get; set; }
        public string ImageName { get; set; } = null!;
        public string StoredImage { get; set; } = null!;

        public virtual Product? Product { get; set; }
    }
}
