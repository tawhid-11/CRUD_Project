using CRUD.Application.Category_Dto;
using CRUD.Application.Repositoy_Interfaces;
using CRUD.Application.Services;
using CRUD.Application.UOW_Interface;
using CRUD.Core.Entities;
using Moq;

namespace CRUD.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();

            _uowMock = new Mock<IUnitOfWork>();

            _uowMock
                .Setup(x => x.categoryRepository)
                .Returns(_categoryRepoMock.Object);

            _service = new CategoryService(_uowMock.Object);
        }
        //Categories Exist test
        [Fact]
        public async Task GetAllCategoriesAsync_Should_Return_All_Categories()
        {
            var categories = new List<Category>
    {
        new Category { Id = 1, Name = "Electronics" },
        new Category { Id = 2, Name = "Food" }
    };

            _categoryRepoMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(categories);

            var result = await _service.GetAllCategoriesAsync();

            Assert.Equal(2, result.Count());
        }
        //Empty list test
        [Fact]
        public async Task GetAllCategoriesAsync_Should_Return_Empty_List()
        {
            _categoryRepoMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Category>());

            var result = await _service.GetAllCategoriesAsync();

            Assert.Empty(result);
        }

        //Dto mapping test
        [Fact]
        public async Task GetAllCategoriesAsync_Should_Map_Data_Correctly()
        {
            var categories = new List<Category>
    {
        new Category
        {
            Id = 1,
            Name = "Electronics"
        }
    };

            _categoryRepoMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(categories);

            var result = await _service.GetAllCategoriesAsync();

            var item = result.First();

            Assert.Equal(1, item.Id);
            Assert.Equal("Electronics", item.Name);
        }

        //Category Found test
        [Fact]
        public async Task GetCategoryByIdAsync_Should_Return_Category()
        {
            var category = new Category { Id = 1, Name = "Electronics" };

            _categoryRepoMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(category);

            var result = await _service.GetCategoryByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Electronics", result.Name);
        }
        //Category Not Found test
        [Fact]
        public async Task GetCategoryByIdAsync_Should_Return_Null()
        {
            _categoryRepoMock
                .Setup(x => x.GetByIdAsync(999))
                .ReturnsAsync((Category?)null);

            var result = await _service.GetCategoryByIdAsync(999);

            Assert.Null(result);
        }

        //Mapping test
        [Fact]
        public async Task GetCategoryByIdAsync_Should_Map_Correctly()
        {
            var category = new Category
            {
                Id = 5,
                Name = "Books"
            };

            _categoryRepoMock
                .Setup(x => x.GetByIdAsync(5))
                .ReturnsAsync(category);

            var result = await _service.GetCategoryByIdAsync(5);

            Assert.Equal(5, result!.Id);
            Assert.Equal("Books", result.Name);
        }

        //Create Category test
        [Fact]
        public async Task CreateCategoryAsync_Should_Return_New_Id()
        {
            var dto = new CreateCategoryDto
            {
                Name = "Electronics"
            };

            _categoryRepoMock
                .Setup(x => x.CreateAsync(It.IsAny<Category>()))
                .ReturnsAsync(10);

            var result = await _service.CreateCategoryAsync(dto);

            Assert.Equal(10, result);
        }
        //Null name test
        [Fact]
        public async Task CreateCategoryAsync_Should_Throw_When_Name_Null()
        {
            var dto = new CreateCategoryDto
            {
                Name = null
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.CreateCategoryAsync(dto));
        }
        //Empty name test
        [Fact]
        public async Task CreateCategoryAsync_Should_Throw_When_Name_Empty()
        {
            var dto = new CreateCategoryDto
            {
                Name = ""
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.CreateCategoryAsync(dto));
        }

        //Verify repository call test
        [Fact]
        public async Task CreateCategoryAsync_Should_Call_Repository_Once()
        {
            var dto = new CreateCategoryDto
            {
                Name = "Electronics"
            };

            _categoryRepoMock
                .Setup(x => x.CreateAsync(It.IsAny<Category>()))
                .ReturnsAsync(1);

            await _service.CreateCategoryAsync(dto);

            _categoryRepoMock.Verify(
                x => x.CreateAsync(It.IsAny<Category>()),
                Times.Once);
        }

        //Update Category test
        [Fact]
        public async Task UpdateCategoryAsync_Should_Return_True()
        {
            var dto = new UpdateCategoryDto
            {
                Id = 1,
                Name = "Updated"
            };

            var category = new Category
            {
                Id = 1,
                Name = "Old"
            };

            _categoryRepoMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(category);

            _categoryRepoMock
                .Setup(x => x.UpdateAsync(It.IsAny<Category>()))
                .ReturnsAsync(true);

            var result = await _service.UpdateCategoryAsync(dto);

            Assert.True(result);
        }

        //Dto Null test
        [Fact]
        public async Task UpdateCategoryAsync_Should_Throw_When_Dto_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _service.UpdateCategoryAsync(null!));
        }
        //Empty name test
        [Fact]
        public async Task UpdateCategoryAsync_Should_Throw_When_Name_Empty()
        {
            var dto = new UpdateCategoryDto
            {
                Id = 1,
                Name = ""
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.UpdateCategoryAsync(dto));
        }
        //Category Not Found test
        [Fact]
        public async Task UpdateCategoryAsync_Should_Throw_When_Category_Not_Found()
        {
            var dto = new UpdateCategoryDto
            {
                Id = 1,
                Name = "Updated"
            };

            _categoryRepoMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Category?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.UpdateCategoryAsync(dto));
        }
        //delete category test
        [Fact]
        public async Task DeleteCategoryAsync_Should_Return_True()
        {
            var category = new Category
            {
                Id = 1,
                Name = "Electronics"
            };

            _categoryRepoMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(category);

            _categoryRepoMock
                .Setup(x => x.DeleteAsync(1))
                .ReturnsAsync(true);

            var result = await _service.DeleteCategoryAsync(1);

            Assert.True(result);
        }
        //Category Not Found test
        [Fact]
        public async Task DeleteCategoryAsync_Should_Return_False_When_Not_Found()
        {
            _categoryRepoMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Category?)null);

            var result = await _service.DeleteCategoryAsync(1);

            Assert.False(result);
        }
        //verify delete call test
        [Fact]
        public async Task DeleteCategoryAsync_Should_Call_Delete_Once()
        {
            var category = new Category
            {
                Id = 1,
                Name = "Electronics"
            };
            _categoryRepoMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(category);
            _categoryRepoMock
                .Setup(x => x.DeleteAsync(1))
                .ReturnsAsync(true);
            await _service.DeleteCategoryAsync(1);
            _categoryRepoMock.Verify(x => x.DeleteAsync(1), Times.Once);
        }
        //Search test
        [Fact]
        public async Task SearchAsync_Should_Return_Data()
        {
            var categories = new List<Category>
    {
        new Category
        {
            Id = 1,
            Name = "Electronics"
        }
    };

            _categoryRepoMock
                .Setup(x => x.SearchAsync("Elec"))
                .ReturnsAsync(categories);

            var result = await _service.SearchAsync("Elec");

            Assert.Single(result);
        }
        //Search with empty text test
        [Fact]
        public async Task SearchAsync_Should_Return_Empty_List()
        {
            _categoryRepoMock
                .Setup(x => x.SearchAsync("xyz"))
                .ReturnsAsync(new List<Category>());

            var result = await _service.SearchAsync("xyz");

            Assert.Empty(result);
        }
    }
}
