namespace Fosol.Schedule.API.Helpers.Mail
{
    public class MailOptions
    {
        #region Properties
        public string AccountAddress { get; set; }
        public string AccountPassword { get; set; }
        public string SmtpDNS { get; set; }
        public int SmtpPort { get; set; }
        public bool EnableSsl { get; set; } = true;
        public bool UseDefaultCredentials { get; set; } = false;
        public int Timeout { get; set; } = 15000;
        #endregion
    }
}
