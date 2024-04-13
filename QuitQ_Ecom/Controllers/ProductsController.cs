using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProduct _productRepo;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProduct product, ILogger<ProductsController> logger)
        {
            _productRepo = product;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(string query)
        {
            try
            {
                var products = await _productRepo.SearchProducts(query);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while searching products: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("subcategories/{subcategoryId:int}")]
        public async Task<IActionResult> GetProductsBySubcategoryID(int subcategoryId)
        {
            try
            {
                var products = await _productRepo.GetProductsBySubCategory(subcategoryId);
                if (products == null)
                {
                    return NotFound();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting products by subcategory ID: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "seller, admin")]
        [HttpPost("")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDTO formData)
        {
            try
            {
                // Add product logic here
                var file = Request.Form.Files[0]; // Access the uploaded file

                // Check if the file is not empty
                if (file == null || file.Length == 0)
                    return BadRequest("File is empty");
                var items = Request.Form.ToList();
                var att = new List<string>();
                var val = new List<string>();
                foreach (var item in items)
                {
                    Console.WriteLine(item.Value);
                    if (item.Key.EndsWith("Attribute"))
                    {
                        att.Add(item.Value.ToString());
                    }
                    else if (item.Key.EndsWith("Value"))
                    {
                        val.Add(item.Value.ToString());
                    }
                }
                List<ProductDetailDTO> listproductdetaildtos = new List<ProductDetailDTO>();
                int len = att.Count();
                for (int i = 0; i < len; i++)
                {
                    if ((att[i] != null && att[i] != "") && (val[i] != null && val[i] != ""))
                    {
                        var obj = new ProductDetailDTO()
                        {
                            Attribute = att[i],
                            Value = val[i]
                        };
                        listproductdetaildtos.Add(obj);
                    }
                }
                //formData.productDetailDTO = listproductdetaildtos;
                // Construct the file path for saving (you can adjust the path as needed)
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", uniqueFileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                formData.ProductImage = filePath;
                var returnedObj = await _productRepo.AddNewProduct(formData, listproductdetaildtos);
                if (returnedObj == null)
                {
                    return NotFound();
                }
                return Ok(returnedObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding product: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetProductById([FromRoute] int productId)
        {
            try
            {
                var product = await _productRepo.GetProductById(productId);
                if (product == null)
                {
                    return NotFound($"No product exists with this Id = {productId}");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting product by ID: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{productId:int}")]
        [Authorize(Roles = "seller, admin")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int productId, [FromForm] ProductDTO formData)
        {
            try
            {
                // Update product logic here
                var file = Request.Form.Files[0]; // Access the uploaded file

                // Check if the file is not empty
                if (file == null || file.Length == 0)
                    return BadRequest("File is empty");
                var items = Request.Form.ToList();
                var att = new List<string>();
                var val = new List<string>();
                foreach (var item in items)
                {
                    Console.WriteLine(item.Value);
                    if (item.Key.EndsWith("Attribute"))
                    {
                        att.Add(item.Value.ToString());
                    }
                    else if (item.Key.EndsWith("Value"))
                    {
                        val.Add(item.Value.ToString());
                    }
                }
                //see here the how the productdetails should be sent.

                List<ProductDetailDTO> listproductdetaildtos = new List<ProductDetailDTO>();
                int len = att.Count();
                for (int i = 0; i < len; i++)
                {
                    if ((att[i] != null && att[i] != "") && (val[i] != null && val[i] != ""))
                    {
                        var obj = new ProductDetailDTO()
                        {
                            Attribute = att[i],
                            Value = val[i]
                        };
                        listproductdetaildtos.Add(obj);
                    }
                }

                //formData.productDetailDTO = listproductdetaildtos;
                // Construct the file path for saving (you can adjust the path as needed)
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file.FileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                formData.ProductImage = filePath;
                var returnedObj = await _productRepo.UpdateProduct(productId, formData, listproductdetaildtos);
                if (returnedObj == null)
                {
                    return NotFound();
                }
                return Ok("Product updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating product: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{productId:int}")]
        [Authorize(Roles = "seller, admin")]
        public async Task<IActionResult> DeleteProductByID([FromRoute] int productId)
        {
            try
            {
                // Delete product logic here
                var status = await _productRepo.DeleteProductByID(productId);
                if (status)
                {
                    return Ok("product deleted Successfully");
                }
                else
                {
                    return NotFound("Product not found or deletion unsuccessful");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting product by ID: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
