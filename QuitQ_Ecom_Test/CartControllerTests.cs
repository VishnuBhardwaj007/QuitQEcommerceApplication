using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuitQ_Ecom.Controllers;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.Collections.Generic;
using QuitQ_Ecom.DTOs;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace QuitQ_Ecom.Test
{
    [TestFixture]
    public class CartControllerTests
    {
        private CartController _controller;
        private Mock<ICart> _cartRepoMock;
        private Mock<ILogger<CartController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _cartRepoMock = new Mock<ICart>();
            _loggerMock = new Mock<ILogger<CartController>>();

            _controller = new CartController(_cartRepoMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetUserCartItems_WithValidUserId_ReturnsOk()
        {
            // Arrange
            int userId = 1;
            var cartItems = new List<CartDTO>(); // Mock cart items
            _cartRepoMock.Setup(repo => repo.GetUserCartItems(userId)).ReturnsAsync(cartItems);

            // Act
            var result = await _controller.GetUserCartItems(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(cartItems, okResult.Value);
        }

        [Test]
        public async Task AddProductToCart_WithValidCartItem_ReturnsOk()
        {
            // Arrange
            var cartItem = new CartDTO(); // Mock cart item
            _cartRepoMock.Setup(repo => repo.AddNewProductToCart(cartItem)).ReturnsAsync(cartItem);

            // Act
            var result = await _controller.AddProductToCart(cartItem);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(cartItem, okResult.Value);
        }

        [Test]
        public async Task IncreaseProductQuantity_WithValidCartItemId_ReturnsOk()
        {
            // Arrange
            int cartItemId = 1;
            _cartRepoMock.Setup(repo => repo.IncreaseProductQuantity(cartItemId)).ReturnsAsync(true);

            // Act
            var result = await _controller.IncreaseProductQuantity(cartItemId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Product quantity increased successfully", okResult.Value);
        }

        [Test]
        public async Task DecreaseProductQuantity_WithValidCartItemId_ReturnsOk()
        {
            // Arrange
            int cartItemId = 1;
            _cartRepoMock.Setup(repo => repo.DecreaseProductQuantity(cartItemId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DecreaseProductQuantity(cartItemId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Product quantity decreased successfully", okResult.Value);
        }

        [Test]
        public async Task GetCartTotalCost_WithValidUserId_ReturnsOk()
        {
            // Arrange
            int userId = 1;
            decimal totalCost = 100.00m; // Mock total cost
            _cartRepoMock.Setup(repo => repo.GetTotalCartCost(userId)).ReturnsAsync(totalCost);

            // Act
            var result = await _controller.GetCartTotalCost(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(totalCost, okResult.Value);
        }


    }
}