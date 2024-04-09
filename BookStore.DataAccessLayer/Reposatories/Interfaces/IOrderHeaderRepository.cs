using BookStore.DataAccessLayer.Models;

namespace BookStore.DataAccessLayer.Reposatories.Interfaces
{
    public interface IOrderHeaderRepository : IGenericRepositoryV2<OrderHeader>
    {
        IEnumerable<OrderHeader> GetOrderWithUser();
        IEnumerable<OrderHeader> GetUserOrderWithUser(string id);
        OrderHeader? GetOrder(int id);

    }
}
