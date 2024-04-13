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
    public class UsersControllerTest
    {
        private UsersController _usersController;
        private Mock<IUser> _userRepositoryMock;
        private Mock<ILogger<UsersController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUser>();
            _loggerMock = new Mock<ILogger<UsersController>>();
            _usersController = new UsersController(_userRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Register_ValidUser_ReturnsOk()
        {
            // Arrange
            var userDTO = new UserDTO
            {
                Username = "testuser",
                Password = "password123",
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User",
                Dob = new DateTime(2000, 1, 1),
                ContactNumber = "1234567890",
                UserTypeId = 1
            };
            _userRepositoryMock.Setup(repo => repo.AddUser(userDTO)).ReturnsAsync(userDTO);

            // Act
            var result = await _usersController.Register(userDTO);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(userDTO, okResult.Value);
        }

        [Test]
        public async Task GetAllUsers_ReturnsOk()
        {
            // Arrange
            var users = new List<UserDTO>
            {
                new UserDTO { UserId = 1, Username = "user1" },
                new UserDTO { UserId = 2, Username = "user2" }
            };
            _userRepositoryMock.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _usersController.GetAllUsers();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(users, okResult.Value);
        }

        [Test]
        public async Task GetUserByUserType_ReturnsOk()
        {
            // Arrange
            int userTypeId = 1;
            var users = new List<UserDTO>
            {
                new UserDTO { UserId = 1, Username = "user1", UserTypeId = userTypeId },
                new UserDTO { UserId = 2, Username = "user2", UserTypeId = userTypeId }
            };
            _userRepositoryMock.Setup(repo => repo.GetUsersByUserType(userTypeId)).ReturnsAsync(users);

            // Act
            var result = await _usersController.GetUserByUserType(userTypeId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(users, okResult.Value);
        }
        [Test]
        public async Task DeleteUserById_ReturnsOk()
        {
            // Arrange
            int userId = 1;
            var userDto = new UserDTO { /* Initialize userDTO as needed */ };
            _userRepositoryMock.Setup(repo => repo.DeleteUserById(userId)).ReturnsAsync(userDto);

            // Act
            var result = await _usersController.DeleteUserById(userId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("User deleted successfully.", okResult.Value);
        }

        [Test]
        public async Task GetUserById_ReturnsOk()
        {
            // Arrange
            int userId = 1;
            var userDTO = new UserDTO { UserId = userId, Username = "user1" };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(userDTO);

            // Act
            var result = await _usersController.GetUserById(userId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(userDTO, okResult.Value);
        }

        // Additional test methods can be added for edge cases and error handling

        [TearDown]
        public void Teardown()
        {
            // Clean up resources if needed
        }
    }
}
