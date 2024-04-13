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
    public class CategoriesControllerTests
    {
        private CategoriesController _categoriesController;
        private Mock<ICategory> _categoryRepoMock;
        private Mock<ILogger<CategoriesController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _categoryRepoMock = new Mock<ICategory>();
            _loggerMock = new Mock<ILogger<CategoriesController>>();
            _categoriesController = new CategoriesController(_categoryRepoMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllCategories_ValidData_ReturnsOk()
        {
            // Arrange
            _categoryRepoMock.Setup(repo => repo.GetAllCategories()).ReturnsAsync(new List<CategoryDTO>());

            // Act
            var result = await _categoriesController.GetAllCategories();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
        }

        [Test]
        public async Task GetCategoryById_ValidId_ReturnsOk()
        {
            // Arrange
            int categoryId = 1;
            _categoryRepoMock.Setup(repo => repo.GetCategoryById(categoryId)).ReturnsAsync(new CategoryDTO { CategoryId = categoryId, CategoryName = "TestCategory" });

            // Act
            var result = await _categoriesController.GetCategoryById(categoryId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
        }

        [Test]
        public async Task AddCategory_ValidData_ReturnsCreated()
        {
            // Arrange
            var categoryDTO = new CategoryDTO { CategoryId = 1, CategoryName = "TestCategory" };
            _categoryRepoMock.Setup(repo => repo.AddCategory(categoryDTO)).ReturnsAsync(categoryDTO);

            // Act
            var result = await _categoriesController.AddCategory(categoryDTO);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdResult = result as CreatedAtActionResult;
            Assert.AreEqual(nameof(CategoriesController.GetCategoryById), createdResult.ActionName);
        }

        [Test]
        public async Task UpdateCategory_ValidData_ReturnsOk()
        {
            // Arrange
            int categoryId = 1;
            var categoryDTO = new CategoryDTO { CategoryId = categoryId, CategoryName = "TestCategory" };
            _categoryRepoMock.Setup(repo => repo.UpdateCategory(categoryDTO)).ReturnsAsync(categoryDTO);

            // Act
            var result = await _categoriesController.UpdateCategory(categoryId, categoryDTO);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(categoryDTO, okResult.Value);
        }

        [Test]
        public async Task DeleteCategory_ValidId_ReturnsOk()
        {
            // Arrange
            int categoryId = 1;
            _categoryRepoMock.Setup(repo => repo.DeleteCategory(categoryId)).ReturnsAsync(true);

            // Act
            var result = await _categoriesController.DeleteCategory(categoryId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual($"Category with ID {categoryId} deleted successfully.", okResult.Value);
        }

        [Test]
        public async Task GetSubcategoriesByCategory_ValidId_ReturnsOk()
        {
            // Arrange
            int categoryId = 1;
            _categoryRepoMock.Setup(repo => repo.GetSubCategoriesByCategoryId(categoryId)).ReturnsAsync(new List<SubCategoryDTO>());

            // Act
            var result = await _categoriesController.GetSubcategoriesByCategory(categoryId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
        }

        [Test]
        public async Task GetProductsByCategory_ValidId_ReturnsOk()
        {
            // Arrange
            int categoryId = 1;
            _categoryRepoMock.Setup(repo => repo.GetProductsByCategory(categoryId)).ReturnsAsync(new List<ProductDTO>());

            // Act
            var result = await _categoriesController.GetProductsByCategory(categoryId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
        }
    }
}
