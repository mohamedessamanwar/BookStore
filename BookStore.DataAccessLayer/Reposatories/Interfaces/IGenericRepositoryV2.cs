namespace BookStore.DataAccessLayer.Reposatories.Interfaces
{
	public interface IGenericRepositoryV2<T> where T : class
	{
		IEnumerable<T> GetAll();
		T GetByIdAsync(int id);
		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);

	}
}
