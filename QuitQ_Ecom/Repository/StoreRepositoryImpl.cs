using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Add this namespace for ILogger

namespace QuitQ_Ecom.Repository
{
    public class StoreRepositoryImpl : IStore
    {
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<StoreRepositoryImpl> _logger; // Add ILogger

        public StoreRepositoryImpl(QuitQEcomContext quitQEcomContext, IMapper mapper, ILogger<StoreRepositoryImpl> logger) // Add ILogger to constructor
        {
            _context = quitQEcomContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<StoreDTO> AddStore(StoreDTO storeDTO)
        {
            try
            {
                var store = _mapper.Map<Store>(storeDTO);
                await _context.Stores.AddAsync(store);
                await _context.SaveChangesAsync();
                return _mapper.Map<StoreDTO>(store);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding the store: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        public async Task<bool> DeleteStore(int storeId)
        {
            try
            {
                var store = await _context.Stores.FindAsync(storeId);
                if (store == null)
                    return false;
                var productsToRemove = _context.Products.Where(p => p.StoreId == storeId);
                _context.Products.RemoveRange(productsToRemove);
                _context.Stores.Remove(store);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the store: {ex.Message}");
                return false;
            }
        }

        public async Task<List<StoreDTO>> GetAllStores()
        {
            try
            {
                var stores = await _context.Stores.ToListAsync();
                return _mapper.Map<List<StoreDTO>>(stores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving all stores: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        public async Task<StoreDTO> GetStoreById(int storeId)
        {
            try
            {
                var store = await _context.Stores.FindAsync(storeId);
                return _mapper.Map<StoreDTO>(store);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the store with ID {storeId}: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        public async Task<List<ProductDTO>> GetProductsByStore(int storeId)
        {
            try
            {
                var products = await _context.Products.Where(p => p.StoreId == storeId).ToListAsync();
                return _mapper.Map<List<ProductDTO>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving products for the store with ID {storeId}: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        public async Task<StoreDTO> UpdateStore(int storeId, StoreDTO storeDTO)
        {
            try
            {
                var store = await _context.Stores.FindAsync(storeId);
                if (store == null)
                    throw new Exception("Store not found");

                // Update store properties from the DTO
                _mapper.Map(storeDTO, store);

                // Check if there's a new store logo image
                if (storeDTO.StoreImageFile != null)
                {
                    // Construct the file path for saving
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + storeDTO.StoreImageFile.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", uniqueFileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await storeDTO.StoreImageFile.CopyToAsync(stream);
                    }

                    // Update the store logo path
                    store.StoreLogo = filePath;
                }

                _context.Stores.Update(store);
                await _context.SaveChangesAsync();

                return _mapper.Map<StoreDTO>(store);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the store with ID {storeId}: {ex.Message}");
                throw; // Re-throw the exception
            }
        }
    }
}
