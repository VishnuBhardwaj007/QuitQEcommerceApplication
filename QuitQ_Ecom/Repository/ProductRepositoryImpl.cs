using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;
using Microsoft.Extensions.Logging; // Add this namespace for ILogger
using System;

namespace QuitQ_Ecom.Repository
{
    public class ProductRepositoryImpl : IProduct
    {
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductRepositoryImpl> _logger; // Add ILogger

        public ProductRepositoryImpl(QuitQEcomContext quitQEcomContext, IMapper mapper, ILogger<ProductRepositoryImpl> logger) // Add ILogger to constructor
        {
            _context = quitQEcomContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ProductDTO>> GetProductsBySubCategory(int SubcategoryId)
        {
            try
            {
                var productsOfSubcategory = await _context.Products
                    .Include(p => p.ProductDetails) // Eager loading ProductDetails
                    .Where(x => x.SubCategoryId == SubcategoryId)
                    .ToListAsync();

                if (productsOfSubcategory != null)
                {
                    return _mapper.Map<List<ProductDTO>>(productsOfSubcategory);
                }
                return new List<ProductDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving products by subcategory ID {SubcategoryId}: {ex.Message}");
                throw;
            }
        }

        public async Task<ProductDTO> AddNewProduct(ProductDTO productDTO, List<ProductDetailDTO> productDetailDTOs)
        {
            try
            {
                var storeobj = await _context.Stores.FindAsync(productDTO.StoreId);
                var sellerObj = await _context.Users.FindAsync(storeobj.SellerId);
                var product = _mapper.Map<Product>(productDTO);
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                foreach (var productDetailDTO in productDetailDTOs)
                {
                    var productDetail = new ProductDetail
                    {
                        Attribute = productDetailDTO.Attribute,
                        Value = productDetailDTO.Value
                    };

                    product.ProductDetails.Add(productDetail);
                }

                await _context.SaveChangesAsync();

                var productDtoMapped = _mapper.Map<ProductDTO>(product);
                productDtoMapped.sellerName = sellerObj.FirstName + " " + sellerObj.LastName;
                return productDtoMapped;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding new product: {ex.Message}");
                throw;
            }
        }

        public async Task<ProductDTO> GetProductById(int productId)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.ProductDetails) // Eager loading ProductDetails
                    .FirstOrDefaultAsync(p => p.ProductId == productId);

                if (product == null)
                {
                    return null ;
                }

                var storeobj = await _context.Stores.FindAsync(product.StoreId);
                var sellerObj = await _context.Users.FindAsync(storeobj.SellerId);

                var productDtoMapped = _mapper.Map<ProductDTO>(product);
                productDtoMapped.sellerName = sellerObj.FirstName + " " + sellerObj.LastName;
                return productDtoMapped;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product by ID {productId}: {ex.Message}");
                throw;
            }
        }

        public async Task<ProductDTO> UpdateProduct(int productId, ProductDTO formData, List<ProductDetailDTO> listproductdetaildtos)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(productId);

                if (existingProduct == null)
                {
                    throw new InvalidOperationException("Product not found");
                }

                _mapper.Map(formData, existingProduct);
                existingProduct.ProductDetails = _mapper.Map<List<ProductDetail>>(listproductdetaildtos);
                _context.Update(existingProduct);
                await _context.SaveChangesAsync();

                return _mapper.Map<ProductDTO>(existingProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product with ID {productId}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteProductByID(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return true; // Product deleted successfully
                }
                else
                {
                    return false; // Product not found or deletion unsuccessful
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<ProductDTO>> CheckQuantityOfProducts(List<CartDTO> cartItems)
        {
            try
            {
                List<Product> productsQuantityNotSatistfied = new List<Product>();

                foreach (var cartItem in cartItems)
                {
                    var productobj = await _context.Products.FindAsync(cartItem.ProductId);

                    if (productobj != null && productobj.Quantity < cartItem.Quantity)
                    {
                        productsQuantityNotSatistfied.Add(productobj);
                    }
                }

                return _mapper.Map<List<ProductDTO>>(productsQuantityNotSatistfied);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking quantity of products: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateQuantitiesOfProducts(List<CartDTO> cartItems)
        {
            try
            {
                var cartItemObj = _mapper.Map<List<Cart>>(cartItems);

                foreach (var item in cartItemObj)
                {
                    var productobj = await _context.Products.FindAsync(item.ProductId);

                    if (productobj != null)
                    {
                        productobj.Quantity -= item.Quantity;

                        if (productobj.Quantity <= 0)
                        {
                            productobj.ProductStatusId = 2; // Mark product status as false
                        }

                        _context.Update(productobj);
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating quantities of products: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ProductDTO>> SearchProducts(string query)
        {
            try
            {
                var products = await _context.Products
                    .Where(p => EF.Functions.Like(p.ProductName, $"%{query}%"))
                    .ToListAsync();

                return _mapper.Map<List<ProductDTO>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching products with query '{query}': {ex.Message}");
                throw;
            }
        }
    }
}
