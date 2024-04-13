using NUnit.Framework;
using QuitQ_Ecom.Controllers;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace QuitQ_Ecom_Test
{
    [TestFixture]
    public class GendersControllerTest
    {
        private Mock<IGender> _genderRepoMock;
        private Mock<ILogger<GendersController>> _loggerMock;
        private GendersController _gendersController;

        [SetUp]
        public void Setup()
        {
            _genderRepoMock = new Mock<IGender>();
            _loggerMock = new Mock<ILogger<GendersController>>();
            _gendersController = new GendersController(_genderRepoMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllGenders_Valid_ReturnsOk()
        {
            // Arrange
            var genders = new List<GenderDTO> { new GenderDTO { GenderId = 1, GenderName = "Male" } };
            _genderRepoMock.Setup(repo => repo.GetAllGenders()).ReturnsAsync(genders);

            // Act
            var result = await _gendersController.GetAllGenders();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var model = okResult.Value as List<GenderDTO>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
        }

        [Test]
        public async Task GetGenderById_ExistingId_ReturnsOk()
        {
            // Arrange
            int genderId = 1;
            var gender = new GenderDTO { GenderId = genderId, GenderName = "Male" };
            _genderRepoMock.Setup(repo => repo.GetGenderById(genderId)).ReturnsAsync(gender);

            // Act
            var result = await _gendersController.GetGenderById(genderId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var model = okResult.Value as GenderDTO;
            Assert.IsNotNull(model);
            Assert.AreEqual(genderId, model.GenderId);
        }

        [Test]
        public async Task GetGenderById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int genderId = 1;
            _genderRepoMock.Setup(repo => repo.GetGenderById(genderId)).ReturnsAsync((GenderDTO)null);

            // Act
            var result = await _gendersController.GetGenderById(genderId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task AddGender_ValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var genderDTO = new GenderDTO { GenderId = 1, GenderName = "Male" };
            _genderRepoMock.Setup(repo => repo.AddGender(genderDTO)).ReturnsAsync(genderDTO);

            // Act
            var result = await _gendersController.AddGender(genderDTO);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.AreEqual(nameof(_gendersController.GetGenderById), createdAtActionResult.ActionName);
            Assert.AreEqual(genderDTO.GenderId, createdAtActionResult.RouteValues["genderId"]);
        }

        [Test]
        public async Task UpdateGender_ValidData_ReturnsOk()
        {
            // Arrange
            int genderId = 1;
            var genderDTO = new GenderDTO { GenderId = genderId, GenderName = "Male" };
            _genderRepoMock.Setup(repo => repo.UpdateGender(genderDTO)).ReturnsAsync(genderDTO);

            // Act
            var result = await _gendersController.UpdateGender(genderId, genderDTO);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var model = okResult.Value as GenderDTO;
            Assert.IsNotNull(model);
            Assert.AreEqual(genderId, model.GenderId);
        }

        [Test]
        public async Task UpdateGender_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            int genderId = 1;
            var genderDTO = new GenderDTO { GenderId = genderId + 1, GenderName = "Male" }; // Different ID
            _gendersController.ModelState.AddModelError("GenderId", "Gender ID mismatch");

            // Act
            var result = await _gendersController.UpdateGender(genderId, genderDTO);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task DeleteGender_ExistingId_ReturnsOk()
        {
            // Arrange
            int genderId = 1;
            _genderRepoMock.Setup(repo => repo.DeleteGender(genderId)).ReturnsAsync(true);

            // Act
            var result = await _gendersController.DeleteGender(genderId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task DeleteGender_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int genderId = 1;
            _genderRepoMock.Setup(repo => repo.DeleteGender(genderId)).ReturnsAsync(false);

            // Act
            var result = await _gendersController.DeleteGender(genderId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
    }
}
