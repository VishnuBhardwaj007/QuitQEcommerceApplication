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
using System.Threading.Tasks;

namespace QuitQ_Ecom_Test
{
    [TestFixture]
    public class StoresControllerTests
    {
        private StoresController _storesController;
        private Mock<IStore> _storeRepoMock;
        private ILogger<StoresController> _logger;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _storeRepoMock = new Mock<IStore>();
            _logger = Mock.Of<ILogger<StoresController>>();
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Store, StoreDTO>();
                cfg.CreateMap<StoreDTO, Store>();
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<ProductDTO, Product>();
            }).CreateMapper();

            _storesController = new StoresController(
                _storeRepoMock.Object,
                _logger
            );
        }

        [Test]
        public async Task GetAllStores_ReturnsOk()
        {
            // Arrange
            var stores = new List<StoreDTO>
            {
                new StoreDTO { StoreId = 1, StoreName = "Store 1" },
                new StoreDTO { StoreId = 2, StoreName = "Store 2" }
            };
            _storeRepoMock.Setup(repo => repo.GetAllStores()).ReturnsAsync(stores);

            // Act
            var result = await _storesController.GetAllStores();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedStores = okResult.Value as List<StoreDTO>;
            Assert.IsNotNull(returnedStores);
            Assert.AreEqual(stores.Count, returnedStores.Count);
        }

        [Test]
        public async Task GetStoreById_ExistingId_ReturnsOk()
        {
            // Arrange
            int storeId = 1;
            var store = new StoreDTO { StoreId = storeId, StoreName = "Store 1" };
            _storeRepoMock.Setup(repo => repo.GetStoreById(storeId)).ReturnsAsync(store);

            // Act
            var result = await _storesController.GetStoreById(storeId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedStore = okResult.Value as StoreDTO;
            Assert.IsNotNull(returnedStore);
            Assert.AreEqual(store.StoreId, returnedStore.StoreId);
        }

        [Test]
        public async Task GetStoreById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int storeId = 1;
            _storeRepoMock.Setup(repo => repo.GetStoreById(storeId)).ReturnsAsync((StoreDTO)null);

            // Act
            var result = await _storesController.GetStoreById(storeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        

        // Additional tests for UpdateStore and DeleteStore methods can be added similarly
    }
}
