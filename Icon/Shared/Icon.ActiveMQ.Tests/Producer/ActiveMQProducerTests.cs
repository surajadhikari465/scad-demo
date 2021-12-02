using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Icon.ActiveMQ.Producer;

namespace Icon.ActiveMQ.Tests.Producer
{
    [TestClass]
    public class ActiveMQProducerTests
    {
        [TestMethod]
        public void SendMethodTest()
        {
            ActiveMQConnectionSettings settings = ActiveMQConnectionSettings.CreateSettingsFromConfig();
            ActiveMQProducer producer = new ActiveMQProducer(settings);

            string message = "Test Message";
            Dictionary<string, string> messageProperties = new Dictionary<string, string> ();
            messageProperties.Add("messageID", "TestMessageProperties");

            // Should be sending to the Queue without Exceptions, if settings point to correct Queue
            producer.Send(message, messageProperties);
            producer.Dispose();
        }

        [TestMethod]
        public void OpenConnectionMethodTest()
        {
            ActiveMQConnectionSettings settings = ActiveMQConnectionSettings.CreateSettingsFromConfig();
            ActiveMQProducer producer = new ActiveMQProducer(settings);

            producer.OpenConnection("ClientID");
            Assert.IsTrue(producer.IsConnected);
            producer.Dispose();
        }
    }
}
