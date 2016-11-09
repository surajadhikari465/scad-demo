using Icon.Common.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Icon.Esb.Tests.Email
{
    [TestClass]
    public class EmailClientSettingsTests
    {
        Dictionary<string, string> settingsCopy;

        [TestInitialize]
        public void Initialize()
        {
            settingsCopy = new Dictionary<string, string>();
            TestHelpers.CopyOfAppSettings(settingsCopy);
            TestHelpers.ClearAppSettings(settingsCopy);
        }

        [TestCleanup]
        public void Cleanup()
        {
            TestHelpers.SetAppSettings(settingsCopy);
        }

        [TestCategory("EmailClientSettings")]
        [TestMethod]
        public void LoadFromConfig_AllSettingsExistInConfig_LoadsAllPropertiesToSettings()
        {
            //Given
            ConfigurationManager.AppSettings.Set("SendEmails", "true");
            ConfigurationManager.AppSettings.Set("EmailHost", "TestEmailHost");
            ConfigurationManager.AppSettings.Set("EmailPort", "55");
            ConfigurationManager.AppSettings.Set("EmailUsername", "TestEmailUsername");
            ConfigurationManager.AppSettings.Set("EmailPassword", "TestEmailPassword");
            ConfigurationManager.AppSettings.Set("Sender", "TestEmailSender");
            ConfigurationManager.AppSettings.Set("Recipients", "TestEmailRecipients");

            //When 
            EmailClientSettings settings = new EmailClientSettings();
            settings.LoadFromConfig();

            //Then
            Assert.AreEqual(true, settings.SendEmails);
            Assert.AreEqual("TestEmailHost", settings.Host);
            Assert.AreEqual(55, settings.Port);
            Assert.AreEqual("TestEmailUsername", settings.Username);
            Assert.AreEqual("TestEmailPassword", settings.Password);
            Assert.AreEqual("TestEmailSender", settings.Sender);
            Assert.AreEqual(1, settings.Recipients.Length);
            Assert.AreEqual("TestEmailRecipients", settings.Recipients.First());
        }

        [TestCategory("EmailClientSettings")]
        [TestMethod]
        public void LoadFromConfig_MultipleRecipientsExistAndAreSeparatedByCommas_RecipientsAreLoadedWithAllRecipientsFromConfig()
        {
            //Given
            ConfigurationManager.AppSettings.Set("Recipients", "rec1@recipients.com,rec2@recipients.com,rec3@recipients.com");

            //When
            EmailClientSettings settings = new EmailClientSettings();
            settings.LoadFromConfig();

            //Then
            Assert.AreEqual(3, settings.Recipients.Length);
            Assert.AreEqual("rec1@recipients.com", settings.Recipients[0]);
            Assert.AreEqual("rec2@recipients.com", settings.Recipients[1]);
            Assert.AreEqual("rec3@recipients.com", settings.Recipients[2]);
        }
    }
}
