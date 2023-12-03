using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using Microsoft.AspNetCore.Identity.UI.Services;
using MailKit.Net.Smtp;

namespace JetStudyProject
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(emailToSend);
                smtp.Disconnect(true);
            }
            return Task.FromResult(true);
        }
    }
}
