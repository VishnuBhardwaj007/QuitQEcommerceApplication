using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.IO;
using System.Threading.Tasks;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/stores")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStore _storeRepo;
        private readonly ILogger<StoresController> _logger;

        public StoresController(IStore storeRepo, ILogger<StoresController> logger)
        {
            _storeRepo = storeRepo;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllStores()
        {
            try
            {
                var stores = await _storeRepo.GetAllStores();
                return Ok(stores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting all stores: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{storeId:int}")]
        public async Task<IActionResult> GetStoreById([FromRoute] int storeId)
        {
            try
            {
                var store = await _storeRepo.GetStoreById(storeId);
                if (store == null)
                    return NotFound();
                return Ok(store);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting store by ID: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("")]
        [Authorize(Roles = "seller, admin")]
        public async Task<IActionResult> AddStore([FromForm] StoreDTO storeDTO)
        {
            try
            {
                var file = storeDTO.StoreImageFile; // Access the uploaded file from the StoreDTO

                // Check if the file is not empty
                if (file == null || file.Length == 0)
                    return BadRequest("Store image file is empty");

                // Construct the file path for saving
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", uniqueFileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Update the StoreDTO with the file path
                storeDTO.StoreLogo = filePath;

                // Add the store to the repository
                var returnedObj = await _storeRepo.AddStore(storeDTO);
                if (returnedObj == null)
                {
                    return NotFound();
                }

                return Ok("Store added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding store: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPut("{storeId:int}")]
        [Authorize(Roles = "seller, admin")]
        public async Task<IActionResult> UpdateStore([FromRoute] int storeId, [FromForm] StoreDTO storeDTO)
        {
            try
            {
                var file = storeDTO.StoreImageFile; // Access the uploaded file from the StoreDTO

                // Check if the file is not empty
                if (file == null || file.Length == 0)
                    return BadRequest("File is empty");

                // Construct the file path for saving
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", uniqueFileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Update the StoreDTO with the file path
                storeDTO.StoreLogo = filePath;

                // Update the store in the repository
                var returnedObj = await _storeRepo.UpdateStore(storeId, storeDTO);
                if (returnedObj == null)
                {
                    return NotFound();
                }

                return Ok("Store updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating store: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete("{storeId}")]
        [Authorize(Roles = "seller, admin")]
        public async Task<IActionResult> DeleteStore([FromRoute] int storeId)
        {
            try
            {
                var deleted = await _storeRepo.DeleteStore(storeId);
                if (!deleted)
                    return NotFound();

                return Ok("Store deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting store: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
