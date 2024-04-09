using BookStore.DataAccessLayer.Models;

namespace BookStore.DataAccessLayer.Reposatories.Interfaces
{
    public interface IOrderDetailsReposatory : IGenericRepositoryV2<OrderDetail>
    {
        IEnumerable<OrderDetail> GetOrderDetails(int id);
    }
}
