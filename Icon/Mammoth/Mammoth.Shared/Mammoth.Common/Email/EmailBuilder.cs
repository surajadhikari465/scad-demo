using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Common.Email
{
    public class EmailBuilder : IEmailBuilder
    {
        private static string tableStyle = "style='border:1px solid; border-color: #808080;border-collapse:collapse;font-size:12;font-family:Segoe UI;padding:10px;'";

        public string BuildEmail<T>(T data) where T : IEnumerable
        {
            StringBuilder builder = new StringBuilder();
            string emailBody = BuildEmailTable(data, builder);
            return emailBody;
        }

        public string BuildEmail<T>(T data, string message) where T : IEnumerable
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(String.Format("<table {0}", tableStyle));
            builder.Append(String.Format("<tr {0}>", tableStyle));
            builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, message));
            builder.Append("</tr>");
            builder.Append("</table>");
            builder.Append("<br>");
            string emailBody = BuildEmailTable<T>(data, builder);
            return emailBody;
        }

        private string BuildEmailTable<T>(T data, StringBuilder builder) where T : IEnumerable
        {
            var properties = data.GetType().GetGenericArguments().First().GetProperties();

            builder.Append(String.Format("<table {0}", tableStyle));
            builder.Append(String.Format("<tr {0}>", tableStyle));
            foreach (var property in properties)
            {
                builder.Append(String.Format("<th {0}>{1}</th>", tableStyle, property.Name));
            }
            builder.Append("</tr>");

            foreach (var item in data)
            {
                builder.Append(String.Format("<tr {0}>", tableStyle));
                foreach (var property in properties)
                {
                    builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, property.GetValue(item)));
                }
                builder.Append("</tr>");
            }

            builder.Append("</table>");

            return builder.ToString();
        }
    }
}
