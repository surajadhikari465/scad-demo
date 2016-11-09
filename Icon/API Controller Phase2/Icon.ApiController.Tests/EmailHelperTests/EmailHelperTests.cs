using Icon.ApiController.Common;
using Icon.Common.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;

namespace Icon.ApiController.Tests.EmailHelperTests
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

            ControllerType.Type = "Product";
            ControllerType.Instance = 88;
        }

        [TestMethod]
        public void BuildMessageBodyForUnhandledException_WithValidParameters_EmailShouldBeSentSuccessfully()
        {
            // Given.
            string message = EmailHelper.BuildMessageBodyForUnhandledException(
                String.Format(Resource.HistoryProcessorUnhandledExceptionMessage, ControllerType.Type, ControllerType.Instance),
                "Exception text goes here.");

            string subject = "API Controller Automated Test - Unhandled Exception Occurred";

            // When.
            emailClient.Send(message, subject);

            // Then.
            // Test will pass as long as the email sends successfully.  You've got mail!
        }

        [TestMethod]
        public void BuildMessageBodyForMiniBulkError_WithValidParameters_EmailShouldBeSentSuccessfully()
        {
            // Given.
            string message = EmailHelper.BuildMessageBodyForMiniBulkError(
                String.Format(Resource.FailedToAddQueuedMessageToMiniBulkMessage, ControllerType.Type, ControllerType.Instance), 
                222, 
                "Couldn't do it, sorry.");

            string subject = "API Controller Automated Test - Failed to Add Message to the Mini-Bulk";

            // When.
            emailClient.Send(message, subject);

            // Then.
            // Test will pass as long as the email sends successfully.  You've got mail!
        }

        [TestMethod]
        public void BuildMessageBodyForSerializationFailure_WithValidParameters_EmailShouldBeSentSuccessfully()
        {
            // Given.
            string message = EmailHelper.BuildMessageBodyForSerializationFailure(
                String.Format(Resource.FailedToSerializeMiniBulkMessage, ControllerType.Type, ControllerType.Instance),
                "Nope.");

            string subject = "API Controller Automated Test - Serialization Error";

            // When.
            emailClient.Send(message, subject);

            // Then.
            // Test will pass as long as the email sends successfully.  You've got mail!
        }
    }
}
