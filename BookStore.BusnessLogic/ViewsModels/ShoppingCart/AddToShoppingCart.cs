namespace BookStore.BusnessLogic.ViewsModels.ShoppingCart
{
    public class AddToShoppingCart
    {
        public int Count { get; set; }
        public int ProductId { get; set; }

        public string? UserId { get; set; }
    }
}
