﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Extract.Credentials;

namespace Services.Extract.Tests
{
    [TestClass]
    public class CredentialsTests
    {
        private ISFtpCredentialsCache SftpCredentials;
        private IS3CredentialsCache S3Credentials;
        private IEsbCredentialsCache EsbCredentials;

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
        public void CredentialsCache_LoadFromConfig_Esb_validCredentialsLoaded()
        {
            EsbCredentials = new EsbCredentialsCache();
            EsbCredentials.Refresh();
            Assert.IsTrue(EsbCredentials.Credentials.ContainsKey("inStock"));

            var testCredentials = EsbCredentials.Credentials["inStock"];
            Assert.AreEqual("ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293", testCredentials.ServerUrl);
            Assert.AreEqual("iconUser", testCredentials.JmsUsername);
            Assert.AreEqual("Pjetuc9M7Kmi", testCredentials.JmsPassword);
            Assert.AreEqual("jndiIconUser", testCredentials.JndiUsername);
            Assert.AreEqual("jndiIconUser", testCredentials.JndiPassword);
            Assert.AreEqual("esb", testCredentials.SslPassword);
            Assert.AreEqual("WFMESB.SupplyChain.Retail.InventoryMgmt.Info.Topic.V2", testCredentials.QueueName);
            Assert.AreEqual("ClientAcknowledge", testCredentials.SessionMode);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US", testCredentials.CertificateName);
            Assert.AreEqual("Root", testCredentials.CertificateStoreName);
            Assert.AreEqual("LocalMachine", testCredentials.CertificateStoreLocation);
            Assert.AreEqual("TST-ESB-EMS-1.wfm.pvt", testCredentials.TargetHostName);
            Assert.AreEqual("OrderTopicConnectionFactory", testCredentials.ConnectionFactoryName);
            Assert.AreEqual("Topic", testCredentials.DestinationType);
            Assert.AreEqual("ByteMessage", testCredentials.MessageType);
            Assert.AreEqual("ItemVendorLane", testCredentials.TransactionType);
            Assert.AreEqual("{STORE_NUMBER}_{date:yyyyMMdd}.F.2", testCredentials.TransactionId);
        }
    }
}
