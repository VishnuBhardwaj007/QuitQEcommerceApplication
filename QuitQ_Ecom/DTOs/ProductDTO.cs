using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace QuitQ_Ecom.DTOs
{
    public class ProductDTO
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile? ProductImageFile { get; set; }

        //public List<IFormFile>? ImageFiles { get; set; }
        public string? ProductImage { get; set; }
        public int? StoreId { get; set; }
        public int? ProductStatusId { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public string? sellerName { get; set; }
        [JsonIgnore]
        public List<ProductDetailDTO>? ProductDetails { get; set; }

        //public List<ImageDTO>? Images { get; set; }

        //public static ProductDTO FromForm(IFormCollection form)
        //{
        //    return new ProductDTO
        //    {
        //        ProductName = form["ProductName"],
        //        StoreId = form.ContainsKey("StoreId") ? Convert.ToInt32(form["StoreId"]) : null,
        //        ProductStatusId = form.ContainsKey("ProductStatusId") ? Convert.ToInt32(form["ProductStatusId"]) : null,
        //        BrandId = form.ContainsKey("BrandId") ? Convert.ToInt32(form["BrandId"]) : null,
        //        CategoryId = form.ContainsKey("CategoryId") ? Convert.ToInt32(form["CategoryId"]) : null,
        //        SubCategoryId = form.ContainsKey("SubCategoryId") ? Convert.ToInt32(form["SubCategoryId"]) : null,
        //        Price = Convert.ToDecimal(form["Price"]),
        //        Quantity = Convert.ToInt32(form["Quantity"]),
        //        //ProductDetails = JsonConvert.DeserializeObject<List<ProductDetailDTO>>(form["ProductDetails"])
        //        ProductDetails = form["ProductDetails"];



        //    };
        
    }
}

