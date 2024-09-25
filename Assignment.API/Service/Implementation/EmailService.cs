using Assignment.API.Service.Interface;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Assignment.API.Entity;
using Assignment.API.Model;

namespace Assignment.API.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Dictionary<string, string> _config;

        public EmailService(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _config = config.GetSection("EmailSettings").Get<Dictionary<string, string>>();
        }

        public async Task<bool> Send(Message message, string attachment = "")
        {
            try
            {
                string senderEmail = _config["SenderEmail"];
                string appPassword = _config["Password"];
                IList<string> toEmails = message.To;

                MailMessage mailMessage = new();
                foreach (string toEmail in toEmails)
                {
                    mailMessage.To.Add(toEmail);
                }

                mailMessage.From = new MailAddress(senderEmail);
                mailMessage.Subject = message.Subject;
                mailMessage.Body = message.Body;
                mailMessage.IsBodyHtml = true;

                if (!string.IsNullOrEmpty(attachment))
                {
                    Attachment attach = new(attachment);
                    mailMessage.Attachments.Add(attach);
                    mailMessage.Priority = MailPriority.High;
                }

                using (SmtpClient smtpClient = new SmtpClient(_config["Host"], int.Parse(_config["Port"])))
                {
                    smtpClient.EnableSsl = bool.Parse(_config["UseSsl"]);  // Use SSL based on config
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(senderEmail, appPassword);
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                    // Send email asynchronously
                    await smtpClient.SendMailAsync(mailMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending email: {ex.Message}", ex);
            }
        }
    }

}
