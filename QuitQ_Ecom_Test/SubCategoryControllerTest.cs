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
    public class SubCategoriesControllerTests
    {
        private SubCategoriesController _subCategoriesController;
        private Mock<ISubCategory> _subCategoryRepositoryMock;
        private Mock<ILogger<SubCategoriesController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _subCategoryRepositoryMock = new Mock<ISubCategory>();
            _loggerMock = new Mock<ILogger<SubCategoriesController>>();
            _subCategoriesController = new SubCategoriesController(_subCategoryRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task AddSubCategory_ReturnsOk()
        {
            // Arrange
            var subCategoryToAdd = new SubCategoryDTO { SubCategoryId = 1, SubCategoryName = "New Subcategory", CategoryId = 1 };
            _subCategoryRepositoryMock.Setup(repo => repo.AddSubCategory(subCategoryToAdd)).ReturnsAsync(subCategoryToAdd);

            // Act
            var result = await _subCategoriesController.AddSubCategory(subCategoryToAdd);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(subCategoryToAdd, okResult.Value);
        }

        [Test]
        public async Task GetAllSubCategories_ReturnsOk()
        {
            // Arrange
            var mockSubCategories = new List<SubCategoryDTO>
            {
                new SubCategoryDTO { SubCategoryId = 1, SubCategoryName = "Subcategory 1", CategoryId = 1 },
                new SubCategoryDTO { SubCategoryId = 2, SubCategoryName = "Subcategory 2", CategoryId = 2 }
            };
            _subCategoryRepositoryMock.Setup(repo => repo.GetAllSubCategories()).ReturnsAsync(mockSubCategories);

            // Act
            var result = await _subCategoriesController.GetAllSubCategories();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(mockSubCategories, okResult.Value);
        }

        [Test]
        public async Task GetSubCategoryById_ExistingId_ReturnsOk()
        {
            // Arrange
            int subCategoryId = 1;
            var mockSubCategory = new SubCategoryDTO { SubCategoryId = subCategoryId, SubCategoryName = "Subcategory 1", CategoryId = 1 };
            _subCategoryRepositoryMock.Setup(repo => repo.GetSubCategoryById(subCategoryId)).ReturnsAsync(mockSubCategory);

            // Act
            var result = await _subCategoriesController.GetSubCategoryById(subCategoryId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(mockSubCategory, okResult.Value);
        }

        [Test]
        public async Task GetSubCategoryById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int subCategoryId = 1;
            _subCategoryRepositoryMock.Setup(repo => repo.GetSubCategoryById(subCategoryId)).ReturnsAsync((SubCategoryDTO)null);

            // Act
            var result = await _subCategoriesController.GetSubCategoryById(subCategoryId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task UpdateSubCategory_ReturnsOk()
        {
            // Arrange
            int subCategoryId = 1;
            var subCategoryToUpdate = new SubCategoryDTO { SubCategoryId = subCategoryId, SubCategoryName = "Updated Subcategory", CategoryId = 1 };
            _subCategoryRepositoryMock.Setup(repo => repo.UpdateSubCategory(subCategoryToUpdate)).ReturnsAsync(subCategoryToUpdate);

            // Act
            var result = await _subCategoriesController.UpdateSubCategory(subCategoryId, subCategoryToUpdate);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(subCategoryToUpdate, okResult.Value);
        }

        [Test]
        public async Task DeleteSubCategory_ExistingId_ReturnsOk()
        {
            // Arrange
            int subCategoryId = 1;
            _subCategoryRepositoryMock.Setup(repo => repo.DeleteSubCategory(subCategoryId)).ReturnsAsync(true);

            // Act
            var result = await _subCategoriesController.DeleteSubCategory(subCategoryId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("Subcategory deleted successfully", okResult.Value);
        }

        [Test]
        public async Task DeleteSubCategory_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int subCategoryId = 1;
            _subCategoryRepositoryMock.Setup(repo => repo.DeleteSubCategory(subCategoryId)).ReturnsAsync(false);

            // Act
            var result = await _subCategoriesController.DeleteSubCategory(subCategoryId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
