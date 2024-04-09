using BookStore.DataAccessLayer.Models;

namespace BookStore.BusnessLogic.ViewsModels.OrderDetails
{
	public class OrderDetailsView
	{
		public int Id { get; set; }

		public int OrderId { get; set; }


		public OrderHeader OrderHeader { get; set; }

		public int ProductId { get; set; }

		public BookStore.DataAccessLayer.Models.Product Product { get; set; }
		public int Count { get; set; }
		public double Price { get; set; }
	}
}
