using BookStore.DataAccessLayer.Contexts;
using BookStore.DataAccessLayer.Interfaces;
using BookStore.DataAccessLayer.Reposatories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookStore.DataAccessLayer.Reposatories.Implementations { 
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

        }
        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            // Implement logic to check for existing instance and create a new one if needed
            // (e.g., using a singleton or dependency injection)
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

