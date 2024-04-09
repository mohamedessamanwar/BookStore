namespace BookStore.BusnessLogic.Services.Email
{
    public interface IMailingService
    {
        Task<bool> SendEmailAsync(string email, string subject, string body);
    }
}
