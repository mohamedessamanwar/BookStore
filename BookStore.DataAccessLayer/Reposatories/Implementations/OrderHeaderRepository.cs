using BookStore.DataAccessLayer.Contexts;
using BookStore.DataAccessLayer.Models;
using BookStore.DataAccessLayer.Reposatories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccessLayer.Reposatories.Implementations
{
    public class OrderHeaderRepository : GenericRepositoryV2<Models.OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }


        public IEnumerable<OrderHeader> GetOrderWithUser()
        {
            return context.orderHeaders.Include(o => o.ApplicationUser).ToList();
        }
        public OrderHeader? GetOrder(int id)
        {
            return context.orderHeaders.Include(o => o.ApplicationUser).FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<OrderHeader> GetUserOrderWithUser(string id)
        {
            return context.orderHeaders.Include(o => o.ApplicationUser).Where(o => o.ApplicationUserId == id).ToList();
        }

    }
}
