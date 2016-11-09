using Mammoth.Common.Email;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MammothWebApi.Email
{
    public class EmailMessageBuilder<T> : IEmailMessageBuilder<T> where T: class
    {
        private static string tableStyle = "style='border:1px solid; border-color: #808080;border-collapse:collapse;font-size:12;font-family:Segoe UI;padding:10px;'";

        public string BuildMessage(T data)
        {
            StringBuilder builder = new StringBuilder();
            AppendMessageWithTable(builder, data);
            return builder.ToString();
        }


        public string BuildMessage(T data, Exception e)
        {
            StringBuilder builder = new StringBuilder();
            AppendMessageWithException(builder, e);
            builder.Append("<br><br>");
            AppendMessageWithTable(builder, data);
            return builder.ToString();
        }

        private string AppendMessageWithTable(StringBuilder builder, T data)
        {
            // assumes that T has a property that has generic arguments like 'List<>'
            PropertyInfo[] properties = typeof(T).GetProperties().First().GetValue(data).GetType().GetGenericArguments()[0].GetProperties();

            builder.Append(String.Format("<table {0}", tableStyle));
            builder.Append(String.Format("<tr {0}>", tableStyle));
            foreach (var property in properties)
            {
                
                builder.Append(String.Format("<th {0}>{1}</th>", tableStyle, property.Name));
                    
            }
            builder.Append("</tr>");

            var enumerableData = typeof(T).GetProperties().First().GetValue(data);
            foreach (var element in enumerableData as IEnumerable)
            {
                builder.Append(String.Format("<tr {0}>", tableStyle));
                foreach (var property in properties)
                {
                    builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, property.GetValue(element)));
                }
                builder.Append("</tr>");
            }

            builder.Append("</table>");

            return builder.ToString();
        }

        private string AppendMessageWithException(StringBuilder builder, Exception e)
        {
            // Build small table with exception
            builder.Append(String.Format("<table {0}", tableStyle));

            builder.Append(String.Format("<tr {0}>", tableStyle))
                .Append(String.Format("<th {0}>Exception</th>", tableStyle))
                .Append(String.Format("<td {0}>{1}</td>", tableStyle, e.Message));
            builder.Append(String.Format("</tr>"));

            builder.Append(String.Format("<tr {0}>", tableStyle))
                .Append(String.Format("<th {0}>Inner Exception</th>", tableStyle))
                .Append(String.Format("<td {0}>{1}</td>", tableStyle, e.InnerException));
            builder.Append("</tr>");

            builder.Append("</table>");

            return builder.ToString();
        }
    }
}