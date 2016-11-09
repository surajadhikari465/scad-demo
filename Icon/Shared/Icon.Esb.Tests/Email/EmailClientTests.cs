using Icon.Common.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Icon.Esb.Tests.Email
{
    [TestClass]
    public class EmailClientTests
    {
        [TestMethod]
        public void Constructor_EmailClientsSettingsHasValuesOfSettingsParameter()
        {
            //Given
            EmailClientSettings settings = new EmailClientSettings
            {
                Host = "Host",
                Password = "Password",
                Port = 5,
                Recipients = new string[] { "Rec1", "Rec2", "Rec3"},
                SendEmails = true,
                Sender = "Sender",
                Username = "Username"
            };

            //When
            EmailClient client = new EmailClient(settings);

            //Then
            Assert.AreEqual(settings.Host, client.Settings.Host);
            Assert.AreEqual(settings.Password, client.Settings.Password);
            Assert.AreEqual(settings.Port, client.Settings.Port);
            CollectionAssert.AreEqual(settings.Recipients, client.Settings.Recipients);
            Assert.AreEqual(settings.SendEmails, client.Settings.SendEmails);
            Assert.AreEqual(settings.Sender, client.Settings.Sender);
            Assert.AreEqual(settings.Username, client.Settings.Username);
        }

        [TestMethod]
        public void Send_SendEmailsIsSetToFalse_ShouldNotAttemptToSendAnEmailMade()
        {
            //Given
            EmailClientSettings settings = new EmailClientSettings
            {
                SendEmails = false
            };
            EmailClient client = new EmailClient(settings);

            //When
            client.Send("Test", "Test");

            //Then no assertions need to take place after the Send
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Send_SendEmailsIsTrueAndSettingsHaveIncompleteValues_ShouldThrowAnException()
        {
            //Given 
            EmailClientSettings settings = new EmailClientSettings
            {
                SendEmails = true
            };
            EmailClient client = new EmailClient(settings);

            //When
            client.Send("Test", "Test");
        }

    }
}
