using CRUD.Application.Product_Dto;
using CRUD.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Application.Service_Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        public Task<ProductDto?> GetProductByIdAsync(int id);
        public Task<int> CreateProductAsync(CreateProductDto productDto);
        public Task<bool> UpdateProductAsync(UpdateProductDto productDto);
        public Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ProductDto>> FilterAsync(ProductFilterDto filter);
    }
}
