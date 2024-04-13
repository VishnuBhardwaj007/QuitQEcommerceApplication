using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using QuitQ_Ecom.Controllers;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuitQ_Ecom_Test
{
    [TestFixture]
    public class ShipperControllerTests
    {
        private ShipperController _shipperController;
        private Mock<IShipper> _shipperRepoMock;
        private Mock<ILogger<ShipperController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _shipperRepoMock = new Mock<IShipper>();
            _loggerMock = new Mock<ILogger<ShipperController>>();
            _shipperController = new ShipperController(
                _shipperRepoMock.Object,_loggerMock.Object);
        }

        [Test]
        public async Task GetAllItems_ReturnsOk()
        {
            // Arrange
            var mockShipperItems = new List<ShipperDTO>
{
    new ShipperDTO { ShipperId = 1, ShipperName = "Shipper 1", OrderId = 101 },
    new ShipperDTO { ShipperId = 2, ShipperName = "Shipper 2", OrderId = 102 }
};
            _shipperRepoMock.Setup(repo => repo.GetAllItems()).ReturnsAsync(mockShipperItems);

            // Act
            var result = await _shipperController.GetAllItems();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var items = okResult.Value as List<ShipperDTO>;
            Assert.IsNotNull(items);
            Assert.AreEqual(mockShipperItems.Count, items.Count);
            // Add further assertions as needed
        }

        [Test]
        public async Task GetShipItemById_ReturnsOk()
        {
            // Arrange
            int shipId = 1;
            var mockShipperItem = new ShipperDTO { ShipperId = shipId, ShipperName = "Shipper 1", OrderId = 101 };
            _shipperRepoMock.Setup(repo => repo.GetShipperItemById(shipId)).ReturnsAsync(mockShipperItem);

            // Act
            var result = await _shipperController.GetShipItemById(shipId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var item = okResult.Value as ShipperDTO;
            Assert.IsNotNull(item);
            Assert.AreEqual(shipId, item.ShipperId);
            // Add further assertions as needed
        }

        [Test]
        public async Task GenerateOtp_ReturnsOk()
        {
            // Arrange
            int shipId = 1;
            var otpStatus = true;
            _shipperRepoMock.Setup(repo => repo.GenerateOtpAtCustomer(shipId)).ReturnsAsync(otpStatus);

            // Act
            var result = await _shipperController.GenerateOtp(shipId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(otpStatus, okResult.Value);
            // Add further assertions as needed
        }

        [Test]
        public async Task ValidateOtp_ReturnsOk()
        {
            // Arrange
            var data = new ShipperDTO { ShipperId = 1, ShipperName = "Shipper 1"};
            var res = true;
            _shipperRepoMock.Setup(repo => repo.ValidateOtp(data.ShipperId, data.ShipperName)).ReturnsAsync(res);

            // Act
            var result = await _shipperController.ValidateOtp(data);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(res, okResult.Value);
            // Add further assertions as needed
        }

        [Test]
        public async Task UpdateDeliveryStatus_ReturnsOk()
        {
            // Arrange
            var data = new DeliverDTO { OrderId = 1, OrderStatus = "Delivered" };
            var res = true;
            _shipperRepoMock.Setup(repo => repo.UpdateShipperOrderStatusByOrderId(data.OrderId, data.OrderStatus)).ReturnsAsync(res);

            // Act
            var result = await _shipperController.UpdateDeliveryStatus(data);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(res, okResult.Value);
            // Add further assertions as needed
        }
    }
}
