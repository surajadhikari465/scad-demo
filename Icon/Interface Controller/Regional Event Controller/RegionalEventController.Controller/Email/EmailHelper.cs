using Icon.Common.Email;
using RegionalEventController.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegionalEventController.Controller.Email
{
    public static class EmailHelper
    {
        private static string tableStyle = "style='border:1px solid black;border-collapse:collapse;font-family:Segoe UI;font-size:15px;padding:10px;'";

        public static EmailClientSettings BuildEmailClientSettings()
        {
            return EmailClientSettings.CreateFromConfig();
        }

        public static string BuildEventQueueTable(IEnumerable<IrmaNewItem> events)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(String.Format("<table {0}", tableStyle));

            //Create header row
            builder.Append(String.Format("<tr {0}>", tableStyle))
                .Append(String.Format("<th {0}>RegionCode</th>", tableStyle))
                .Append(String.Format("<th {0}>QueueId</th>", tableStyle))
                .Append(String.Format("<th {0}>IrmaItemKey</th>", tableStyle))
                .Append(String.Format("<th {0}>Identifier</th>", tableStyle))
                .Append(String.Format("<th {0}>IconItemId</th>", tableStyle))
                .Append(String.Format("<th {0}>FailureReason</th>", tableStyle))
                .Append(String.Format("<th {0}>Processed</th>", tableStyle))
                .Append("</tr>");

            //Create new row foreach event
            foreach (var e in events)
            {
                builder.Append(String.Format("<tr {0}>", tableStyle));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, e.RegionCode));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, e.QueueId));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, e.IrmaItemKey));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, e.Identifier));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, e.IconItemId));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, e.FailureReason));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, e.ProcessedByController));
                builder.Append("</tr>");
            }

            builder.Append("</table>");

            return builder.ToString();
        } 
    }
}
