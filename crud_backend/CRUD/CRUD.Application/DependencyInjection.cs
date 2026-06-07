using CRUD.Application.Service_Interfaces;
using CRUD.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CRUD.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}
