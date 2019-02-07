namespace WebAPI_ViewModel.ConfigSettings
{
    public class SmtpConfig
    {
        public string MailServer { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public string DefaultSenderEmail { get; set; }
        public string DefaultSenderName { get; set; }
        public bool IsTesting { get; set; }
        public string TestEmail { get; set; }

  }
}
