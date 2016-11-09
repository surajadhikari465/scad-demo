using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushController.Controller.Decorators;
using System.Configuration;

namespace PushController.Tests.Controller.DecoratorTests
{
    [TestClass]
    public class PriceUomChangeConfigurationTests
    {
        private PriceUomChangeAlertConfiguration configuration;

        [TestMethod]
        public void PriceUomChangeConfiguration_InstantiateClass_PropertiesSet()
        {
            // Given
            string expectedSubject = ConfigurationManager.AppSettings["PriceUomChangeSubject"];
            string expectedRecipient = ConfigurationManager.AppSettings["PriceUomChangeRecipients"];
            string sendEmails = ConfigurationManager.AppSettings["SendUomChangeEmails"];
            
            bool expectedSendEmails = default(bool);
            Boolean.TryParse(sendEmails, out expectedSendEmails);

            // When
            this.configuration = new PriceUomChangeAlertConfiguration();

            // Then
            Assert.AreEqual(expectedSubject, this.configuration.PriceUomChangeSubject);
            Assert.AreEqual(expectedRecipient, this.configuration.PriceUomChangeRecipients);
            Assert.AreEqual(expectedSendEmails, this.configuration.SendEmails);
        }
    }
}
