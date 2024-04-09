using BookStore.DataAccessLayer.Contexts;
using BookStore.DataAccessLayer.Interfaces;
using BookStore.DataAccessLayer.Reposatories.Interfaces;


namespace BookStore.DataAccessLayer.Reposatories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IOrderHeaderRepository _orderHeaderRepository { get; }

        public IOrderDetailsReposatory orderDetailsReposatory { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _orderHeaderRepository = new OrderHeaderRepository(_context);
            orderDetailsReposatory = new OrderDetailsReposatory(_context);

        }
        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

