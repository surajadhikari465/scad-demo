using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wfm.Aws.SQS.Settings;

namespace Wfm.Aws.Tests.SQS.Settings
{
    [TestClass]
    public class SQSFacadeSettingsTests
    {
        [TestMethod]
        public void CreateSettingsFromConfig_ValidTest()
        {
            // When
            SQSFacadeSettings settings = SQSFacadeSettings.CreateSettingsFromConfig();

            // Then
            Assert.IsNotNull(settings);
            Assert.AreEqual(settings.AwsAccessKey, "TestAwsAccessKey");
            Assert.AreEqual(settings.AwsSecretKey, "TestAwsSecretKey");
            Assert.AreEqual(settings.AwsRegion, "TestAwsRegion");
        }

        [TestMethod]
        public void CreateSettingsFromNamedConfig_ValidTest1()
        {
            // When
            SQSFacadeSettings settings = SQSFacadeSettings.CreateSettingsFromNamedConfig("sqsFacadeConfiguration1");

            // Then
            Assert.IsNotNull(settings);
            Assert.AreEqual(settings.AwsAccessKey, "TestAwsAccessKey1");
            Assert.AreEqual(settings.AwsSecretKey, "TestAwsSecretKey1");
            Assert.AreEqual(settings.AwsRegion, "TestAwsRegion1");
        }

        [TestMethod]
        public void CreateSettingsFromNamedConfig_ValidTest2()
        {
            // When
            SQSFacadeSettings settings = SQSFacadeSettings.CreateSettingsFromNamedConfig("sqsFacadeConfiguration2");

            // Then
            Assert.IsNotNull(settings);
            Assert.AreEqual(settings.AwsAccessKey, "TestAwsAccessKey2");
            Assert.AreEqual(settings.AwsSecretKey, "TestAwsSecretKey2");
            Assert.AreEqual(settings.AwsRegion, "TestAwsRegion2");
        }
    }
}
