using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace QuitQ_Ecom.DTOs
{
    public class ProductDetailDTO
    {
        public int? ProductDetailId { get; set; }
        public int? ProductId { get; set; }
        [JsonProperty("Attribute")]
        public string Attribute { get; set; }
        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}
