namespace BookStore.DataAccessLayer.Models
{
    public class ResetPasswordSecurty
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }

    }
}
