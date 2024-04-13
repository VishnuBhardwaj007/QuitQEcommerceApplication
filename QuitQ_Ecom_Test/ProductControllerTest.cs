using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using QuitQ_Ecom.Controllers;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;
using QuitQ_Ecom.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuitQ_Ecom_Test
{
    [TestFixture]
    public class ProductsControllerTests
    {
        private ProductsController _productsController;
        private Mock<IProduct> _productRepoMock;
        private IMapper _mapper;
        private Mock<ILogger<ProductsController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _productRepoMock = new Mock<IProduct>();
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductDTO, Product>();
            }).CreateMapper();
            _loggerMock = new Mock<ILogger<ProductsController>>();
            _productsController = new ProductsController(
                _productRepoMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task SearchProducts_ReturnsOk()
        {
            // Arrange
            string query = "test";
            var mockProducts = new List<ProductDTO>
            {
                new ProductDTO { ProductId = 1, ProductName = "Test Product 1", Price = 10.0m },
                new ProductDTO { ProductId = 2, ProductName = "Test Product 2", Price = 15.0m }
            };
            _productRepoMock.Setup(repo => repo.SearchProducts(query)).ReturnsAsync(mockProducts);

            // Act
            var result = await _productsController.SearchProducts(query);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var products = okResult.Value as List<ProductDTO>;
            Assert.IsNotNull(products);
            Assert.AreEqual(mockProducts.Count, products.Count);
            // Add further assertions as needed
        }

        [Test]
        public async Task GetProductsBySubcategoryID_ReturnsOk()
        {
            // Arrange
            int subcategoryId = 1;
            var mockProducts = new List<ProductDTO>
            {
                new ProductDTO { ProductId = 1, ProductName = "Test Product 1", Price = 10.0m },
                new ProductDTO { ProductId = 2, ProductName = "Test Product 2", Price = 15.0m }
            };
            _productRepoMock.Setup(repo => repo.GetProductsBySubCategory(subcategoryId)).ReturnsAsync(mockProducts);

            // Act
            var result = await _productsController.GetProductsBySubcategoryID(subcategoryId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var products = okResult.Value as List<ProductDTO>;
            Assert.IsNotNull(products);
            Assert.AreEqual(mockProducts.Count, products.Count);
            // Add further assertions as needed
        }

        //[Test]
        //public async Task AddProduct_ReturnsOk()
        //{
        //    // Arrange
        //    var productDto = new ProductDTO
        //    {
        //        ProductName = "Test Product",
        //        Price = 10.0m,
        //        Quantity = 100
        //    };
        //    var formFiles = new FormFileCollection();
        //    var fileStream = new MemoryStream();
        //    var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", "test.jpg")
        //    {
        //        Headers = new HeaderDictionary(),
        //        ContentType = "image/jpeg"
        //    };
        //    formFiles.Add(formFile);

        //    var formData = new ProductDTO
        //    {
        //        ProductName = "Test Product",
        //        Price = 10.0m,
        //        Quantity = 100,
        //        ProductImageFile = formFile
        //    };

        //    _productRepoMock.Setup(repo => repo.AddNewProduct(It.IsAny<ProductDTO>(), It.IsAny<List<ProductDetailDTO>>()))
        //        .ReturnsAsync(productDto);

        //    // Act
        //    var result = await _productsController.AddProduct(formData);

        //    // Assert
        //    var okResult = result as OkObjectResult;
        //    Assert.IsNull(okResult);
        //    Assert.AreEqual(200, okResult.StatusCode);
        //    Assert.AreEqual("Product added successfully", okResult.Value);
        //    // Add further assertions as needed
        //}

        //[Test]
        //public async Task GetProductById_ReturnsOk()
        //{
        //    // Arrange
        //    int productId = 1;
        //    var mockProduct = new ProductDTO
        //    {
        //        ProductId = productId,
        //        ProductName = "Test Product",
        //        Price = 10.0m
        //    };
        //    _productRepoMock.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(mockProduct);

        //    // Act
        //    var result = await _productsController.GetProductById(productId);

        //    // Assert
        //    var okResult = result as OkObjectResult;
        //    Assert.IsNotNull(okResult);
        //    Assert.AreEqual(200, okResult.StatusCode);
        //    var product = okResult.Value as ProductDTO;
        //    Assert.IsNotNull(product);
        //    Assert.AreEqual(productId, product.ProductId);
        //    // Add further assertions as needed
        //}
        [Test]
        [Ignore("checked")]
        public async Task AddProduct_ReturnsOk()
        {
            // Arrange
            var productDto = new ProductDTO
            {
                ProductName = "Test Product",
                Price = 10.0m,
                Quantity = 100
            };

            // Simulate file upload
            var formFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Fake file")), 0, 0, "Data", "fakefile.txt");
            var formData = new ProductDTO
            {
                ProductName = "Test Product",
                Price = 10.0m,
                Quantity = 100,
                ProductImageFile = formFile
            };

            var productDetailList = new List<ProductDetailDTO>
    {
        new ProductDetailDTO { Attribute = "Attribute1", Value = "Value1" },
        new ProductDetailDTO { Attribute = "Attribute2", Value = "Value2" }
    };

            _productRepoMock.Setup(repo => repo.AddNewProduct(It.IsAny<ProductDTO>(), It.IsAny<List<ProductDetailDTO>>()))
                .ReturnsAsync(productDto);

            // Act
            var result = await _productsController.AddProduct(formData);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("Product added successfully", okResult.Value);

            // Further assertions as needed
            _productRepoMock.Verify(repo => repo.AddNewProduct(It.IsAny<ProductDTO>(), It.IsAny<List<ProductDetailDTO>>()), Times.Once);
        }


        [Test]
        [Ignore("checked")]
        public async Task UpdateProduct_ReturnsOk()
        {
            // Arrange
            int productId = 1;
            var productDto = new ProductDTO
            {
                ProductName = "Test Product Updated",
                Price = 15.0m,
                Quantity = 150
            };
            var formFiles = new FormFileCollection();
            var fileStream = new MemoryStream();
            var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
            formFiles.Add(formFile);

            var formData = new ProductDTO
            {
                ProductName = "Test Product",
                Price = 10.0m,
                Quantity = 100,
                ProductImageFile = formFile
            };

            _productRepoMock.Setup(repo => repo.UpdateProduct(productId, It.IsAny<ProductDTO>(), It.IsAny<List<ProductDetailDTO>>()))
                .ReturnsAsync(productDto);

            // Act
            var result = await _productsController.UpdateProduct(productId, formData);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("product updated Successfully", okResult.Value);
            // Add further assertions as needed
        }

        [Test]
        public async Task DeleteProductByID_ReturnsOk()
        {
            // Arrange
            int productId = 1;
            _productRepoMock.Setup(repo => repo.DeleteProductByID(productId)).ReturnsAsync(true);

            // Act
            var result = await _productsController.DeleteProductByID(productId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("product deleted Successfully", okResult.Value);
            // Add further assertions as needed
        }
    }
}
