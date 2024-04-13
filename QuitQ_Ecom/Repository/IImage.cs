using System.Collections.Generic;
using System.Threading.Tasks;
using QuitQ_Ecom.DTOs;

namespace QuitQ_Ecom.Repository
{
    public interface IImage
    {
        Task<IEnumerable<ImageDTO>> GetAllImages();
        Task<List<ImageDTO>> GetImageById(int productId);
        Task<ImageDTO> AddImage(ImageDTO imageDTO);
        Task<ImageDTO> UpdateImage(int imageId, ImageDTO imageDTO);
        Task<bool> DeleteImage(int imageId);
    }
}
