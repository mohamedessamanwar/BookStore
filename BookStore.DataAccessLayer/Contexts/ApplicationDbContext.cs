
using BookStore.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccessLayer.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
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

        public DbSet<ShopingCart> ShopingCarts { get; set; }
        public DbSet<ResetPasswordSecurty> ResetPasswordSecurty { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<OrderHeader> orderHeaders { get; set; }

        public DbSet<OrderDetail> orderDetails { get; set; }


    }
}
