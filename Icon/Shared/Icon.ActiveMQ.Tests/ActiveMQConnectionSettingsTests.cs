using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Apache.NMS;

namespace Icon.ActiveMQ.Tests
{
    [TestClass]
    public class ActiveMQConnectionSettingsTests
    {
        [TestMethod]
        public void CreateSettingsFromConfigMethodTest()
        {
            // App.config values - Needed to match exactly with App.config
            string serverUrl = "activemq:ssl://b-ca82e329-8e4f-4160-bc42-caa0e4624b0a-1.mq.us-west-2.amazonaws.com:61617";
            string username = "quintessence";
            string password = "quintessence";
            string queueName = "UnitTest-Queue";
            int reconnectDelay = 30000;
            AcknowledgementMode sessionMode = AcknowledgementMode.AutoAcknowledge;

            ActiveMQConnectionSettings settings = ActiveMQConnectionSettings.CreateSettingsFromConfig("QueueName");
            Assert.IsNotNull(settings);

            // Values check
            Assert.AreEqual(settings.ServerUrl, serverUrl);
            Assert.AreEqual(settings.JmsUsername, username);
            Assert.AreEqual(settings.JmsPassword, password);
            Assert.AreEqual(settings.ReconnectDelay, reconnectDelay);
            Assert.AreEqual(settings.SessionMode, sessionMode);
            Assert.AreEqual(settings.QueueName, queueName);
        }
    }
}
