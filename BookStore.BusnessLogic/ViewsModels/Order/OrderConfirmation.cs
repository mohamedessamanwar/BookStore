using BookStore.DataAccessLayer.Models;

namespace BookStore.BusnessLogic.ViewsModels.Order
{
    public class OrderConfirmation
    {
        public List<ShopingCart> shopingCarts { get; set; }
        public int orderId { get; set; }
    }
}
