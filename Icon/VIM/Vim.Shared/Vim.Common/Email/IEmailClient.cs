using System.Net.Mail;

namespace Vim.Common.Email
{
    public interface IEmailClient
    {
        void Send(string message, string subject);
        void Send(string message, string subject, bool isBodyHtml);
        void Send(string message, string subject, string[] recipients);
        void SetRecipients(string[] recipients);
    }
}
