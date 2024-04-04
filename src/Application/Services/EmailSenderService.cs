using Core.Services;
using System.Net;
using System.Net.Mail;

namespace Application.Services
{
    public class EmailSenderService : IEmailSenderServices
    {
        public Task SendEmail(string toEmail, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("ayselabdullayeva657@gmail.com", "omsf wgyx axiu txlz"),
                EnableSsl = true
            };

            var email = client.SendMailAsync(new MailMessage
            {
                From = new MailAddress("ayselabdullayeva657@gmail.com", "Aysel"),
                To = { new MailAddress(toEmail) },
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            });
            return email;
        }
    }
}

