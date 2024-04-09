using BookStore.DataAccessLayer.Contexts;
using BookStore.DataAccessLayer.Reposatories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccessLayer.Reposatories.Implementations
{
	public class GenericRepositoryV2<T> : IGenericRepositoryV2<T> where T : class
	{
		private ApplicationDbContext context;

		public GenericRepositoryV2(ApplicationDbContext context)
		{
			this.context = context;
		}
		public IEnumerable<T> GetAll()
		{
			return context.Set<T>().AsNoTracking();
		}
		public T GetByIdAsync(int id)
		{
			return context.Set<T>().Find(id);
		}
		public void Add(T entity)
		{
			context.Set<T>().Add(entity);
		}
		public void Update(T entity)
		{

		}
		public void Delete(T entity)
		{
			context.Set<T>().Remove(entity);

		}
	}
}
