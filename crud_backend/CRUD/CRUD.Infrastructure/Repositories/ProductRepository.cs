using CRUD.Application.Product_Dto;
using CRUD.Application.Repositoy_Interfaces;
using CRUD.Core.Entities;
using CRUD.Infrastructure.Dapper;
using Dapper;
using System.Data.Common;


namespace CRUD.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDapperContext _context;
        public ProductRepository(AppDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            const string query = "select * from Products";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product>(query);
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            const string query = "select * from Products where Id= @Id";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Product>(query, new { Id = id });
        }
        public async Task<int> CreateAsync(Product product)
        {
            const string query = "insert into products(Name, Description, Size, Price, Stock, MFG, ExpireDate, CategoryId) values(@Name, @Description, @Size, @Price, @Stock, @MFG, @ExpireDate, @CategoryId); select cast (scope_identity() as int)";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, product);
        }
        public async Task<bool> UpdateAsync(Product product)
        {
            const string query = "update products set Name=@Name, Description=@Description, Size=@Size, Price=@Price, Stock=@Stock, MFG=@MFG, ExpireDate=@ExpireDate, CategoryId=@CategoryId where Id=@Id";
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(query, product);
            return rowsAffected > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            const string query = "delete from products where Id=@Id";
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }
        public async Task<IEnumerable<Product>> FilterAsync(ProductFilterDto filter)
        {
            var sql = @"SELECT * FROM Products WHERE 1=1";
            var param = new DynamicParameters();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                sql += " AND Name LIKE @Name";
                param.Add("Name", "%" + filter.Name + "%");
            }

            if (filter.ExpireDate.HasValue)
            {
                sql += " AND CAST(ExpireDate AS DATE) = @ExpireDate";
                param.Add("ExpireDate", filter.ExpireDate.Value.Date);
            }

            if (filter.CategoryId.HasValue)
            {
                sql += " AND CategoryId = @CategoryId";
                param.Add("CategoryId", filter.CategoryId);
            }

            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<Product>(sql, param);
        }
    }
}
