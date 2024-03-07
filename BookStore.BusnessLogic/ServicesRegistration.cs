using BookStore.BusnessLogic.Services.Implementations;
using BookStore.BusnessLogic.Services.Interfaces;
using BookStore.BusnessLogic.ViewsModels.Atts;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.BusnessLogic
{
    public static class ServicesRegistration
    {
        public static IServiceCollection AddServicesRegistration(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<NameUnique>();
            return services;
        }
    }
}
