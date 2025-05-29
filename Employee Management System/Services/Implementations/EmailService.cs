using Employee_Management_System.Models.Entities;
using Employee_Management_System.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Employee_Management_System.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailSetting _emailSetting;

        public EmailService(IOptions<EmailSetting> options)
        {
              _emailSetting = options.Value;  
        }

        public async Task SenderEmailAsync(string toEmail, string subject, string body)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(_emailSetting.SenderEmail, _emailSetting.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(toEmail);

            using var smtp = new SmtpClient(_emailSetting.UserName, _emailSetting.Port)
            {
                Credentials = new NetworkCredential(_emailSetting.UserName, _emailSetting.Password),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
