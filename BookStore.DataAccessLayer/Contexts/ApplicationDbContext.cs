
using BookStore.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccessLayer.Contexts
{
    public class ApplicationDbContext : DbContext
    {
       //public ApplicationDbContext(DbContextOptions options) : base(options)
       // { 
       // }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configure your database connection string here
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=BookStore;Integrated Security=True;TrustServerCertificate=True");
            }
        }

        // Tables
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        
    }
}
