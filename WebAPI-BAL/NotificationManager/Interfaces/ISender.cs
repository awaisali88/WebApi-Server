using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WebAPI_ViewModel.ConfigSettings;

namespace WebAPI_BAL.NotificationManager
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }

    public interface IEmailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        void SendEmail(string email, string subject, string htmlMessage);
        bool SendOfflineMessageEmail(SmtpConfig smtpConfig, string subject, object htmlMessage, bool isAlternateView = false, params string[] toEmails);
    }
}
