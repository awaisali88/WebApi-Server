using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
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
                List<MailAddress> receiverEmails = new List<MailAddress>() { new MailAddress(email) };
                if (_smtpConfig.IsTesting && !string.IsNullOrEmpty(_smtpConfig.TestEmail))
                {
                    var testEmails = _smtpConfig.TestEmail.Split(',').ToList();
                    receiverEmails = new List<MailAddress>();
                    receiverEmails.AddRange(testEmails.Select(x => new MailAddress(x, email)));
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
                var mail = new MailMessage()
                {
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                    From = new MailAddress(_smtpConfig.DefaultSenderEmail, _smtpConfig.DefaultSenderName)
                };

                foreach (var receiverEmail in receiverEmails)
                    mail.To.Add(receiverEmail);

                return client.SendMailAsync(mail);
            }
            catch (Exception e)
            {
                _httpContextAccessor.HttpContext.Session.Set(AppConstants.SessionErrorKey, e.ToByteArray());
                return Task.FromResult<object>(null);
            }
        }

        public void SendEmail(string email, string subject, string htmlMessage)
        {
            try
            {
                List<MailAddress> receiverEmails = new List<MailAddress>() { new MailAddress(email) };
                if (_smtpConfig.IsTesting && !string.IsNullOrEmpty(_smtpConfig.TestEmail))
                {
                    var testEmails = _smtpConfig.TestEmail.Split(',').ToList();
                    receiverEmails = new List<MailAddress>();
                    receiverEmails.AddRange(testEmails.Select(x => new MailAddress(x, email)));
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
                var mail = new MailMessage()
                {
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                    From = new MailAddress(_smtpConfig.DefaultSenderEmail, _smtpConfig.DefaultSenderName)
                };

                foreach (var receiverEmail in receiverEmails)
                    mail.To.Add(receiverEmail);

                client.Send(mail);
            }
            catch (Exception e)
            {
                _httpContextAccessor.HttpContext.Session.Set(AppConstants.SessionErrorKey, e.ToByteArray());
            }
        }

        public bool SendOfflineMessageEmail(SmtpConfig smtpConfig, string subject, object htmlMessage, bool isAlternateView = false, params string[] toEmails)
        {
            try
            {
                foreach (var toEmail in toEmails)
                {
                    if (string.IsNullOrEmpty(toEmail)) return false;

                    List<MailAddress> receiverEmails = new List<MailAddress>() { new MailAddress(toEmail) };
                    if (smtpConfig.IsTesting && !string.IsNullOrEmpty(smtpConfig.TestEmail))
                    {
                        var testEmails = smtpConfig.TestEmail.Split(',').ToList();
                        receiverEmails = new List<MailAddress>();
                        receiverEmails.AddRange(testEmails.Select(x => new MailAddress(x, toEmail)));
                    }

                    // Configure the client:
                    SmtpClient client =
                        new SmtpClient(smtpConfig.MailServer, smtpConfig.Port)
                        {
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            EnableSsl = smtpConfig.Ssl,
                            Credentials = new System.Net.NetworkCredential(smtpConfig.SmtpUser, smtpConfig.SmtpPass)
                        };

                    var mail = new MailMessage
                    {
                        Subject = subject,
                        From = new MailAddress(smtpConfig.DefaultSenderEmail, smtpConfig.DefaultSenderName)
                    };

                    if (!isAlternateView && htmlMessage is string messageBody)
                    {
                        mail.Body = messageBody;
                        mail.IsBodyHtml = true;
                    }
                    else
                        mail.AlternateViews.Add((htmlMessage as AlternateView));


                    foreach (var receiverEmail in receiverEmails)
                        mail.To.Add(receiverEmail);

                    client.Send(mail);

                    mail.Dispose();
                }

                return true;
            }
            catch (Exception e)
            {
                _httpContextAccessor.HttpContext.Session.Set(AppConstants.SessionErrorKey, e.ToByteArray());
                return false;
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            return Task.FromResult(0);
        }
    }
}
