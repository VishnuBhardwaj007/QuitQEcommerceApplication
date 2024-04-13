namespace QuitQ_Ecom.DTOs
{
    public class ImageDTO
    {
        public int? ImageId { get; set; }
        public int? ProductId { get; set; }
        public IFormFile ImageFile { get; set; }
        public string? ImageName { get; set; } = null!;
        public string? StoredImage { get; set; } = null!;
    }
}
