using BookStore.DataAccessLayer.Contexts;
using BookStore.DataAccessLayer.Models;
using BookStore.DataAccessLayer.Reposatories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccessLayer.Reposatories.Implementations
{

    public class OrderDetailsReposatory : GenericRepositoryV2<Models.OrderDetail>, IOrderDetailsReposatory
    {
        private readonly ApplicationDbContext context;
        public OrderDetailsReposatory(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<OrderDetail> GetOrderDetails(int id)
        {
            return context.orderDetails.Include(o => o.Product).Where(o => o.OrderId == id).ToList();
        }
    }
}
