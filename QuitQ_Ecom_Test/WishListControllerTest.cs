using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using QuitQ_Ecom.Controllers;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.Threading.Tasks;

namespace QuitQ_Ecom_Test.Controllers
{
    [TestFixture]
    public class WishlistControllerTests
    {
        private Mock<IWishlist> _wishlistRepositoryMock;
        private WishlistController _wishlistController;
        private Mock<ILogger<WishlistController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _wishlistRepositoryMock = new Mock<IWishlist>();
            _loggerMock = new Mock<ILogger<WishlistController>>();
            _wishlistController = new WishlistController(_wishlistRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetUserWishList_ReturnsOk()
        {
            // Arrange
            int userId = 1;
            var wishlistItems = new List<WishListDTO> { new WishListDTO { WishListId = 1, UserId = userId, ProductId = 1 } };
            _wishlistRepositoryMock.Setup(repo => repo.GetUserWishList(userId)).ReturnsAsync(wishlistItems);

            // Act
            var result = await _wishlistController.GetUserWishList(userId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(wishlistItems, okResult.Value);
        }

        [Test]
        public async Task AddToWishList_ReturnsOk()
        {
            // Arrange
            var wishlistItemToAdd = new WishListDTO { WishListId = 1, UserId = 1, ProductId = 1 };
            _wishlistRepositoryMock.Setup(repo => repo.AddToWishList(It.IsAny<WishListDTO>())).ReturnsAsync(wishlistItemToAdd);

            // Act
            var result = await _wishlistController.AddToWishList(wishlistItemToAdd);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(wishlistItemToAdd, okResult.Value);
        }

        [Test]
        public async Task RemoveFromWishList_ReturnsOk()
        {
            // Arrange
            int userId = 1;
            int productId = 1;
            _wishlistRepositoryMock.Setup(repo => repo.RemoveFromWishList(userId, productId)).ReturnsAsync(true);

            // Act
            var result = await _wishlistController.RemoveFromWishList(userId, productId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("Item removed from wishlist", okResult.Value);
        }
    }
}

