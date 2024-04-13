namespace QuitQ_Ecom.DTOs
{
    public class StoreDTO
    {
        public int? StoreId { get; set; }
        public int? SellerId { get; set; }
        public string? StoreName { get; set; }
        public string? Description { get; set; }
        public IFormFile StoreImageFile { get; set; }
        public string? StoreLogo { get; set; }
        public string StoreFullAddress { get; set; }
        public int? CityId { get; set; }
        public string ContactNumber { get; set; }
    }
}
