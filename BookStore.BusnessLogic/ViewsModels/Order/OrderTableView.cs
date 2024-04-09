namespace BookStore.BusnessLogic.ViewsModels.Order
{
    public class OrderTableView
    {
        public int Id { get; set; }
        public double OrderTotal { get; set; }
        public string OrderStatus { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }

    }
}
