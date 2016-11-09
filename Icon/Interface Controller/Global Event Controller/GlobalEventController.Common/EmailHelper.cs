using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalEventController.Common
{
    public static class EmailHelper
    {
        private static string tableStyle = "style='border:1px solid black;border-collapse:collapse;font-family:Segoe UI;padding:10px;'";

        public static string BuildEventQueueTable(IEnumerable<FailedEvent> events)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(String.Format("<table {0}", tableStyle));

            //Create header row
            builder.Append(String.Format("<tr {0}>", tableStyle))
                .Append(String.Format("<th {0}>QueueId</th>", tableStyle))
                .Append(String.Format("<th {0}>EventId</th>", tableStyle))
                .Append(String.Format("<th {0}>EventMessage</th>", tableStyle))
                .Append(String.Format("<th {0}>EventReferenceId</th>", tableStyle))
                .Append(String.Format("<th {0}>RegionCode</th>", tableStyle))
                .Append(String.Format("<th {0}>InsertDate</th>", tableStyle))
                .Append(String.Format("<th {0}>FailureReason</th>", tableStyle))
                .Append("</tr>");

            //Create new row foreach event
            foreach (var e in events)
            {
                builder.Append(String.Format("<tr {0}>", tableStyle));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, e.Event.QueueId));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, e.Event.EventId));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, e.Event.EventMessage));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, e.Event.EventReferenceId));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, e.Event.RegionCode));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, e.Event.InsertDate));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, e.FailureReason));
                builder.Append("</tr>");
            }

            builder.Append("</table>");

            return builder.ToString();
        }

        public static string BuildUomChangeTable(IEnumerable<IrmaItemModel> uomChanges)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(String.Format("<table {0}", tableStyle));

            //Create header row
            builder.Append(String.Format("<tr {0}>", tableStyle))
                .Append(String.Format("<th {0}>ScanCode</th>", tableStyle))
                .Append(String.Format("<th {0}>Product Description</th>", tableStyle))
                .Append(String.Format("<th {0}>SubTeam Name</th>", tableStyle))
                .Append(String.Format("<th {0}>Retail Pack</th>", tableStyle))
                .Append(String.Format("<th {0}>Retail Size</th>", tableStyle))
                .Append(String.Format("<th {0}>Existing Retail UOM</th>", tableStyle))
                .Append(String.Format("<th {0}>New Retail UOM</th>", tableStyle))
                .Append(String.Format("<th {0}>Existing Retail Unit</th>", tableStyle))
                .Append(String.Format("<th {0}>New Retail Unit</th>", tableStyle))
                .Append("</tr>");

            //Create new row foreach event
            foreach (var uomChange in uomChanges)
            {
                builder.Append(String.Format("<tr {0}>", tableStyle));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, uomChange.Identifier));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, uomChange.Description));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, uomChange.SubTeamName));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, uomChange.RetailPack));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, uomChange.RetailSize));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, uomChange.RetailUomAbbreviation));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, uomChange.IconRetailUomAbbreviation));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, uomChange.RetailUnitAbbreviation));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, uomChange.IconRetailUomAbbreviation == "LB" ? "LB" : "EA"));
                builder.Append("</tr>");
            }

            builder.Append("</table>");

            return builder.ToString();
        }

        public static string BuildRegionalItemMessageTable(IEnumerable<RegionalItemMessageModel> message)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(String.Format("<table {0}", tableStyle));

            //Create header row
            builder.Append(String.Format("<tr {0}>", tableStyle))
                .Append(String.Format("<th {0}>RegionCode</th>", tableStyle))
                .Append(String.Format("<th {0}>Identifier</th>", tableStyle))
                .Append(String.Format("<th {0}>Message</th>", tableStyle))
                .Append("</tr>");

            //Create new row foreach event
            foreach (var uomChange in message)
            {
                builder.Append(String.Format("<tr {0}>", tableStyle));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, uomChange.RegionCode));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, uomChange.Identifier));
                builder.Append(String.Format("<td {0}>{1}</td>", tableStyle, uomChange.Message));
                builder.Append("</tr>");
            }

            builder.Append("</table>");

            return builder.ToString();
        }
    }
}
