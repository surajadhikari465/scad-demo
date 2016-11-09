using Icon.Common.Email;
using Icon.Framework;
using Icon.Testing.Builders;
using Irma.Framework;
using Irma.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushController.Common;
using PushController.Common.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;

namespace PushController.Tests.Controller.EmailHelperTests
{
    [TestClass]
    public class EmailHelperTests
    {
        private EmailClient emailClient;

        [TestInitialize]
        public void Initialize()
        {
            string domain = "wfm.pvt";
            string username = WindowsIdentity.GetCurrent().Name;
            PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, domain);
            UserPrincipal user = UserPrincipal.FindByIdentity(domainContext, username);

            var emailClientSettings = EmailHelper.BuildEmailClientSettings();
            emailClientSettings.Recipients = new string[] { user.EmailAddress };

            emailClient = new EmailClient(emailClientSettings);
        }

        [TestMethod]
        public void BuildMessageBodyForInvalidSaleDates_WithValidParameters_EmailShouldBeSentSuccessfully()
        {
            // Given.
            var invalidSaleDateRecords = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(113).WithIdentifier("11111").WithSaleEndDate(DateTime.Now).WithSaleStartDate(DateTime.Now.AddDays(1)).WithChangeType(Constants.IrmaPushChangeTypes.NonRegularPriceChange).WithSalePrice(1.99m),
                new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(113).WithIdentifier("22222").WithSaleEndDate(DateTime.Now).WithSaleStartDate(DateTime.Now.AddDays(1)).WithChangeType(Constants.IrmaPushChangeTypes.NonRegularPriceChange).WithSalePrice(1.99m),
                new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(113).WithIdentifier("33333").WithSaleEndDate(DateTime.Now).WithSaleStartDate(DateTime.Now.AddDays(1)).WithChangeType(Constants.IrmaPushChangeTypes.NonRegularPriceChange).WithSalePrice(1.99m),
            };

            string message = EmailHelper.BuildMessageBodyForInvalidSaleDates("The following items have the sale start date after the sale end date:", invalidSaleDateRecords);
            string subject = "POS Push Controller Automated Test - Incorrect Sale Start and End Dates";

            // When.
            emailClient.Send(message, subject);

            // Then.
            // Test will pass as long as the email sends successfully.  You've got mail!
        }

        [TestMethod]
        public void BuildMessageBodyForUnhandledException_WithValidParameters_EmailShouldBeSentSuccessfully()
        {
            // Given.
            string message = EmailHelper.BuildMessageBodyForUnhandledException("An unhandled exception occurred.", "Exception text goes here.");
            string subject = "POS Push Controller Automated Test - Unhandled Exception Occurred";

            // When.
            emailClient.Send(message, subject);

            // Then.
            // Test will pass as long as the email sends successfully.  You've got mail!
        }

        [TestMethod]
        public void BuildMessageBodyForFailedIrmaPushConversion_WithValidParameters_EmailShouldBeSentSuccessfully()
        {
            // Given.
            var failedConversionRecords = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(113).WithIdentifier("11111").WithSaleEndDate(DateTime.Now).WithSaleStartDate(DateTime.Now.AddDays(1)).WithChangeType(Constants.IrmaPushChangeTypes.NonRegularPriceChange).WithSalePrice(1.99m),
                new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(113).WithIdentifier("22222").WithSaleEndDate(DateTime.Now).WithSaleStartDate(DateTime.Now.AddDays(1)).WithChangeType(Constants.IrmaPushChangeTypes.NonRegularPriceChange).WithSalePrice(1.99m),
                new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(113).WithIdentifier("33333").WithSaleEndDate(DateTime.Now).WithSaleStartDate(DateTime.Now.AddDays(1)).WithChangeType(Constants.IrmaPushChangeTypes.NonRegularPriceChange).WithSalePrice(1.99m),
            };

            string message = EmailHelper.BuildMessageBodyForFailedIrmaPushConversion("The following records could not be converted to IRMAPush objects:", failedConversionRecords);
            string subject = "POS Push Controller Automated Test - Failed to Convert Records to IRMAPush";

            // When.
            emailClient.Send(message, subject);

            // Then.
            // Test will pass as long as the email sends successfully.  You've got mail!
        }

        [TestMethod]
        public void BuildMessageBodyForBulkInsertFailure_WithValidParameters_EmailShouldBeSentSuccessfully()
        {
            // Given.
            string message = EmailHelper.BuildMessageBodyForBulkInsertFailure("IRMAPush/ItemLocaleMessage/PriceMessage bulk insert failed.", "Exception text goes here.");
            string subject = "POS Push Controller Automated Test - IRMAPush Bulk Staging Failed";

            // When.
            emailClient.Send(message, subject);

            // Then.
            // Test will pass as long as the email sends successfully.  You've got mail!
        }

        [TestMethod]
        public void BuildMessageBodyForIrmaPushRowByRowFailure_WithValidParameters_EmailShouldBeSentSuccessfully()
        {
            // Given.
            var failedIrmaPushRecords = new List<IrmaPushModel>
            {
                new IrmaPushModel{ RegionCode = "SW", Identifier = "22222" },
                new IrmaPushModel{ RegionCode = "SW", Identifier = "22222" },
                new IrmaPushModel{ RegionCode = "SW", Identifier = "22222" }
            };

            string message = EmailHelper.BuildMessageBodyForIrmaPushRowByRowFailure("The following records could not be staged in IRMAPush:", failedIrmaPushRecords);
            string subject = "POS Push Controller Automated Test - Row-by-Row Insert Failed";

            // When.
            emailClient.Send(message, subject);

            // Then.
            // Test will pass as long as the email sends successfully.  You've got mail!
        }

        [TestMethod]
        public void BuildMessageBodyForMessageBuildFailure_WithValidParameters_EmailShouldBeSentSuccessfully()
        {
            // Given.
            var failedItemLocaleMessages = new List<IrmaPushModel>
            {
                new TestIrmaPushBuilder().Build().ToModel(),
                new TestIrmaPushBuilder().Build().ToModel(),
                new TestIrmaPushBuilder().Build().ToModel()
            };

            foreach (var failedMessage in failedItemLocaleMessages)
            {
                failedMessage.MessageBuildError = "Sumpthin' happened.";
            }

            string message = EmailHelper.BuildMessageBodyForMessageBuildFailure("Could not build ItemLocale/Price messages for the following:", failedItemLocaleMessages);
            string subject = "POS Push Controller Automated Test - Failed to Build ItemLocale Messages";

            // When.
            emailClient.Send(message, subject);

            // Then.
            // Test will pass as long as the email sends successfully.  You've got mail!
        }

        [TestMethod]
        public void BuildMessageBodyForItemLocaleMessageInsertRowByRowFailure_WithValidParameters_EmailShouldBeSentSuccessfully()
        {
            // Given.
            var failedItemLocaleMessages = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder(),
                new TestItemLocaleMessageBuilder(),
                new TestItemLocaleMessageBuilder()
            };

            string message = EmailHelper.BuildMessageBodyForItemLocaleMessageInsertRowByRowFailure("Could not insert the following ItemLocale messages:", failedItemLocaleMessages);
            string subject = "POS Push Controller Automated Test - Failed to Save ItemLocale Messages";

            // When.
            emailClient.Send(message, subject);

            // Then.
            // Test will pass as long as the email sends successfully.  You've got mail!
        }

        [TestMethod]
        public void BuildMessageBodyForPriceMessageInsertRowByRowFailure_WithValidParameters_EmailShouldBeSentSuccessfully()
        {
            // Given.
            var failedPriceMessages = new List<MessageQueuePrice>
            {
                new TestPriceMessageBuilder(),
                new TestPriceMessageBuilder(),
                new TestPriceMessageBuilder()
            };

            string message = EmailHelper.BuildMessageBodyForPriceMessageInsertRowByRowFailure("Could not insert the following Price messages:", failedPriceMessages);
            string subject = "POS Push Controller Automated Test - Failed to Save Price Messages";

            // When.
            emailClient.Send(message, subject);

            // Then.
            // Test will pass as long as the email sends successfully.  You've got mail!
        }

        [TestMethod]
        public void BuildMessageBodyForUdmBuildFailure_WithValidParameters_EmailShouldBeSentSuccessfully()
        {
            // Given.
            var failedIrmaPushRecords = new List<IRMAPush>
            {
                new TestIrmaPushBuilder(),
                new TestIrmaPushBuilder(),
                new TestIrmaPushBuilder()
            };

            string message = EmailHelper.BuildMessageBodyForUdmBuildFailure("Failed to build ItemLink/ItemPrice entities for the following:", failedIrmaPushRecords);
            string subject = "POS Push Controller Automated Test - Failed to Build UDM Updates";

            // When.
            emailClient.Send(message, subject);

            // Then.
            // Test will pass as long as the email sends successfully.  You've got mail!
        }

        [TestMethod]
        public void BuildMessageBodyForItemLinkInsertRowByRowFailure_WithValidParameters_EmailShouldBeSentSuccessfully()
        {
            // Given.
            var failedItemLinkUpdates = new List<ItemLinkModel>
            {
                new ItemLinkModel(),
                new ItemLinkModel(),
                new ItemLinkModel()
            };

            string message = EmailHelper.BuildMessageBodyForItemLinkInsertRowByRowFailure("Failed to update the following ItemLink entities:", failedItemLinkUpdates);
            string subject = "POS Push Controller Automated Test - Failed to Save ItemLink Entities";

            // When.
            emailClient.Send(message, subject);

            // Then.
            // Test will pass as long as the email sends successfully.  You've got mail!
        }

        [TestMethod]
        public void BuildMessageBodyForItemPriceInsertRowByRowFailure_WithValidParameters_EmailShouldBeSentSuccessfully()
        {
            // Given.
            var failedItemPriceUpdates = new List<ItemPriceModel>
            {
                new ItemPriceModel(),
                new ItemPriceModel(),
                new ItemPriceModel()
            };

            string message = EmailHelper.BuildMessageBodyForItemPriceInsertRowByRowFailure("Failed to update the following ItemPrice entities::", failedItemPriceUpdates);
            string subject = "POS Push Controller Automated Test - Failed to Save ItemPrice Entities";

            // When.
            emailClient.Send(message, subject);

            // Then.
            // Test will pass as long as the email sends successfully.  You've got mail!
        }
    }
}
