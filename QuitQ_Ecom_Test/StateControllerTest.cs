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
    public class StateControllerTests
    {
        private StateController _stateController;
        private Mock<IState> _stateRepoMock;
        private Mock<ILogger<StateController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _stateRepoMock = new Mock<IState>();
            _loggerMock = new Mock<ILogger<StateController>>();
            _stateController = new StateController(
                _stateRepoMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllStates_ReturnsOk()
        {
            // Arrange
            var mockStates = new List<StateDTO>
            {
                new StateDTO { StateId = 1, StateName = "State 1" },
                new StateDTO { StateId = 2, StateName = "State 2" }
            };
            _stateRepoMock.Setup(repo => repo.GetAllStates()).ReturnsAsync(mockStates);

            // Act
            var result = await _stateController.GetAllStates();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var states = okResult.Value as List<StateDTO>;
            Assert.IsNotNull(states);
            Assert.AreEqual(mockStates.Count, states.Count);
            // Add further assertions as needed
        }

        [Test]
        public async Task GetStateById_ReturnsOk()
        {
            // Arrange
            int stateId = 1;
            var mockState = new StateDTO { StateId = stateId, StateName = "State 1" };
            _stateRepoMock.Setup(repo => repo.GetStateById(stateId)).ReturnsAsync(mockState);

            // Act
            var result = await _stateController.GetStateById(stateId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var state = okResult.Value as StateDTO;
            Assert.IsNotNull(state);
            Assert.AreEqual(stateId, state.StateId);
            // Add further assertions as needed
        }

        [Test]
        public async Task AddState_ReturnsCreatedAtAction()
        {
            // Arrange
            var stateToAdd = new StateDTO { StateId = 1, StateName = "New State" };
            _stateRepoMock.Setup(repo => repo.AddState(stateToAdd)).ReturnsAsync(stateToAdd);

            // Act
            var result = await _stateController.AddState(stateToAdd);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(nameof(_stateController.GetStateById), createdAtActionResult.ActionName);
            var addedState = createdAtActionResult.Value as StateDTO;
            Assert.IsNotNull(addedState);
            Assert.AreEqual(stateToAdd.StateId, addedState.StateId);
            // Add further assertions as needed
        }

        [Test]
        public async Task UpdateState_ReturnsOk()
        {
            // Arrange
            int stateId = 1;
            var stateToUpdate = new StateDTO { StateId = stateId, StateName = "Updated State" };
            _stateRepoMock.Setup(repo => repo.UpdateState(stateId, stateToUpdate)).ReturnsAsync(stateToUpdate);

            // Act
            var result = await _stateController.UpdateState(stateId, stateToUpdate);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var updatedState = okResult.Value as StateDTO;
            Assert.IsNotNull(updatedState);
            Assert.AreEqual(stateId, updatedState.StateId);
            Assert.AreEqual(stateToUpdate.StateName, updatedState.StateName);
            // Add further assertions as needed
        }

        [Test]
        public async Task DeleteState_ReturnsOk()
        {
            // Arrange
            int stateId = 1;
            _stateRepoMock.Setup(repo => repo.DeleteState(stateId)).ReturnsAsync(true);

            // Act
            var result = await _stateController.DeleteState(stateId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("State deleted successfully", okResult.Value);
            // Add further assertions as needed
        }
    }
}
