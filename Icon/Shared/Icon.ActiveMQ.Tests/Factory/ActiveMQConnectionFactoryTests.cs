using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.ActiveMQ.Factory;
using Icon.ActiveMQ.Producer;
using System;


namespace Icon.ActiveMQ.Tests.Factory
{
    [TestClass]
    public class ActiveMQConnectionFactoryTests
    {
        [TestMethod]
        public void CreateProducerShouldReturnActiveMQProducer()
        {

            // CreateProducer() static Method
            ActiveMQProducer producer = ActiveMQConnectionFactory.CreateProducer(ActiveMQConnectionSettings.CreateSettingsFromConfig());
            Assert.IsNotNull(producer);
            producer.Dispose();

            // CreateProducer(clientId, openConnection) Method
            ActiveMQConnectionFactory factory = new ActiveMQConnectionFactory(ActiveMQConnectionSettings.CreateSettingsFromConfig());
            producer = (ActiveMQProducer) factory.CreateProducer("ClientID");
            Assert.IsNotNull(producer);
            producer.Dispose();
        }

        [TestMethod]
        public void ProducerIsConnectedForOpenConnectionTrue()
        {
            ActiveMQConnectionFactory factory = new ActiveMQConnectionFactory(ActiveMQConnectionSettings.CreateSettingsFromConfig());

            // openConnection parameter is True, it should create Connection
            ActiveMQProducer producer = (ActiveMQProducer) factory.CreateProducer("ClientID", true);
            Assert.IsTrue(producer.IsConnected);
            producer.Dispose();
        }

        [TestMethod]
        public void ProducerIsNotConnectedForOpenConnectionFalse()
        {
            ActiveMQConnectionFactory factory = new ActiveMQConnectionFactory(ActiveMQConnectionSettings.CreateSettingsFromConfig());

            // openConnection parameter is false, it should not create connection
            ActiveMQProducer producer = (ActiveMQProducer) factory.CreateProducer("ClientID", false);
            Assert.IsFalse(producer.IsConnected);
            producer.Dispose();
        }
    }
}
