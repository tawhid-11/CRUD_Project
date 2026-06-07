using CRUD.Application.Product_Dto;
using CRUD.Application.Service_Interfaces;
using CRUD.Application.UOW_Interface;
using CRUD.Core.Entities;

namespace CRUD.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        public ProductService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _uow.Productrepository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Size = p.Size,
                Price = p.Price,
                Stock = p.Stock,
                MFG = p.MFG,
                ExpireDate = p.ExpireDate,
                CategoryId = p.CategoryId
            });
        }
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var p = await _uow.Productrepository.GetByIdAsync(id);
            if (p == null)
                return null;
            return new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Size = p.Size,
                Price = p.Price,
                Stock = p.Stock,
                MFG = p.MFG,
                ExpireDate = p.ExpireDate,
                CategoryId = p.CategoryId
            };
        }
        public async Task<int> CreateProductAsync(CreateProductDto cpo)
        {
            if (string.IsNullOrWhiteSpace(cpo.Name))
                throw new ArgumentException("Product name cannot be empty");
            if (cpo.Price <= 0)
                throw new ArgumentException("Price must be greater than zero");

            if (!string.IsNullOrEmpty(cpo.Description) && cpo.Description.Length > 200)
            {
                throw new ArgumentException("Description cannot be longer than 200 characters");
            }
            if (cpo.Stock < 0)
                throw new ArgumentException("Stock cannot be negative");
            if (cpo.CategoryId <= 0)
                throw new ArgumentException("Valid CategoryId is required");

            var p = new Core.Entities.Product
            {
                Name = cpo.Name,
                Description = cpo.Description,
                Size = cpo.Size,
                Price = cpo.Price,
                Stock = cpo.Stock,
                MFG = cpo.MFG,
                ExpireDate = cpo.ExpireDate,
                CategoryId = cpo.CategoryId
            };
            return await _uow.Productrepository.CreateAsync(p);
        }

        public async Task<bool> UpdateProductAsync(UpdateProductDto upo)
        {
            if (string.IsNullOrWhiteSpace(upo.Name))
                throw new ArgumentException("Product name cannot be empty");
            if (upo.Price <= 0)
                throw new ArgumentException("Price must be greater than zero");
            if (upo.Stock < 0)
                throw new ArgumentException("Stock cannot be negative");
            var today = DateTime.Today;

            if (upo.ExpireDate.Date <= today)
                throw new ArgumentException("Expire date must be a future date");
            if (upo.CategoryId <= 0)
                throw new ArgumentException("Valid CategoryId is required");

            var p = new Product
            {
                Id = upo.Id,
                Name = upo.Name,
                Description = upo.Description,
                Size = upo.Size,
                Price = upo.Price,
                Stock = upo.Stock,
                MFG = upo.MFG,
                ExpireDate = upo.ExpireDate,
                CategoryId = upo.CategoryId
            };
            return await _uow.Productrepository.UpdateAsync(p);
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _uow.Productrepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<ProductDto>> FilterAsync(ProductFilterDto filter)
        {
            var products = await _uow.Productrepository.FilterAsync(filter);

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Size = p.Size,
                Price = p.Price,
                Stock = p.Stock,
                MFG = p.MFG,
                ExpireDate = p.ExpireDate,
                CategoryId = p.CategoryId
            });
        }

    }
}
