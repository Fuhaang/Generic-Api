using SendGrid;
using SendGrid.Helpers.Mail;

using Service.Contract;
using System;
using System.Threading.Tasks;

namespace Service
{
    public class MessageServices : IEmailSender, ISmsSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(Environment.GetEnvironmentVariable("SENDGRID_EMAIL"), "GenericApi Team");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", message);
            return Task.FromResult(client.SendEmailAsync(msg));
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
