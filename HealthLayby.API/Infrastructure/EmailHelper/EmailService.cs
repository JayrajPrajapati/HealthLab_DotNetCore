using System.Net.Mail;
using System.Net;

namespace HealthLayby.API.Infrastructure.EmailHelper
{
    /// <summary>
    /// EmailService
    /// </summary>
    /// <seealso cref="HealthLayby.API.Infrastructure.EmailHelper.IEmailService" />
    public class EmailService : IEmailService
    {
        #region Private Variables

        /// <summary>
        /// From address
        /// </summary>
        private readonly string fromAddress;

        /// <summary>
        /// The user name
        /// </summary>
        private readonly string userName;

        /// <summary>
        /// The password
        /// </summary>
        private readonly string password;

        /// <summary>
        /// The SMTP client host
        /// </summary>
        private readonly string smtpClientHost;

        /// <summary>
        /// The SMTP client port
        /// </summary>
        private readonly int smtpClientPort;

        /// <summary>
        /// The SMTP client enable SSL
        /// </summary>
        private readonly bool smtpClientEnableSsl;

        #endregion       

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public EmailService(IConfiguration configuration)
        {
            fromAddress = Convert.ToString(configuration.GetValue("EmailSettings:ClientEmail", "johny.ctpl@gmail.com"));
            userName = Convert.ToString(configuration.GetValue("EmailSettings:userName", "Johny"));
            password = Convert.ToString(configuration.GetValue("EmailSettings:password", "hpgrfevlskwamugp"));
            smtpClientHost = Convert.ToString(configuration.GetValue("EmailSettings:smtpServer", "smtp.gmail.com"));
            smtpClientPort = Convert.ToInt16(configuration.GetValue("EmailSettings:smtpPort", 587));
            smtpClientEnableSsl = Convert.ToBoolean(configuration.GetValue("EmailSettings:IsSSLEnabled", true));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="toEmail">To email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="mailBody">The mail body.</param>
        /// <param name="isBodyHTML">if set to <c>true</c> [is body HTML].</param>
        public async Task SendEmail(string[] toEmail, string subject, string mailBody, bool isBodyHTML = true)
        {
            try
            {
                var fromAddress = new MailAddress(this.fromAddress);
                var message = new MailMessage
                {
                    From = fromAddress,
                    Subject = subject,
                    IsBodyHtml = isBodyHTML,
                    Body = mailBody
                };

                message.ReplyToList.Add(fromAddress);

                foreach (string mail in toEmail)
                {
                    message.To.Add(mail);
                }

                using (var smtp = new SmtpClient(smtpClientHost, smtpClientPort)
                {
                    Credentials = new NetworkCredential(this.fromAddress, password),
                    EnableSsl = smtpClientEnableSsl
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
