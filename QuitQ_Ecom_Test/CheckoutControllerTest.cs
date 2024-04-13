using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using QuitQ_Ecom.Controllers;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using Razorpay.Api;
using Razorpay.Api.Errors;

namespace QuitQ_Ecom_Test
{
    [TestFixture]
    public class CheckoutControllerTests
    {
        private CheckoutController _checkoutController;
        private Mock<IOrder> _orderRepoMock;
        private Mock<ILogger<CheckoutController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _orderRepoMock = new Mock<IOrder>();
            _loggerMock = new Mock<ILogger<CheckoutController>>();
            _checkoutController = new CheckoutController(_orderRepoMock.Object, _loggerMock.Object);
        }

        [Test]
        public void CreateOrder_ValidData_ReturnsOk()
        {
            // Arrange
            var data = new ra { amount = 100, currency = "USD", userId = 1 };

            // Act
            var result = _checkoutController.CreateOrder(data);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
        }

        

        
    }
}

