using Icon.Common.Email;
using System;
using System.Text;

namespace Icon.ApiController.Common
{
    public static class EmailHelper
    {
        private static string tableStyle = "style='border:1px solid black;border-collapse:collapse;font-family:Segoe UI;padding:10px;'";

        public static EmailClientSettings BuildEmailClientSettings()
        {
            return EmailClientSettings.CreateFromConfig();
        }

        public static string BuildMessageBodyForUnhandledException(string errorMessage, string exceptionText)
        {
            var messageBody = new StringBuilder();

            BuildMessageHeader(errorMessage, messageBody);

            messageBody.AppendLine(exceptionText);

            string footerMessage = String.Empty;
            BuildMessageFooter(footerMessage, messageBody);

            return messageBody.ToString();
        }

        public static string BuildMessageBodyForMiniBulkError(string errorMessage, int messageQueueId, string exceptionText)
        {
            var messageBody = new StringBuilder();

            BuildMessageHeader(errorMessage, messageBody);

            messageBody.AppendLine(String.Format("<table {0}>", tableStyle));
            messageBody.AppendLine(String.Format("<tr {0}><th {0}>Message Queue ID</th></tr>", tableStyle));

            messageBody.AppendLine(String.Format("<tr {0}><td {0}>{1}</td></tr>", tableStyle, messageQueueId));

            messageBody.AppendLine("</table>");

            string footerMessage = exceptionText;
            BuildMessageFooter(footerMessage, messageBody);

            return messageBody.ToString();
        }

        public static string BuildMessageBodyForSerializationFailure(string errorMessage, string exceptionText)
        {
            var messageBody = new StringBuilder();

            BuildMessageHeader(errorMessage, messageBody);

            messageBody.AppendLine(exceptionText);
            
            string footerMessage = "Please check the AppLog table for additional error information.";
            BuildMessageFooter(footerMessage, messageBody);

            return messageBody.ToString();
        }

        private static void BuildMessageHeader(string headerMessage, StringBuilder messageBody)
        {
            messageBody.AppendLine("<div style='font-family:Segoe UI;'>");
            messageBody.AppendLine(headerMessage);
            messageBody.AppendLine("</div>");

            messageBody.AppendLine("<div style='margin-top:50px;font-family:Segoe UI;'>");
        }

        private static void BuildMessageFooter(string footerMessage, StringBuilder messageBody)
        {
            messageBody.AppendLine("</div>");

            messageBody.AppendLine("<div style='font-family:Segoe UI;margin-top:50px;'>");
            messageBody.AppendLine(footerMessage);
            messageBody.AppendLine("</div>");
        }
    }
}
