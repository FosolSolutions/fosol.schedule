namespace Fosol.Schedule.API.Helpers.Mail
{
    /// <summary>
    /// MailOptions class, provides a way to configure the MailClient.
    /// </summary>
    public class MailOptions
    {
        #region Properties
        /// <summary>
        /// get/set - The email account address.
        /// </summary>
        public string AccountAddress { get; set; }

        /// <summary>
        /// get/set - The email account password.
        /// </summary>
        public string AccountPassword { get; set; }

        /// <summary>
        /// get/set - The email account SMTP DNS.
        /// </summary>
        public string SmtpDNS { get; set; }

        /// <summary>
        /// get/set - The email account SMTP port.
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// get/set - Whether to enable SSL.
        /// </summary>
        public bool EnableSsl { get; set; } = true;

        /// <summary>
        /// get/set - Whether to use default credentials when sending email.
        /// </summary>
        public bool UseDefaultCredentials { get; set; } = false;

        /// <summary>
        /// get/set - How long to wait in milliseconds before timing out a request to send email.
        /// </summary>
        public int Timeout { get; set; } = 15000;
        #endregion
    }
}
