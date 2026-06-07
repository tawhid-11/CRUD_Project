using CRUD.API.Controllers;
using CRUD.Application.Category_Dto;
using CRUD.Application.Repositoy_Interfaces;
using CRUD.Application.Service_Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CRUD.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _serviceMock;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _serviceMock = new Mock<ICategoryService>();
            _controller = new CategoryController(_serviceMock.Object);
        }
        //1. GET ALL 
        //TC-1: Return Ok with data
        [Fact]
        public async Task GetAll_Should_Return_OkResult_With_Data() { //test method name following methodname_condition_expectedResult pattern
                                                                      
            var data = new List<CategoryDto> { //Arrange and mocking data
        new CategoryDto { 
            Id = 1, Name = "Electronics"
        }
    };
            _serviceMock.Setup(x => x.GetAllCategoriesAsync()) //mock setup to return data when GetAllCategoriesAsync is called
                .ReturnsAsync(data);

            var result = await _controller.GetAll(); //Act - calling controller method

            var ok = Assert.IsType<OkObjectResult>(result); //Assert - check if result is OkObjectResult
            var value = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(ok.Value); //Assert - check if value is IEnumerable<CategoryDto>

            Assert.Single(value); //Assert - check if there is exactly one item in the result
        }
        //TC-2: Empty list test
        [Fact]
        public async Task GetAll_Should_Return_Empty_List()
        {
            _serviceMock.Setup(x => x.GetAllCategoriesAsync())
                .ReturnsAsync(new List<CategoryDto>());

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result); 
            var value = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(ok.Value);

            Assert.Empty(value);
        }
        //2. GET BY ID
        //TC-3: Found
        [Fact]
        public async Task GetById_Should_Return_Ok_When_Found() //test method name following methodname_condition_expectedResult pattern
        {
            var dto = new CategoryDto { Id = 1, Name = "Food" }; //Arrange - creating a CategoryDto object to return from the mock

            _serviceMock.Setup(x => x.GetCategoryByIdAsync(1)) //mock setup to return the dto when GetCategoryByIdAsync is called with ID 1
                .ReturnsAsync(dto);

            var result = await _controller.GetById(1); //Act - calling the GetById method of the controller with ID 1

            var ok = Assert.IsType<OkObjectResult>(result); //Assert - check if the result is of type OkObjectResult
            var value = Assert.IsType<CategoryDto>(ok.Value); //Assert - check if the value of the OkObjectResult is of type CategoryDto

            Assert.Equal(1, value.Id); //Assert - check if the ID of the returned CategoryDto is 1
        }
        //TC-4: Not Found
        [Fact]
        public async Task GetById_Should_Return_NotFound_When_Null()
        {
            _serviceMock.Setup(x => x.GetCategoryByIdAsync(1))
                .ReturnsAsync((CategoryDto?)null);  //mock setup to return null when GetCategoryByIdAsync is called with ID 1

            var result = await _controller.GetById(1);

            Assert.IsType<NotFoundResult>(result);
        }
        //TC-5 verify service call
        [Fact]
        public async Task GetById_Should_Call_Service_Once()
        {
            _serviceMock.Setup(x => x.GetCategoryByIdAsync(1))
                .ReturnsAsync(new CategoryDto { Id = 1 });

            await _controller.GetById(1);

            _serviceMock.Verify(x => x.GetCategoryByIdAsync(1), Times.Once); //verify that GetCategoryByIdAsync was called once with ID 1
        }
        //3. CREATE 
        //TC-6: Success
        [Fact]
        public async Task Create_Should_Return_CreatedAtAction()
        {
            var dto = new CreateCategoryDto { Name = "Electronics" };

            _serviceMock.Setup(x => x.CreateCategoryAsync(dto))
                .ReturnsAsync(10);

            var result = await _controller.Create(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal("GetById", created.ActionName);
        }
        //TC-7: Verify service call
        [Fact]
        public async Task Create_Should_Call_Service_Once()
        {
            var dto = new CreateCategoryDto { Name = "Food" };

            _serviceMock.Setup(x => x.CreateCategoryAsync(dto))
                .ReturnsAsync(1);

            await _controller.Create(dto);

            _serviceMock.Verify(x => x.CreateCategoryAsync(dto), Times.Once);
        }
        //TC-8: Null Dto
        [Fact]
        public async Task Create_Should_Return_BadRequest_When_Null()
        {
            var result = await _controller.Create(null!);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal("DTO cannot be null", badRequest.Value);
        }
        //4. UPDATE 
        //TC-9: Success
        [Fact]
        public async Task Update_Should_Return_Ok_When_Success()
        {
            var dto = new UpdateCategoryDto
            {
                Id = 1,
                Name = "Updated"
            };

            _serviceMock.Setup(x => x.UpdateCategoryAsync(dto))
                .ReturnsAsync(true);

            var result = await _controller.Update(1, dto);

            var ok = Assert.IsType<OkObjectResult>(result);
        }
        //TC-10: ID Mismatch
        [Fact]
        public async Task Update_Should_Return_BadRequest_When_Id_Mismatch()
        {
            var dto = new UpdateCategoryDto { Id = 2 };

            var result = await _controller.Update(1, dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }
        //TC-11: Not Found
        [Fact]
        public async Task Update_Should_Return_NotFound_When_False()
        {
            var dto = new UpdateCategoryDto { Id = 1 };

            _serviceMock.Setup(x => x.UpdateCategoryAsync(dto))
                .ReturnsAsync(false);

            var result = await _controller.Update(1, dto);

            Assert.IsType<NotFoundResult>(result);
        }
        //TC-12: verify service call
        [Fact]
        public async Task Update_Should_Call_Service_Once()
        {
            var dto = new UpdateCategoryDto { Id = 1 };

            _serviceMock.Setup(x => x.UpdateCategoryAsync(dto))
                .ReturnsAsync(true);

            await _controller.Update(1, dto);

            _serviceMock.Verify(x => x.UpdateCategoryAsync(dto), Times.Once);
        }
        //TC-13: Null Dto
        [Fact]
        public async Task Update_Should_Handle_Null_Dto()
        {
            var result = await _controller.Update(1, null!);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal("DTO cannot be null", badRequest.Value);
        }
        //5. DELETE
        //TC-14: Success
        [Fact]
        public async Task Delete_Should_Return_Ok_When_Success()
        {
            _serviceMock.Setup(x => x.DeleteCategoryAsync(1))
                .ReturnsAsync(true);

            var result = await _controller.Delete(1);

            Assert.IsType<OkObjectResult>(result);
        }
        //TC-15: Not Found
        [Fact]
        public async Task Delete_Should_Return_NotFound_When_False()
        {
            _serviceMock.Setup(x => x.DeleteCategoryAsync(1))
                .ReturnsAsync(false);

            var result = await _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }
        //TC-16: verify service call
        [Fact]
        public async Task Delete_Should_Call_Service_Once()
        {
            _serviceMock.Setup(x => x.DeleteCategoryAsync(1))
                .ReturnsAsync(true);

            await _controller.Delete(1);

            _serviceMock.Verify(x => x.DeleteCategoryAsync(1), Times.Once);
        }
        //TC-17: Negative ID edge case
        [Fact]
        public async Task Delete_Should_Handle_Negative_Id()
        {
            _serviceMock.Setup(x => x.DeleteCategoryAsync(-1))
                .ReturnsAsync(false);

            var result = await _controller.Delete(-1);

            Assert.IsType<NotFoundResult>(result);
        }
        //6. SEARCH
        //TC-18: Success
        [Fact]
        public async Task Search_Should_Return_Data()
        {
            var data = new List<CategoryDto>
    {
        new CategoryDto { Id = 1, Name = "Electronics" }
    };

            _serviceMock.Setup(x => x.SearchAsync("Elec"))
                .ReturnsAsync(data);

            var result = await _controller.Search("Elec");

            var ok = Assert.IsType<OkObjectResult>(result);
        }
        //TC-19 Empty result test
        [Fact]
        public async Task Search_Should_Return_Empty_List()
        {
            _serviceMock.Setup(x => x.SearchAsync("xyz"))
                .ReturnsAsync(new List<CategoryDto>());

            var result = await _controller.Search("xyz");

            var ok = Assert.IsType<OkObjectResult>(result);

            var value = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(ok.Value);

            Assert.Empty(value);
        }
        //TC-20: verify service call
        [Fact]
        public async Task Search_Should_Call_Service_Once()
        {
            _serviceMock.Setup(x => x.SearchAsync("test"))
                .ReturnsAsync(new List<CategoryDto>());

            await _controller.Search("test");

            _serviceMock.Verify(x => x.SearchAsync("test"), Times.Once);
        }
    }
}
