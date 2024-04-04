namespace Core.Services
{
    public interface IEmailSenderServices
    {
        Task SendEmail(string toEmail, string subject, string message);
    }
}
