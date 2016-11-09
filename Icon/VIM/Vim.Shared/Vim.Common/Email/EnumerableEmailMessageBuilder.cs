using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Vim.Common.Email
{
    public class EnumerableEmailMessageBuilder<T> : IEmailMessageBuilder<T> where T : IEnumerable
    {
        private static string tableStyle = "style='border:1px solid; border-color: #808080;border-collapse:collapse;font-size:12;font-family:Segoe UI;padding:10px;'";

        public string BuildMessage(T data)
        {
            StringBuilder builder = new StringBuilder();
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

        public string BuildMessage(T data, Exception e)
        {
            throw new NotImplementedException();
        }
    }
}

