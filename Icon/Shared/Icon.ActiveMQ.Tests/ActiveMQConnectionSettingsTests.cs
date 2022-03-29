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
            string serverUrl = "failover:(ssl://b-b1a575b2-58e7-4bd5-b17a-03dbdd11e562-1.mq.us-west-2.amazonaws.com:61617,ssl://b-b1a575b2-58e7-4bd5-b17a-03dbdd11e562-2.mq.us-west-2.amazonaws.com:61617)?randomize=true";
            string username = "wfmdatavayuactivemqproducer";
            string password = "4dsV8hY1zm3HBOtq";
            string queueName = "test.ActiveMQLibrary";
            int reconnectDelay = 30000;
            AcknowledgementMode sessionMode = AcknowledgementMode.AutoAcknowledge;

            ActiveMQConnectionSettings settings = ActiveMQConnectionSettings.CreateSettingsFromConfig();
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
