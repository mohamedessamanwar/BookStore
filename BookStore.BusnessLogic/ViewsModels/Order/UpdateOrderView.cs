namespace BookStore.BusnessLogic.ViewsModels.Order
{
    public class UpdateOrderView
    {
        public int Header_Id { get; set; }
        public string? Header_Carrier { get; set; }
        public string Header_PhoneNumber { get; set; }
        public string Header_StreetAddress { get; set; }
        public string Header_City { get; set; }
        public string Header_State { get; set; }
        public string Header_Name { get; set; }
        public string? Header_TrackingNumber { get; set; }

    }
}
