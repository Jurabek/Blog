﻿using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Blog.Core.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var apiKey = Settings.SendGridKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("jurabek.azizkhujaev@gmail.com", "Blog Site");
            var subject = message.Subject;
            var to = new EmailAddress(message.Destination, "Blog Site");
            var plainTextContent = message.Body;
            var htmlContent = "<div>" + message.Body + "</div>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}