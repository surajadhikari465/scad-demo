using Icon.Common.Email;
using Icon.Framework;
using Irma.Framework;
using PushController.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushController.Common
{
    public static class EmailHelper
    {
        private static string tableStyle = "style='border:1px solid black;border-collapse:collapse;font-family:Segoe UI;font-size:15px;padding:10px;'";

        public static EmailClientSettings BuildEmailClientSettings()
        {
            return EmailClientSettings.CreateFromConfig();
        }

        public static string BuildMessageBodyForInvalidSaleDates(string errorMessage, List<IConPOSPushPublish> invalidSaleDateRecords)
        {
            var messageBody = new StringBuilder();

            BuildMessageHeader(errorMessage, messageBody);

            messageBody.AppendLine(String.Format("<table {0}>", tableStyle));
            messageBody.AppendLine(String.Format("<tr {0}><th {0}>Identifier</th><th {0}>Store Number</th><th {0}>Business Unit</th><th {0}>Sale Price</th><th {0}>Sale Start Date</th><th {0}>Sale End Date</th></tr>", tableStyle));

            foreach (var invalidRecord in invalidSaleDateRecords)
            {
                messageBody.AppendLine(String.Format("<tr {0}><td {0}>{1}</td><td {0}>{2}</td><td {0}>{3}</td><td {0}>{4}</td><td {0}>{5}</td><td {0}>{6}</td></tr>", tableStyle,
                    invalidRecord.Identifier, invalidRecord.Store_No, invalidRecord.BusinessUnit_ID, invalidRecord.Sale_Price, invalidRecord.Sale_Start_Date.Value.ToShortDateString(), invalidRecord.Sale_End_Date.Value.ToShortDateString()));
            }

            messageBody.AppendLine("</table>");

            BuildMessageFooter(String.Empty, messageBody);

            return messageBody.ToString();
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

        public static string BuildMessageBodyForFailedIrmaPushConversion(string errorMessage, List<IConPOSPushPublish> failedConversionRecords)
        {
            var messageBody = new StringBuilder();

            BuildMessageHeader(errorMessage, messageBody);

            messageBody.AppendLine(String.Format("<table {0}>", tableStyle));
            messageBody.AppendLine(String.Format("<tr {0}><th {0}>Region</th><th {0}>Scan Code</th><th {0}>IconPosPushPublish ID</th></tr>", tableStyle));

            foreach (var failedRecord in failedConversionRecords)
            {
                messageBody.AppendLine(String.Format("<tr {0}><td {0}>{1}</td><td {0}>{2}</td><td {0}>{3}</td></tr>", tableStyle,
                    failedRecord.RegionCode, failedRecord.Identifier, failedRecord.IConPOSPushPublishID));
            }

            messageBody.AppendLine("</table>");

            string footerMessage = "Please check the AppLog table for more detailed error information.";
            BuildMessageFooter(footerMessage, messageBody);

            return messageBody.ToString();
        }

        public static string BuildMessageBodyForBulkInsertFailure(string errorMessage, string exceptionText)
        {
            var messageBody = new StringBuilder();

            BuildMessageHeader(errorMessage, messageBody);

            messageBody.AppendLine(exceptionText);

            string footerMessage = String.Empty;
            BuildMessageFooter(footerMessage, messageBody);

            return messageBody.ToString();
        }

        public static string BuildMessageBodyForIrmaPushRowByRowFailure(string errorMessage, List<IrmaPushModel> failedRecords)
        {
            var messageBody = new StringBuilder();

            BuildMessageHeader(errorMessage, messageBody);

            messageBody.AppendLine(String.Format("<table {0}>", tableStyle));
            messageBody.AppendLine(String.Format("<tr {0}><th {0}>Region</th><th {0}>Scan Code</th><th {0}>IconPosPushPublish ID</th></tr>", tableStyle));

            foreach (var failedRecord in failedRecords)
            {
                messageBody.AppendLine(String.Format("<tr {0}><td {0}>{1}</td><td {0}>{2}</td><td {0}>{3}</td></tr>", tableStyle,
                    failedRecord.RegionCode, failedRecord.Identifier, failedRecord.IconPosPushPublishId));
            }

            messageBody.AppendLine("</table>");

            string footerMessage = "Please check the AppLog table for more detailed error information.";
            BuildMessageFooter(footerMessage, messageBody);

            return messageBody.ToString();
        }

        public static string BuildMessageBodyForMessageBuildFailure(string errorMessage, List<IrmaPushModel> failedRecords)
        {
            var messageBody = new StringBuilder();

            BuildMessageHeader(errorMessage, messageBody);

            messageBody.AppendLine(String.Format("<table {0}>", tableStyle));
            messageBody.AppendLine(String.Format("<tr {0}><th {0}>IRMAPushID</th><th {0}>Scan Code</th><th {0}>Region</th><th {0}>Business Unit ID</th><th {0}>Error</th></tr>", tableStyle));

            foreach (var failedRecord in failedRecords)
            {
                messageBody.AppendLine(String.Format("<tr {0}><td {0}>{1}</td><td {0}>{2}</td><td {0}>{3}</td><td {0}>{4}</td><td {0}>{5}</td></tr>",
                    tableStyle, failedRecord.IrmaPushId, failedRecord.Identifier, failedRecord.RegionCode, failedRecord.BusinessUnitId, failedRecord.MessageBuildError));
            }

            messageBody.AppendLine("</table>");

            string footerMessage = "Please check the AppLog table for additional information.";
            BuildMessageFooter(footerMessage, messageBody);

            return messageBody.ToString();
        }

        public static string BuildMessageBodyForItemLocaleMessageInsertRowByRowFailure(string failureMessage, List<MessageQueueItemLocale> failedMessages)
        {
            var messageBody = new StringBuilder();

            BuildMessageHeader(failureMessage, messageBody);

            messageBody.AppendLine(String.Format("<table {0}>", tableStyle));
            messageBody.AppendLine(String.Format("<tr {0}><th {0}>IRMAPushID</th><th {0}>Scan Code</th><th {0}>Region</th><th {0}>Business Unit ID</th></tr>", tableStyle));

            foreach (var failedMessage in failedMessages)
            {
                messageBody.AppendLine(String.Format("<tr {0}><td {0}>{1}</td><td {0}>{2}</td><td {0}>{3}</td><td {0}>{4}</td></tr>", tableStyle, failedMessage.IRMAPushID, failedMessage.ScanCode, failedMessage.RegionCode, failedMessage.BusinessUnit_ID));
            }

            messageBody.AppendLine("</table>");

            string footerMessage = "Please check the AppLog table for more detailed error information.";
            BuildMessageFooter(footerMessage, messageBody);

            return messageBody.ToString();
        }

        public static string BuildMessageBodyForPriceMessageInsertRowByRowFailure(string failureMessage, List<MessageQueuePrice> failedMessages)
        {
            var messageBody = new StringBuilder();

            BuildMessageHeader(failureMessage, messageBody);

            messageBody.AppendLine(String.Format("<table {0}>", tableStyle));
            messageBody.AppendLine(String.Format("<tr {0}><th {0}>IRMAPushID</th><th {0}>Scan Code</th><th {0}>Region</th><th {0}>Business Unit ID</th></tr>", tableStyle));

            foreach (var failedMessage in failedMessages)
            {
                messageBody.AppendLine(String.Format("<tr {0}><td {0}>{1}</td><td {0}>{2}</td><td {0}>{3}</td><td {0}>{4}</td></tr>", tableStyle, failedMessage.IRMAPushID, failedMessage.ScanCode, failedMessage.RegionCode, failedMessage.BusinessUnit_ID));
            }

            messageBody.AppendLine("</table>");

            string footerMessage = "Please check the AppLog table for more detailed error information.";
            BuildMessageFooter(footerMessage, messageBody);

            return messageBody.ToString();
        }

        public static string BuildMessageBodyForUdmBuildFailure(string failureMessage, List<IRMAPush> failedRecords)
        {
            var messageBody = new StringBuilder();

            BuildMessageHeader(failureMessage, messageBody);

            messageBody.AppendLine(String.Format("<table {0}>", tableStyle));
            messageBody.AppendLine(String.Format("<tr {0}><th {0}>IRMAPushID</th><th {0}>Scan Code</th><th {0}>Region</th><th {0}>Business Unit ID</th></tr>", tableStyle));

            foreach (var failedRecord in failedRecords)
            {
                messageBody.AppendLine(String.Format("<tr {0}><td {0}>{1}</td><td {0}>{2}</td><td {0}>{3}</td><td {0}>{4}</td></tr>", tableStyle, failedRecord.IRMAPushID, failedRecord.Identifier, failedRecord.RegionCode, failedRecord.BusinessUnit_ID));
            }

            messageBody.AppendLine("</table>");

            string footerMessage = "Please check the AppLog table for more detailed error information.";
            BuildMessageFooter(footerMessage, messageBody);

            return messageBody.ToString();
        }

        public static string BuildMessageBodyForItemLinkInsertRowByRowFailure(string failureMessage, List<ItemLinkModel> failedEntities)
        {
            var messageBody = new StringBuilder();

            BuildMessageHeader(failureMessage, messageBody);

            messageBody.AppendLine(String.Format("<table {0}>", tableStyle));
            messageBody.AppendLine(String.Format("<tr {0}><th {0}>IRMA Push ID</th><th {0}>Parent Item ID</th><th {0}>Child Item ID</th><th {0}>Locale ID</th></tr>", tableStyle));

            foreach (var failedEntity in failedEntities)
            {
                messageBody.AppendLine(String.Format("<tr {0}><td {0}>{1}</td><td {0}>{2}</td><td {0}>{3}</td><td {0}>{4}</td></tr>", 
                    tableStyle, failedEntity.IrmaPushId, failedEntity.ParentItemId, failedEntity.ChildItemId, failedEntity.LocaleId));
            }

            messageBody.AppendLine("</table>");

            string footerMessage = "Please check the AppLog table for more detailed error information.";
            BuildMessageFooter(footerMessage, messageBody);

            return messageBody.ToString();
        }

        public static string BuildMessageBodyForItemPriceInsertRowByRowFailure(string failureMessage, List<ItemPriceModel> failedEntities)
        {
            var messageBody = new StringBuilder();

            BuildMessageHeader(failureMessage, messageBody);

            messageBody.AppendLine(String.Format("<table {0}>", tableStyle));
            messageBody.AppendLine(String.Format("<tr {0}><th {0}>IRMA Push ID</th><th {0}>Item ID</th><th {0}>Locale ID</th><th {0}>ItemPriceType ID</th><th {0}>Price Amount</th></tr>", tableStyle));

            foreach (var failedEntity in failedEntities)
            {
                messageBody.AppendLine(String.Format("<tr {0}><td {0}>{1}</td><td {0}>{2}</td><td {0}>{3}</td><td {0}>{4}</td><td {0}>{5}</td></tr>", tableStyle,
                    failedEntity.IrmaPushId, failedEntity.ItemId, failedEntity.LocaleId, failedEntity.ItemPriceTypeId, failedEntity.ItemPriceAmount));
            }

            messageBody.AppendLine("</table>");

            string footerMessage = "Please check the AppLog table for more detailed error information.";
            BuildMessageFooter(footerMessage, messageBody);

            return messageBody.ToString();
        }

        public static string BuildMessageBodyForPriceUomChanges(List<PriceUomChangeModel> changedRecords)
        {
            var messageBody = new StringBuilder();
            string headerMessage = "Impacted User,";
            BuildMessageHeader(headerMessage, messageBody);
            headerMessage = @"Please be advised that a UOM change has been initiated for the following item within IRMA, " +
                "and please reference the table below to understand what this change entails:";
            BuildMessageHeader(headerMessage, messageBody);

            messageBody.AppendLine(String.Format("<table {0}>", tableStyle));
            messageBody.AppendLine(String.Format("<tr {0}><th {0}>Scan Code</th><th {0}>Region</th><th {0}>Business Unit ID</th><th {0}>Price</th><th {0}>Multiple</th>" +
                "<th {0}>Current POS UOM</th><th {0}>New POS UOM</th></tr>",
                tableStyle));

            foreach (var row in changedRecords)
            {
                messageBody.AppendLine(String.Format("<tr {0}><td {0}>{1}</td><td {0}>{2}</td><td {0}>{3}</td><td {0}>{4}</td><td {0}>{5}</td><td {0}>{6}</td><td {0}>{7}</td></tr>",
                    tableStyle,
                    row.Identifier,
                    row.RegionCode,
                    row.BusinessUnit_ID,
                    row.Price,
                    row.Multiple,
                    row.CurrentPosUom,
                    row.NewPosUom));
            }

            messageBody.AppendLine("</table>");

            string footerMessage = @"The impact of this change needs to be assessed, and a manual intervention is required to remove the secondary (now obsolete) " +
                "regular price record from R10, via the Office Client.  Please adhere to the defined business process to address the price records for this item.";
            BuildMessageFooter(footerMessage, messageBody);
            footerMessage = "Regards,";
            BuildMessageFooter(footerMessage, messageBody);
            footerMessage = "IRMA Applications Team";
            BuildMessageFooter(footerMessage, messageBody);

            return messageBody.ToString();
        }

        private static void BuildMessageFooter(string footerMessage, StringBuilder messageBody)
        {
            messageBody.AppendLine("</div>");

            messageBody.AppendLine("<div style='font-family:Segoe UI;font-size:15px;margin-top:50px;'>");
            messageBody.AppendLine(footerMessage);
            messageBody.AppendLine("</div>");
        }

        private static void BuildMessageHeader(string headerMessage, StringBuilder messageBody)
        {
            messageBody.AppendLine("<div style='font-family:Segoe UI;font-size:15px;'>");
            messageBody.AppendLine(headerMessage);
            messageBody.AppendLine("</div>");

            messageBody.AppendLine("<div style='margin-top:50px;font-family:Segoe UI;font-size:15px;'>");
        }
    }
}
