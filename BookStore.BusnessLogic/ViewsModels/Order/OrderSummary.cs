using BookStore.BusnessLogic.ViewsModels.ShoppingCart;
using System.ComponentModel.DataAnnotations;

namespace BookStore.BusnessLogic.ViewsModels.Order
{
	public class OrderSummary
	{
		public List<CartView>? CartViews { get; set; } = new List<CartView>();
		public double? OrderTotal { get; set; }
		[Required]
		public string PhoneNumber { get; set; }
		[Required]
		public string StreetAddress { get; set; }
		[Required]
		public string City { get; set; }
		[Required]
		public string State { get; set; }

		[Required]
		public string Name { get; set; }
	}
}
