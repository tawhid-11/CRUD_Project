using CRUD.Application.Repositoy_Interfaces;
using CRUD.Application.UOW_Interface;
using CRUD.Infrastructure.Dapper;
using CRUD.Infrastructure.Repositories;
using CRUD.Infrastructure.unitofWork;
using Microsoft.Extensions.DependencyInjection;


namespace CRUD.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<AppDapperContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
