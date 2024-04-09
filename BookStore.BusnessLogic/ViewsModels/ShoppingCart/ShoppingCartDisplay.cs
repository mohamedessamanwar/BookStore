namespace BookStore.BusnessLogic.ViewsModels.ShoppingCart
{
    public class ShoppingCartDisplay
    {
        public List<CartView> CartViews { get; set; } = new List<CartView>();

        public double TotalPrice { get; set; }
    }
}
