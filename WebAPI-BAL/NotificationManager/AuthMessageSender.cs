using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using WebAPI_ViewModel.ConfigSettings;

namespace WebAPI_BAL.NotificationManager
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SmtpConfig _smtpConfig;
        public AuthMessageSender(IOptions<SmtpConfig> smtpConfig, IHttpContextAccessor httpContextAccessor)
        {
            _smtpConfig = smtpConfig.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                MailAddress receiverEmail = new MailAddress(email);
                if (_smtpConfig.IsTesting)
                {
                    receiverEmail = new MailAddress(_smtpConfig.TestEmail);
                }

                // Configure the client:
                SmtpClient client =
                    new SmtpClient(_smtpConfig.MailServer, _smtpConfig.Port)
                    {
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        EnableSsl = _smtpConfig.Ssl,
                        Credentials = new System.Net.NetworkCredential(_smtpConfig.SmtpUser, _smtpConfig.SmtpPass)
                    };

                // Create the message:
                var mail = new MailMessage(
                    new MailAddress(_smtpConfig.DefaultSenderEmail, _smtpConfig.DefaultSenderName),
                    receiverEmail)
                {
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };

                return client.SendMailAsync(mail);
            }
            catch (Exception e)
            {
                _httpContextAccessor.HttpContext.Session.Set(AppConstants.SessionErrorKey, e.ToByteArray());
                return Task.FromResult<object>(null);
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            return Task.FromResult(0);
        }
    }

    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
