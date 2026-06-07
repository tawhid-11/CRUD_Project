using CRUD.Application.Product_Dto;
using CRUD.Application.Repositoy_Interfaces;
using CRUD.Application.Services;
using CRUD.Application.UOW_Interface;
using CRUD.Core.Entities;
using Moq;

namespace CRUD.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            _uowMock
                .Setup(x => x.Productrepository)
                .Returns(_productRepoMock.Object);

            _service = new ProductService(_uowMock.Object);
        }
        [Fact]
        public async Task GetAllProductsAsync_Should_Return_All_Products()
        {
            // Arrange
            var products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1000 },
            new Product { Id = 2, Name = "Mouse", Price = 50 }
        };

            _productRepoMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _service.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Laptop", result.First().Name);
        }
        [Fact]
        public async Task GetProductByIdAsync_Should_Return_Product()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 1000
            };

            _productRepoMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            var result = await _service.GetProductByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Laptop", result.Name);
        }
        [Fact]
        public async Task GetProductByIdAsync_Should_Return_Null_When_Not_Found()
        {
            _productRepoMock
                .Setup(x => x.GetByIdAsync(999))
                .ReturnsAsync((Product?)null);

            var result = await _service.GetProductByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateProductAsync_Should_Return_Id_When_Valid()
        {
            var dto = new CreateProductDto
            {
                Name = "Laptop",
                Description = "Gaming laptop",
                Price = 1000,
                Stock = 5,
                CategoryId = 1,
                Size = "M"
            };

            _productRepoMock
                .Setup(x => x.CreateAsync(It.IsAny<Product>()))
                .ReturnsAsync(10);

            var result = await _service.CreateProductAsync(dto);

            Assert.Equal(10, result);
        }

        [Fact]
        public async Task CreateProductAsync_Should_Throw_Exception_When_Name_Is_Null()
        {
            var dto = new CreateProductDto
            {
                Name = null,
                Price = 100
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.CreateProductAsync(dto));
        }
        [Fact]
        public async Task CreateProductAsync_Should_Throw_When_Name_Empty()
        {
            var dto = new CreateProductDto
            {
                Name = "",
                Price = 100
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.CreateProductAsync(dto));
        }


        [Fact]
        public async Task CreateProductAsync_Should_Throw_When_Price_Zero()
        {
            var dto = new CreateProductDto
            {
                Name = "Laptop",
                Price = 0
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.CreateProductAsync(dto));
        }

        [Fact]
        public async Task CreateProductAsync_Should_Throw_When_Price_Negative()
        {
            var dto = new CreateProductDto
            {
                Name = "Laptop",
                Price = -10
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.CreateProductAsync(dto));
        }



        [Fact]
        public async Task CreateProductAsync_Should_Throw_When_CategoryId_Invalid()
        {
            var dto = new CreateProductDto
            {
                Name = "Laptop",
                Price = 100,
                CategoryId = 0
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.CreateProductAsync(dto));
        }

        [Fact]
        public async Task CreateProductAsync_Should_Throw_When_Description_Too_Long()
        {
            var dto = new CreateProductDto
            {
                Name = "Laptop",
                Price = 100,
                Description = new string('A', 201)
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.CreateProductAsync(dto));
        }

        [Fact]
        public async Task CreateProductAsync_Should_Throw_Exception_When_Price_Is_Zero()
        {
            var dto = new CreateProductDto
            {
                Name = "Laptop",
                Price = 0,
                Stock = 5,
                CategoryId = 1
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _service.CreateProductAsync(dto);
            });
        }

        [Fact]
        public async Task UpdateProductAsync_Should_Throw_Exception_When_ExpireDate_Is_Today()
        {
            var dto = new UpdateProductDto
            {
                Id = 1,
                Name = "Laptop",
                Price = 100,
                Stock = 10,
                CategoryId = 1,
                ExpireDate = DateTime.Today
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _service.UpdateProductAsync(dto);
            });
        }

        [Fact]
        public async Task DeleteProductAsync_Should_Return_True()
        {
            _productRepoMock
                .Setup(x => x.DeleteAsync(1))
                .ReturnsAsync(true);

            var result = await _service.DeleteProductAsync(1);

            Assert.True(result);
        }



        [Fact]
        public async Task FilterAsync_Should_Map_Products_To_ProductDto_Correctly()
        {
            // Arrange
            var filter = new ProductFilterDto();

            var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 1000,
                Stock = 5,
                CategoryId = 2
            }
        };

            _productRepoMock
             .Setup(x => x.FilterAsync(It.IsAny<ProductFilterDto>()))
             .ReturnsAsync(products);

            // Act
            var result = await _service.FilterAsync(filter);

            var item = result.First();

            // Assert
            Assert.Equal(1, item.Id);
            Assert.Equal("Laptop", item.Name);
            Assert.Equal(1000, item.Price);
            Assert.Equal(5, item.Stock);
            Assert.Equal(2, item.CategoryId);
        }
    }
}
