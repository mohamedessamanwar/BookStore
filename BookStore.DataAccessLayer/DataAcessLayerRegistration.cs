using BookStore.DataAccessLayer.Contexts;
using BookStore.DataAccessLayer.Interfaces;
using BookStore.DataAccessLayer.Reposatories.Implementations;
using BookStore.DataAccessLayer.Reposatories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace BookStore.DataAccessLayer
{
	public static class DataAcessLayerRegistration
	{
		public static IServiceCollection AddDataAcessLayerRegistration(this IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>();
			services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			return services;
		}
	}
}
