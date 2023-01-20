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
            string serverUrl = "failover:(ssl://b-30e2193f-fe4b-4212-831e-58ef6e0e2784-1.mq.us-west-2.amazonaws.com:61617,ssl://b-ebf4b796-b53d-4480-960c-caa3e8e06722-1.mq.us-west-2.amazonaws.com:61617)?randomize=true";
            string username = "wfmdatavayuactivemqv2producer";
            string password = "f16WTI8Uqbh3CCIq";
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
