using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Extract.Credentials;

namespace Services.Extract.Tests
{
    [TestClass]
    public class CredentialsTests
    {
        private ISFtpCredentialsCache SftpCredentials;
        private IS3CredentialsCache S3Credentials;

        [TestMethod]
        public void CredentialsCache_LoadFromConfig_SFTP_validCredentialsLoaded()
        {
            SftpCredentials = new SFtpCredentialsCache();
            SftpCredentials.Refresh();
            Assert.IsTrue(SftpCredentials.Credentials.ContainsKey("sftptest"));
            Assert.IsFalse(SftpCredentials.Credentials.ContainsKey("notthere"));

            var testCredentials = SftpCredentials.Credentials["sftptest"];
            Assert.AreEqual("testhost", testCredentials.Host);
            Assert.AreEqual("testuser", testCredentials.Username);
            Assert.AreEqual("testpass", testCredentials.Password);
        }


        [TestMethod]
        public void CredentialsCache_LoadFromConfig_S3_validCredentialsLoaded()
        {
            S3Credentials = new S3CredentialsCache();
            S3Credentials.Refresh();
            Assert.IsTrue(S3Credentials.Credentials.ContainsKey("s3test"));
            Assert.IsFalse(S3Credentials.Credentials.ContainsKey("notthere"));

            var testCredentials = S3Credentials.Credentials["s3test"];
            Assert.AreEqual("testaccesskey", testCredentials.AccessKey);
            Assert.AreEqual("testsecretkey", testCredentials.SecretKey);
            Assert.AreEqual("testbucketname", testCredentials.BucketName);
            Assert.AreEqual("testbucketregion", testCredentials.BucketRegion);
        }
    }
}
