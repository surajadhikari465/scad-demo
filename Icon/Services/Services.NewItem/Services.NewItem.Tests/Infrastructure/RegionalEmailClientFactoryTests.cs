using Icon.Common.Email;
using Services.NewItem.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.NewItem.Tests.Infrastructure
{
    [TestClass]
    public class RegionalEmailClientFactoryTests
    {
        private RegionalEmailClientFactory factory;

        [TestInitialize]
        public void Initialize()
        {
            factory = new RegionalEmailClientFactory();
        }

        [TestMethod]
        public void CreateEmailClient_RegionalRecipientExistsInAppConfig_ReturnsEmailClientWithAppSettingAsRecipient()
        {
            //Given
            var regionCode = "FL";

            //When
            var client = factory.CreateEmailClient(regionCode) as EmailClient;

            //Then
            Assert.AreEqual("smtp.wholefoods.com", client.Settings.Host);
            Assert.AreEqual("", client.Settings.Password);
            Assert.AreEqual(25, client.Settings.Port);
            Assert.AreEqual("FL@wholefoods.com", client.Settings.Recipients.Single());
            Assert.AreEqual(true, client.Settings.SendEmails);
            Assert.AreEqual("Icon-Dev@wholefoods.com", client.Settings.Sender);
            Assert.AreEqual("", client.Settings.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateEmailClient_RegionalRecipientDoesNotExistsInAppConfig_ThrowsException()
        {
            //Given
            var regionCode = "Not exists";

            //When
            var client = factory.CreateEmailClient(regionCode) as EmailClient;
        }
    }
}
