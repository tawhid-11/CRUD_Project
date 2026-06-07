using CRUD.API.Controllers;
using CRUD.Application.Product_Dto;
using CRUD.Application.Service_Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CRUD.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _serviceMock;

        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _serviceMock = new Mock<IProductService>();

            _controller =
                new ProductController(_serviceMock.Object);
        }
        //get all test
        [Fact]
        public async Task GetAll_Should_Return_OkResult()
        {
            var products = new List<ProductDto>
    {
        new ProductDto { Id = 1, Name = "Laptop" }
    };

            _serviceMock
                .Setup(x => x.GetAllProductsAsync())
                .ReturnsAsync(products);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);

            var data = Assert.IsAssignableFrom<IEnumerable<ProductDto>>
                (okResult.Value);

            Assert.Single(data);
        }
        //GetById test
        [Fact]
        public async Task GetById_Should_Return_Ok_When_Product_Exists()
        {
            // Arrange

            var product = new ProductDto
            {
                Id = 1,
                Name = "Laptop"
            };

            _serviceMock
                .Setup(x => x.GetProductByIdAsync(1))
                .ReturnsAsync(product);

            // Act

            var result =
                await _controller.GetById(1);

            // Assert

            Assert.IsType<OkObjectResult>(result);
        }
        //Not Found test
        [Fact]
        public async Task GetById_Should_Return_NotFound()
        {
            _serviceMock
                .Setup(x => x.GetProductByIdAsync(1))
                .ReturnsAsync((ProductDto?)null);

            var result =
                await _controller.GetById(1);

            Assert.IsType<NotFoundResult>(result);
        }
        //create test
        [Fact]
        public async Task Create_Should_Return_CreatedAtAction()
        {
            var dto = new CreateProductDto
            {
                Name = "Laptop",
                Price = 1000,
                CategoryId = 1
            };

            _serviceMock
                .Setup(x => x.CreateProductAsync(dto))
                .ReturnsAsync(10);

            var result =
                await _controller.Create(dto);

            Assert.IsType<CreatedAtActionResult>(result);
        }
        //verify service called test
        [Fact]
        public async Task Create_Should_Call_Service_Once()
        {
            var dto = new CreateProductDto
            {
                Name = "Laptop",
                Price = 1000,
                CategoryId = 1
            };

            _serviceMock
                .Setup(x => x.CreateProductAsync(dto))
                .ReturnsAsync(1);

            await _controller.Create(dto);

            _serviceMock.Verify(
                x => x.CreateProductAsync(dto),
                Times.Once);
        }
        //Update Id missmatch test
        [Fact]
        public async Task Update_Should_Return_BadRequest_When_Id_Mismatch()
        {
            var dto = new UpdateProductDto
            {
                Id = 2
            };

            var result = await _controller.Update(1, dto);

            Assert.IsType<BadRequestResult>(result);
        }
        //update success test
        [Fact]
        public async Task Update_Should_Return_Ok_When_Product_Exists()
        {
            var dto = new UpdateProductDto
            {
                Id = 1,
                Name = "Updated Laptop",
                Price = 1200,
                CategoryId = 1
            };

            _serviceMock
                .Setup(x => x.UpdateProductAsync(dto))
                .ReturnsAsync(true);

            var result = await _controller.Update(1, dto);

            Assert.IsType<OkObjectResult>(result);
        }
        //update not found test
        [Fact]
        public async Task Update_Should_Return_NotFound()
        {
            var dto = new UpdateProductDto
            {
                Id = 1,
                Name = "Updated",
                Price = 100,
                Stock = 10,
                CategoryId = 1,
                ExpireDate = DateTime.Today.AddDays(5)
            };

            _serviceMock
                .Setup(x => x.UpdateProductAsync(dto))
                .ReturnsAsync(false);

            var result = await _controller.Update(1, dto);

            Assert.IsType<NotFoundResult>(result);
        }
        //verify update service called test
        [Fact]
        public async Task Update_Should_Call_Service_Once()
        {
            var dto = new UpdateProductDto
            {
                Id = 1,
                Name = "Updated",
                Price = 100,
                Stock = 10,
                CategoryId = 1,
                ExpireDate = DateTime.Today.AddDays(2)
            };

            _serviceMock
                .Setup(x => x.UpdateProductAsync(dto))
                .ReturnsAsync(true);

            await _controller.Update(1, dto);

            _serviceMock.Verify(
                x => x.UpdateProductAsync(dto),
                Times.Once);
        }
        //delete success test
        [Fact]
        public async Task Delete_Should_Return_Ok()
        {
            _serviceMock
                .Setup(x => x.DeleteProductAsync(1))
                .ReturnsAsync(true);

            var result = await _controller.Delete(1);

            Assert.IsType<OkObjectResult>(result);
        }
        //delete not found  test
        [Fact]
        public async Task Delete_Should_Return_NotFound()
        {
            _serviceMock
                .Setup(x => x.DeleteProductAsync(1))
                .ReturnsAsync(false);

            var result = await _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }
        //verify delete service called test
        [Fact]
        public async Task Delete_Should_Call_Service_Once()
        {
            _serviceMock
                .Setup(x => x.DeleteProductAsync(1))
                .ReturnsAsync(true);

            await _controller.Delete(1);

            _serviceMock.Verify(
                x => x.DeleteProductAsync(1),
                Times.Once);
        }
        //filter success
        [Fact]
        public async Task Filter_Should_Return_Ok()
        {
            var filter = new ProductFilterDto();

            var products = new List<ProductDto>
    {
        new ProductDto
        {
            Id = 1,
            Name = "Laptop"
        }
    };

            _serviceMock
                .Setup(x => x.FilterAsync(filter))
                .ReturnsAsync(products);

            var result = await _controller.Filter(filter);

            var okResult =
                Assert.IsType<OkObjectResult>(result);

            var data =
                Assert.IsAssignableFrom<IEnumerable<ProductDto>>
                (okResult.Value);

            Assert.Single(data);
        }
        //verify filter service called test
        [Fact]
        public async Task Filter_Should_Call_Service_Once()
        {
            var filter = new ProductFilterDto();

            _serviceMock
                .Setup(x => x.FilterAsync(filter))
                .ReturnsAsync(new List<ProductDto>());

            await _controller.Filter(filter);

            _serviceMock.Verify(
                x => x.FilterAsync(filter),
                Times.Once);
        }
    }
}
