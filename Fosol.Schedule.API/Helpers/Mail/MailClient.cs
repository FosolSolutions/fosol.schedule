using Fosol.Schedule.Models;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace Fosol.Schedule.API.Helpers.Mail
{
    public class MailClient
    {
        #region Variables
        private MailOptions _options;
        private SmtpClient _client;
        #endregion

        #region Constructors
        public MailClient(MailOptions options)
        {
            _options = options;

            _client = new SmtpClient(_options.SmtpDNS)
            {
                Port = _options.SmtpPort,
                UseDefaultCredentials = _options.UseDefaultCredentials,
                Credentials = new System.Net.NetworkCredential(_options.AccountAddress, _options.AccountPassword),
                EnableSsl = _options.EnableSsl,
                Timeout = _options.Timeout
            };
        }

        public MailClient(IOptions<MailOptions> options) : this(options.Value)
        {
        }
        #endregion

        #region Methods
        public void Send(Participant participant)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_options.AccountAddress),
                Subject = "Victoria Ecclesial Volunteer Schedule",
                IsBodyHtml = true,
                Body = $@"
                Hello {participant.FirstName},
                <p>
                    Please use the following link to access the Victoria Ecclesial Volunteer Schedule - <a href=""http://fosolschedule.azurewebsites.net/auth/signin/participant/{participant.Key}"">VOLUNTEER LINK</a><br/>
                    This link is specifically generated for you, please do not forward it to someone else.
                </p>
                Love in Christ, Jeremy"
            };
            message.To.Add(new MailAddress(participant.Email));

            Send(message);
        }

        public void Send(MailMessage message)
        {
            _client.Send(message);
        }
        #endregion
    }
}