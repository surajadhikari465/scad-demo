using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace NutritionWebApi.Email
{
    public class EmailBuilder
    {
        private static string tableStyle = "style='border:1px solid black;border-collapse:collapse;font-family:Segoe UI;padding:10px;'";

        public static string BuildEmailBody(string user, string requestedUri, string method, string remoteIp, string message)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(String.Format("<table {0}", tableStyle));

            //Create header row
            builder.Append(String.Format("<tr {0}>", tableStyle))
                .Append(String.Format("<th {0}>User</th>", tableStyle))
                .Append(String.Format("<th {0}>Requested URI</th>", tableStyle))
                .Append(String.Format("<th {0}>Method</th>", tableStyle))
                .Append(String.Format("<th {0}>Remote IP</th>", tableStyle))
                .Append(String.Format("<th {0}>Error Message</th>", tableStyle))
                .Append("</tr>");

            //Create new row foreach event
           
                builder.Append(String.Format("<tr {0}>", tableStyle));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, user));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, requestedUri));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, method));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, remoteIp));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, message));

                builder.Append("</table>");

                return builder.ToString();
        }
    }
}