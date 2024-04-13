namespace QuitQ_Ecom.DTOs
{
    public class BrandDTO
    {
        public int? BrandId { get; set; }
        public string BrandName { get; set; }


        public IFormFile BrandLogoImg { get; set; }
        public string? BrandLogo { get; set; }
    }
}
