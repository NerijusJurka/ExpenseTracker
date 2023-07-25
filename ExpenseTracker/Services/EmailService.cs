
using System.Net.Mail;
using System.Net;


namespace ExpenseTracker.Services
{
    internal class EmailService : IEmailService
    {
        public async Task SendEmail(string to, string subject, string body)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("your-email@gmail.com", "your-password"),
                EnableSsl = true
            };

            await client.SendMailAsync("your-email@gmail.com", to, subject, body);
        }
    }
}
