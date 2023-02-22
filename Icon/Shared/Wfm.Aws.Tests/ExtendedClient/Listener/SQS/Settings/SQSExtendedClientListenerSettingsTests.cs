using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wfm.Aws.ExtendedClient.Listener.SQS.Settings;
using Wfm.Aws.SQS.Settings;

namespace Wfm.Aws.Tests.ExtendedClient.Listener.SQS.Settings
{
    [TestClass]
    public class SQSExtendedClientListenerSettingsTests
    {
        [TestMethod]
        public void CreateSettingsFromConfig_ValidTest()
        {
            // When
            SQSExtendedClientListenerSettings settings = SQSExtendedClientListenerSettings.CreateSettingsFromConfig();

            // Then
            Assert.IsNotNull(settings);
            Assert.AreEqual("TestSQSListenerApplicationName", settings.SQSListenerApplicationName);
            Assert.AreEqual("TestSQSListenerQueueUrl", settings.SQSListenerQueueUrl);
            Assert.AreEqual(30, settings.SQSListenerPollIntervalInSeconds);
            Assert.AreEqual(5, settings.SQSListenerTimeoutInSeconds);
            Assert.AreEqual(20, settings.SQSListenerSafeStopCheckInSeconds);
            Assert.AreEqual(false, settings.SQSListenerSafeStopCheckEnabled);
        }

        [TestMethod]
        public void CreateSettingsFromNamedConfig_ValidTest1()
        {
            // When
            SQSExtendedClientListenerSettings settings = SQSExtendedClientListenerSettings.CreateSettingsFromNamedConfig("sqsExtendedClientListenerConfiguration1");

            // Then
            Assert.IsNotNull(settings);
            Assert.AreEqual("TestSQSListenerApplicationName1", settings.SQSListenerApplicationName);
            Assert.AreEqual("TestSQSListenerQueueUrl1", settings.SQSListenerQueueUrl);
            Assert.AreEqual(30, settings.SQSListenerPollIntervalInSeconds);
            Assert.AreEqual(15, settings.SQSListenerTimeoutInSeconds);
            Assert.AreEqual(15, settings.SQSListenerSafeStopCheckInSeconds);
            Assert.AreEqual(false, settings.SQSListenerSafeStopCheckEnabled);
        }

        [TestMethod]
        public void CreateSettingsFromNamedConfig_ValidTest2()
        {
            // When
            SQSExtendedClientListenerSettings settings = SQSExtendedClientListenerSettings.CreateSettingsFromNamedConfig("sqsExtendedClientListenerConfiguration2");

            // Then
            Assert.IsNotNull(settings);
            Assert.AreEqual("TestSQSListenerApplicationName2", settings.SQSListenerApplicationName);
            Assert.AreEqual("TestSQSListenerQueueUrl2", settings.SQSListenerQueueUrl);
            Assert.AreEqual(15, settings.SQSListenerPollIntervalInSeconds);
            Assert.AreEqual(5, settings.SQSListenerTimeoutInSeconds);
            Assert.AreEqual(20, settings.SQSListenerSafeStopCheckInSeconds);
            Assert.AreEqual(true, settings.SQSListenerSafeStopCheckEnabled);
        }
    }
}
