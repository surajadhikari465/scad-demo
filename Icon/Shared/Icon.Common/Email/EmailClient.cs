using System.Net;
using System.Net.Mail;

namespace Icon.Common.Email
{
    public class EmailClient : IEmailClient
    {
        public EmailClientSettings Settings { get; set; }

        public EmailClient(EmailClientSettings settings)
        {
            Settings = settings;
        }

        public void Send(string message, string subject)
        {
            SendImplementation(message, subject, Settings.Recipients, true);
        }

        public void Send(string message, string subject, bool isBodyHtml)
        {
            SendImplementation(message, subject, Settings.Recipients, isBodyHtml);
        }

        public void Send(string message, string subject, string[] recipients)
        {
            SendImplementation(message, subject, recipients, true);
        }

        private void SendImplementation(string message, string subject, string[] recipients, bool isBodyHtml)
        {
            if (Settings.SendEmails)
            {
                using (SmtpClient client = new SmtpClient(Settings.Host, Settings.Port))
                {
                    client.Credentials = new NetworkCredential(Settings.Username, Settings.Password);

                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(Settings.Sender),
                        Body = message,
                        Subject = subject,
                        IsBodyHtml = isBodyHtml
                    };

                    foreach (var recipient in recipients)
                    {
                        mailMessage.To.Add(new MailAddress(recipient));
                    }

                    client.Send(mailMessage);
                }
            }
        }

        public void SetRecipients(string[] recipients)
        {
            Settings.Recipients = recipients;
        }

        public static EmailClient CreateFromConfig()
        {
            return new EmailClient(EmailClientSettings.CreateFromConfig());
        }

        public static EmailClient CreateFromConfigForRegion(string regionAbbreviation)
        {
            return new EmailClient(EmailClientSettings.CreateFromConfigForRegion(regionAbbreviation));
        }
    }
}
