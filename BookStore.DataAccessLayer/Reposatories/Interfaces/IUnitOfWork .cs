using BookStore.DataAccessLayer.Reposatories.Interfaces;

namespace BookStore.DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        IOrderHeaderRepository _orderHeaderRepository { get; }
        IOrderDetailsReposatory orderDetailsReposatory { get; }
        IGenericRepository<T> GetRepository<T>() where T : class;
        int Complete();
        void Dispose();

    }
}
