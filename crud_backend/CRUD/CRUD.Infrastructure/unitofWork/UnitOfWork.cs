using CRUD.Application.Repositoy_Interfaces;
using CRUD.Application.UOW_Interface;
using CRUD.Infrastructure.Dapper;

namespace CRUD.Infrastructure.unitofWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDapperContext _context;


        public IProductRepository Productrepository { get; }
        public ICategoryRepository categoryRepository { get; }
        public UnitOfWork(AppDapperContext context, IProductRepository productrepository, ICategoryRepository categoryRepository)
        {
            this._context = context;
            this.Productrepository = productrepository;
            this.categoryRepository = categoryRepository;
        }
        public Task SaveAsync()
        {
            // Dapper usually auto-save kore
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

    }

}

