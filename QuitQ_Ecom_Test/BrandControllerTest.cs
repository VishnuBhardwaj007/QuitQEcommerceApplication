using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
using System.Threading.Tasks;

namespace QuitQ_Ecom_Test
{
    [TestFixture]
    public class BrandsControllerTests
    {
        private BrandsController _brandsController;
        private Mock<IBrand> _brandRepoMock;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _brandRepoMock = new Mock<IBrand>();
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Brand, BrandDTO>();
                cfg.CreateMap<BrandDTO, Brand>();
            }).CreateMapper();

            _brandsController = new BrandsController(
                _brandRepoMock.Object,
                null // Mock ILogger if needed
            );
        }

        [Test]
        public async Task GetAllBrands_ReturnsOk()
        {
            // Arrange
            var expectedBrands = new List<BrandDTO>
            {
                new BrandDTO { BrandId = 1, BrandName = "Brand 1" },
                new BrandDTO { BrandId = 2, BrandName = "Brand 2" }
            };
            _brandRepoMock.Setup(repo => repo.GetAllBrands()).ReturnsAsync(expectedBrands);

            // Act
            var result = await _brandsController.GetAllBrands();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOf<List<BrandDTO>>(okResult.Value);
            var actualBrands = okResult.Value as List<BrandDTO>;
            Assert.AreEqual(expectedBrands.Count, actualBrands.Count);
            for (int i = 0; i < expectedBrands.Count; i++)
            {
                Assert.AreEqual(expectedBrands[i].BrandId, actualBrands[i].BrandId);
                Assert.AreEqual(expectedBrands[i].BrandName, actualBrands[i].BrandName);
            }
        }

        [Test]
        public async Task GetBrandById_ExistingId_ReturnsOk()
        {
            // Arrange
            int brandId = 1;
            var expectedBrand = new BrandDTO { BrandId = brandId, BrandName = "Brand 1" };
            _brandRepoMock.Setup(repo => repo.GetBrandById(brandId)).ReturnsAsync(expectedBrand);

            // Act
            var result = await _brandsController.GetBrandById(brandId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOf<BrandDTO>(okResult.Value);
            var actualBrand = okResult.Value as BrandDTO;
            Assert.AreEqual(expectedBrand.BrandId, actualBrand.BrandId);
            Assert.AreEqual(expectedBrand.BrandName, actualBrand.BrandName);
        }

        

        

        [Test]
        public async Task DeleteBrand_ExistingId_ReturnsOk()
        {
            // Arrange
            int brandId = 1;
            _brandRepoMock.Setup(repo => repo.DeleteBrand(brandId)).ReturnsAsync(true);

            // Act
            var result = await _brandsController.DeleteBrand(brandId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual($"Brand with ID {brandId} deleted successfully.", okResult.Value);
        }
    }
}
