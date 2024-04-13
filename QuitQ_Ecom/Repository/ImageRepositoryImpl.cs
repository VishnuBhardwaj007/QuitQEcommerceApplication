using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;

namespace QuitQ_Ecom.Repository
{
    public class ImageRepository : IImage
    {
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ImageRepository> _logger;

        public ImageRepository(QuitQEcomContext context, IMapper mapper, ILogger<ImageRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ImageDTO>> GetAllImages()
        {
            try
            {
                var images = await _context.Images.ToListAsync();
                return _mapper.Map<IEnumerable<ImageDTO>>(images);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all images.");
                throw new Exception("Failed to get all images.", ex);
            }
        }

        public async Task<List<ImageDTO>> GetImageById(int ProductId)
        {
            try
            {
                //var image = await _context.Images.FindAsync(imageId);
                var images = _context.Images.Where(x=>x.ProductId == ProductId).ToList();
                return _mapper.Map<List<ImageDTO>>(images);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get image by ID {ImageId}.", ProductId);
                throw new Exception("Failed to get image by ID.", ex);
            }
        }

        public async Task<ImageDTO> AddImage(ImageDTO imageDTO)
        {
            try
            {
                var image = _mapper.Map<Image>(imageDTO);
                _context.Images.Add(image);
                await _context.SaveChangesAsync();
                return _mapper.Map<ImageDTO>(image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add image.");
                throw new Exception("Failed to add image.", ex);
            }
        }

        public async Task<ImageDTO> UpdateImage(int imageId, ImageDTO imageDTO)
        {
            try
            {
                var image = await _context.Images.FindAsync(imageId);
                if (image == null)
                {
                    _logger.LogWarning("Image with ID {ImageId} not found.", imageId);
                    return null;
                }

                // Update the properties of the image entity
                image.ImageName = imageDTO.ImageName;
                image.StoredImage = imageDTO.StoredImage;
                image.ProductId = imageDTO.ProductId;

                _context.Images.Update(image);
                await _context.SaveChangesAsync();

                return _mapper.Map<ImageDTO>(image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update image with ID {ImageId}.", imageId);
                throw new Exception("Failed to update image.", ex);
            }
        }

        public async Task<bool> DeleteImage(int imageId)
        {
            try
            {
                var image = await _context.Images.FindAsync(imageId);
                if (image == null)
                {
                    _logger.LogWarning("Image with ID {ImageId} not found.", imageId);
                    return false;
                }

                _context.Images.Remove(image);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete image with ID {ImageId}.", imageId);
                throw new Exception("Failed to delete image.", ex);
            }
        }
    }
}
