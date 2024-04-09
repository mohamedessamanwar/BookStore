using BookStore.DataAccessLayer.Models;

namespace BookStore.BusnessLogic.ViewsModels.Order
{
    public class OrderView
    {
        public List<OrderDetail> Detail { get; set; }
        public OrderHeader Header { get; set; }

    }
}
