using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.ActiveMQ;

namespace Icon.ActiveMQ.Tests
{
    [TestClass]
    public class ActiveMQConnectionTests
    {
        [TestMethod]
        public void OpenConnectionMethodTest()
        {
            ActiveMQConnectionSettings settings = ActiveMQConnectionSettings.CreateSettingsFromConfig();
            ActiveMQConnection connection = new ActiveMQConnection(settings);

            connection.OpenConnection("ClientID");

            Assert.IsTrue(connection.IsConnected);
            connection.Dispose();
        }

        [TestMethod]
        public void DisposeMethodTest()
        {
            ActiveMQConnectionSettings settings = ActiveMQConnectionSettings.CreateSettingsFromConfig();
            ActiveMQConnection connection = new ActiveMQConnection(settings);
            connection.OpenConnection("ClientID");
            connection.Dispose();

            Assert.IsFalse(connection.IsConnected);
        }
    }
}
