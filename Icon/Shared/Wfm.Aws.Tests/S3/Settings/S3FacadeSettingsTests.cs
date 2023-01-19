using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wfm.Aws.S3.Settings;

namespace Wfm.Aws.Tests.S3.Settings
{
    [TestClass]
    public class S3FacadeSettingsTests
    {
        [TestMethod]
        public void CreateSettingsFromConfig_ValidTest()
        {
            // When
            S3FacadeSettings settings = S3FacadeSettings.CreateSettingsFromConfig();

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
            S3FacadeSettings settings = S3FacadeSettings.CreateSettingsFromNamedConfig("s3FacadeConfiguration1");

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
            S3FacadeSettings settings = S3FacadeSettings.CreateSettingsFromNamedConfig("s3FacadeConfiguration2");

            // Then
            Assert.IsNotNull(settings);
            Assert.AreEqual(settings.AwsAccessKey, "TestAwsAccessKey2");
            Assert.AreEqual(settings.AwsSecretKey, "TestAwsSecretKey2");
            Assert.AreEqual(settings.AwsRegion, "TestAwsRegion2");
        }
    }
}
