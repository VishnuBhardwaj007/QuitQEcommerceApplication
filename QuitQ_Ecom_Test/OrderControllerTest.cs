using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using QuitQ_Ecom.Controllers;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
namespace QuitQ_Ecom_Test
{
    [TestFixture]
    public class OrderControllerTest
    {
        private Mock<IOrder> _orderRepoMock;
        private OrderController _orderController;
        private Mock<ILogger<OrderController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _orderRepoMock = new Mock<IOrder>();
            _loggerMock = new Mock<ILogger<OrderController>>();
            _orderController = new OrderController(_orderRepoMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetOrdersOfUser_ExistingUserId_ReturnsOk()
        {
            // Arrange
            int userId = 1;
            var orders = new List<OrderDTO> { new OrderDTO { OrderId = 1, UserId = userId } };
            _orderRepoMock.Setup(repo => repo.ViewAllOrdersByUserId(userId)).ReturnsAsync(orders);

            // Act
            var result = await _orderController.GetOrdersOfUser(userId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var model = okResult.Value as List<OrderDTO>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
        }

        [Test]
        public async Task GetOrdersOfUser_NonExistingUserId_ReturnsNoContent()
        {
            // Arrange
            int userId = 1;
            _orderRepoMock.Setup(repo => repo.ViewAllOrdersByUserId(userId)).ReturnsAsync((List<OrderDTO>)null);

            // Act
            var result = await _orderController.GetOrdersOfUser(userId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Orders_ExistingSellerId_ReturnsOk()
        {
            // Arrange
            int sellerId = 1;
            var orders = new List<OrderDTO> { new OrderDTO { OrderId = 1 } };
            _orderRepoMock.Setup(repo => repo.ViewOrdersBySellerId(sellerId)).ReturnsAsync(orders);

            // Act
            var result = await _orderController.Orders(sellerId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var model = okResult.Value as List<OrderDTO>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
        }

        [Test]
        public async Task Orders_NonExistingSellerId_ReturnsOk()
        {
            // Arrange
            int sellerId = 1;
            _orderRepoMock.Setup(repo => repo.ViewOrdersBySellerId(sellerId)).ReturnsAsync(new List<OrderDTO>());

            // Act
            var result = await _orderController.Orders(sellerId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var model = okResult.Value as List<OrderDTO>;
            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Count);
        }
    }
}
