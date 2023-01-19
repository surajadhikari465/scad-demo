using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wfm.Aws.SNS.Settings;

namespace Wfm.Aws.Tests.SNS.Settings
{
    [TestClass]
    public class SNSFacadeSettingsTests
    {
        [TestMethod]
        public void CreateSettingsFromConfig_ValidTest()
        {
            // When
            SNSFacadeSettings settings = SNSFacadeSettings.CreateSettingsFromConfig();

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
            SNSFacadeSettings settings = SNSFacadeSettings.CreateSettingsFromNamedConfig("snsFacadeConfiguration1");

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
            SNSFacadeSettings settings = SNSFacadeSettings.CreateSettingsFromNamedConfig("snsFacadeConfiguration2");

            // Then
            Assert.IsNotNull(settings);
            Assert.AreEqual(settings.AwsAccessKey, "TestAwsAccessKey2");
            Assert.AreEqual(settings.AwsSecretKey, "TestAwsSecretKey2");
            Assert.AreEqual(settings.AwsRegion, "TestAwsRegion2");
        }
    }
}
