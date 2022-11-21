using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Icon.Dvs.Tests
{
    [TestClass]
    public class DvsListenerSettingsTests
    {
        [TestMethod]
        public void CreateSettingsFromConfig_Test()
        {
            // When
            var settings = DvsListenerSettings.CreateSettingsFromConfig();

            // Then
            Assert.IsNotNull(settings);
            Assert.AreEqual(settings.AwsAccessKey, "TestAwsAccessKey");
            Assert.AreEqual(settings.AwsSecretKey, "TestAwsSecretKey");
            Assert.AreEqual(settings.ListenerApplicationName, "TestListenerApplicationName");
            Assert.AreEqual(settings.Region, "TestRegion");
            Assert.AreEqual(settings.SqsQueueUrl, "TestSqsQueueUrl");
            Assert.AreEqual(settings.SqsTimeout, 3);
            Assert.AreEqual(settings.PollInterval, 10000);
        }
    }
}
