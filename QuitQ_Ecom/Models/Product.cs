//using System;
//using System.Collections.Generic;

//namespace QuitQ_Ecom.Models
//{
//    public partial class Product
//    {
//        public Product()
//        {
//            Carts = new HashSet<Cart>();
//            Images = new HashSet<Image>();
//            OrderItems = new HashSet<OrderItem>();
//            ProductDetails = new HashSet<ProductDetail>();
//            WishLists = new HashSet<WishList>();
//        }

//        public int ProductId { get; set; }
//        public string ProductName { get; set; } = null!;
//        public int? StoreId { get; set; }
//        public int? ProductStatusId { get; set; }
//        public int? BrandId { get; set; }
//        public int? CategoryId { get; set; }
//        public int? SubCategoryId { get; set; }
//        public decimal Price { get; set; }
//        public int Quantity { get; set; }
//        public string ProductImage { get; set; } = null!;

//        public virtual Brand? Brand { get; set; }
//        public virtual Category? Category { get; set; }
//        public virtual Status? ProductStatus { get; set; }
//        public virtual Store? Store { get; set; }
//        public virtual SubCategory? SubCategory { get; set; }
//        public virtual ICollection<Cart> Carts { get; set; }
//        public virtual ICollection<Image> Images { get; set; }
//        public virtual ICollection<OrderItem> OrderItems { get; set; }
//        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
//        public virtual ICollection<WishList> WishLists { get; set; }
//    }
//}
using System;
using System.Collections.Generic;

namespace QuitQ_Ecom.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new List<Cart>();
            Images = new List<Image>();
            OrderItems = new List<OrderItem>();
            ProductDetails = new List<ProductDetail>();
            WishLists = new List<WishList>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int? StoreId { get; set; }
        public int? ProductStatusId { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductImage { get; set; } = string.Empty;

        public virtual Brand? Brand { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Status? ProductStatus { get; set; }
        public virtual Store? Store { get; set; }
        public virtual SubCategory? SubCategory { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
    }
}
