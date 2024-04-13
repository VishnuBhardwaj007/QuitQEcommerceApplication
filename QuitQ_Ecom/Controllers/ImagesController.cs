using Microsoft.AspNetCore.Mvc;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/images")]
    [ApiController]
    [Authorize(Roles= "seller, admin")]
    public class ImagesController : ControllerBase
    {
        private readonly IImage _imageRepo;
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(IImage imageRepo, ILogger<ImagesController> logger)
        {
            _imageRepo = imageRepo;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllImages()
        {
            try
            {
                var images = await _imageRepo.GetAllImages();
                return Ok(images);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving all images: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("{ProductId:int}")]
        public async Task<IActionResult> GetImageByProductId([FromRoute] int ProductId)
        {
            try
            {
                var image = await _imageRepo.GetImageById(ProductId);
                if (image == null)
                {
                    _logger.LogError($"Image with ID {ProductId} not found.");
                    return NotFound();
                }
                return Ok(image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the image with ID {ProductId}: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AddImage([FromForm] ImageDTO imageDTO)
        {
            try
            {
                var file = imageDTO.ImageFile; // Access the uploaded file

                // Check if the file is not empty
                if (file == null || file.Length == 0)
                {
                    _logger.LogError("Image file is empty.");
                    return BadRequest("Image file is empty");
                }

                // Construct the file path for saving
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", uniqueFileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Update the ImageDTO with the file path
                imageDTO.StoredImage = filePath;

                // Add the image to the repository
                var returnedObj = await _imageRepo.AddImage(imageDTO);
                if (returnedObj == null)
                {
                    _logger.LogError($"Failed to add image.");
                    return NotFound();
                }

                return Ok("Image added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding the image: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPut("{imageId:int}")]
        public async Task<IActionResult> UpdateImage([FromRoute] int imageId, [FromForm] ImageDTO imageDTO)
        {
            try
            {
                var file = imageDTO.ImageFile; // Access the uploaded file

                // Check if the file is not empty
                if (file == null || file.Length == 0)
                {
                    _logger.LogError("Image file is empty.");
                    return BadRequest("Image file is empty");
                }

                // Construct the file path for saving
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", uniqueFileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Update the ImageDTO with the file path
                imageDTO.StoredImage = filePath;

                // Update the image in the repository
                var returnedObj = await _imageRepo.UpdateImage(imageId, imageDTO);
                if (returnedObj == null)
                {
                    _logger.LogError($"Failed to update image with ID {imageId}.");
                    return NotFound();
                }

                return Ok("Image updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the image: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete("{imageId:int}")]
        public async Task<IActionResult> DeleteImage([FromRoute] int imageId)
        {
            try
            {
                var deleted = await _imageRepo.DeleteImage(imageId);
                if (!deleted)
                {
                    _logger.LogError($"Image with ID {imageId} not found.");
                    return NotFound();
                }

                return Ok("Image deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the image: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
