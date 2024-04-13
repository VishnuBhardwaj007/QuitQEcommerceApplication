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
using System.Linq;
using System.Threading.Tasks;

namespace QuitQ_Ecom_Test
{
    [TestFixture]
    public class PaymentsControllerTests
    {
        private PaymentsController _paymentsController;
        private Mock<IPayment> _paymentRepoMock;
        private Mock<IOrder> _orderRepoMock;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _paymentRepoMock = new Mock<IPayment>();
            _orderRepoMock = new Mock<IOrder>();
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cart, CartDTO>();
            }).CreateMapper();

            _paymentsController = new PaymentsController(
                _paymentRepoMock.Object,
                _orderRepoMock.Object,
                null, // You may need to mock QuitQEcomContext if necessary
                _mapper
            );
        }

        [Test]
        public async Task PaymentHandle_ReturnsOk()
        {
            // Arrange
            int userId = 1;
            _orderRepoMock.Setup(repo => repo.PlaceOrder(userId, "cod"))
                          .ReturnsAsync(new Dictionary<bool, string> { { true, "Order placed successfully" } });

            // Act
            var result = await _paymentsController.PaymentHandle(userId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual("Order placed successfully", okResult.Value);
        }

        
    }
}
