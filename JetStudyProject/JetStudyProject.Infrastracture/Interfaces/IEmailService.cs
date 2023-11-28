using JetStudyProject.Infrastracture.DTOs.EmailDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string htmlMessage);
    }
}
