using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FableFortuneCardList.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.GmailEmailAddress, Options.GmailEmailPassword, subject, message, email);
        }

        public Task Execute(string fromEmail, string pwd, string subject, string message, string toEmail)
        {
            MailMessage msg = new MailMessage(fromEmail, toEmail, subject, message);
            msg.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(fromEmail, pwd);
            client.SendCompleted += (s, e) =>
            {
                client.Dispose();
                msg.Dispose();
            };
            return client.SendMailAsync(msg);
        }
    }
}
