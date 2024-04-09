using BookStore.BusnessLogic.ViewsModels.ShoppingCart;

namespace BookStore.BusnessLogic.Services.Interfaces
{
	public interface IShoppingCartService
	{
		Task<string> AddShoppingCart(AddToShoppingCart addToShoppingCart);
		ShoppingCartDisplay GetShoppingCart(string id);
		Task Plus(int cartId);
		Task Minus(int cartId);

		Task ClearCart(string userid);
	}
}
