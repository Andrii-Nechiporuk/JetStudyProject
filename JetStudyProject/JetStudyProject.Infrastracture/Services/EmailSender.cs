using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using JetStudyProject.Infrastracture.DTOs.EmailDTOs;
using JetStudyProject.Infrastracture.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace JetStudyProject.Infrastracture.Services
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
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = _config.GetSection("EmailHost").Value, //or another email sender provider
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value)
            };

            return client.SendMailAsync(_config.GetSection("EmailUsername").Value, email, subject, htmlMessage);
        }
    }
}