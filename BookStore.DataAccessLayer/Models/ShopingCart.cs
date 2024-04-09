namespace BookStore.DataAccessLayer.Models
{
    public class ShopingCart
    {
        public int Id { get; set; }

        public int Count { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }


    }
}
