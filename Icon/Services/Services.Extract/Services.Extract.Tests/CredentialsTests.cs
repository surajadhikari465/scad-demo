using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Extract.Credentials;

namespace Services.Extract.Tests
{

    [TestClass]
    public class FileDestinationTests
    {
        private IFileDestinationCache FileDestinations;

        [TestMethod]
        public void FileDestinations_LoadFromonfig_DestinationsLoaded()
        {
            FileDestinations = new FileDestinationsCache();
            FileDestinations.Refresh();
            Assert.IsTrue(FileDestinations.FileDestinations.ContainsKey("CAP"));
            Assert.IsFalse(FileDestinations.FileDestinations.ContainsKey("notthere"));

            var fileDestination = FileDestinations.FileDestinations["CAP"];
            Assert.AreEqual("c:\\temp\\", fileDestination.Path);
        }
    }

    [TestClass]
    public class CredentialsTests
    {
        private ISFtpCredentialsCache SftpCredentials;
        private IS3CredentialsCache S3Credentials;
        private IActiveMqCredentialsCache ActiveMqCredentials;

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

        [TestMethod]
        public void CredentialsCache_LoadFromCache_ActiveMq_validCredentialsLoaded()
        {
            ActiveMqCredentials = new ActiveMqCredentialCache();
            ActiveMqCredentials.Refresh();
            Assert.IsTrue(ActiveMqCredentials.Credentials.ContainsKey("inStock"));

            var testCredentials = ActiveMqCredentials.Credentials["inStock"];
            Assert.AreEqual("failover:(ssl://b-30e2193f-fe4b-4212-831e-58ef6e0e2784-1.mq.us-west-2.amazonaws.com:61617,ssl://b-ebf4b796-b53d-4480-960c-caa3e8e06722-1.mq.us-west-2.amazonaws.com:61617)?randomize=true", testCredentials.ServerUrl);
            Assert.AreEqual("wfmdatavayuactivemqv2producer", testCredentials.JmsUsername);
            Assert.AreEqual("f16WTI8Uqbh3CCIq", testCredentials.JmsPassword);
            Assert.AreEqual("ItemVendorTestQueue", testCredentials.QueueName);
            Assert.AreEqual("AutoAcknowledge", testCredentials.SessionMode);
            Assert.AreEqual("ByteMessage", testCredentials.MessageType);
            Assert.AreEqual("ItemVendorLane", testCredentials.TransactionType);
            Assert.AreEqual("{STORE_NUMBER}_{date:yyyyMMdd}.F.2", testCredentials.TransactionId);
        }
    }
}
