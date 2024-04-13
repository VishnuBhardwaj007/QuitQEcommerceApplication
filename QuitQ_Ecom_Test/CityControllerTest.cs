using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using QuitQ_Ecom.Controllers;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using Microsoft.Extensions.Logging;

namespace QuitQ_Ecom_Test
{
    [TestFixture]
    public class CityControllerTest
    {
        private Mock<ICity> _cityRepoMock;
        private CityController _cityController;
        private Mock<ILogger<CityController>> _loggerMock;
        [SetUp]
        public void Setup()
        {
            _cityRepoMock = new Mock<ICity>();
            _loggerMock = new Mock<ILogger<CityController>>();
            _cityController = new CityController(_cityRepoMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllCities_Valid_ReturnsOk()
        {
            // Arrange
            var cities = new List<CityDTO> { new CityDTO { CityId = 1, CityName = "City1", StateId = 1 } };
            _cityRepoMock.Setup(repo => repo.GetAllCities()).ReturnsAsync(cities);

            // Act
            var result = await _cityController.GetAllCities();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var model = okResult.Value as List<CityDTO>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
        }

        [Test]
        public async Task GetCityById_ExistingId_ReturnsOk()
        {
            // Arrange
            int cityId = 1;
            var city = new CityDTO { CityId = cityId, CityName = "City1", StateId = 1 };
            _cityRepoMock.Setup(repo => repo.GetCityById(cityId)).ReturnsAsync(city);

            // Act
            var result = await _cityController.GetCityById(cityId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var model = okResult.Value as CityDTO;
            Assert.IsNotNull(model);
            Assert.AreEqual(cityId, model.CityId);
        }

        [Test]
        public async Task GetCityById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int cityId = 1;
            _cityRepoMock.Setup(repo => repo.GetCityById(cityId)).ReturnsAsync((CityDTO)null);

            // Act
            var result = await _cityController.GetCityById(cityId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task AddCity_ValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var cityDTO = new CityDTO { CityId = 1, CityName = "City1", StateId = 1 };
            _cityRepoMock.Setup(repo => repo.AddCity(cityDTO)).ReturnsAsync(cityDTO);

            // Act
            var result = await _cityController.AddCity(cityDTO);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.AreEqual(nameof(_cityController.GetCityById), createdAtActionResult.ActionName);
            Assert.AreEqual(cityDTO.CityId, createdAtActionResult.RouteValues["cityId"]);
        }

        [Test]
        public async Task UpdateCityState_ValidData_ReturnsOk()
        {
            // Arrange
            int cityId = 1;
            int stateId = 2;
            var updatedCity = new CityDTO { CityId = cityId, CityName = "City1", StateId = stateId };
            _cityRepoMock.Setup(repo => repo.UpdateCityState(cityId, stateId)).ReturnsAsync(updatedCity);

            // Act
            var result = await _cityController.UpdateCityState(cityId, stateId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var model = okResult.Value as CityDTO;
            Assert.IsNotNull(model);
            Assert.AreEqual(cityId, model.CityId);
            Assert.AreEqual(stateId, model.StateId);
        }

        [Test]
        public async Task UpdateCityState_InvalidData_ReturnsNotFound()
        {
            // Arrange
            int cityId = 1;
            int stateId = 2;
            _cityRepoMock.Setup(repo => repo.UpdateCityState(cityId, stateId)).ReturnsAsync((CityDTO)null);

            // Act
            var result = await _cityController.UpdateCityState(cityId, stateId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task DeleteCity_ExistingId_ReturnsOk()
        {
            // Arrange
            int cityId = 1;
            _cityRepoMock.Setup(repo => repo.DeleteCity(cityId)).ReturnsAsync(true);

            // Act
            var result = await _cityController.DeleteCity(cityId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task DeleteCity_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int cityId = 1;
            _cityRepoMock.Setup(repo => repo.DeleteCity(cityId)).ReturnsAsync(false);

            // Act
            var result = await _cityController.DeleteCity(cityId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
    }
}
