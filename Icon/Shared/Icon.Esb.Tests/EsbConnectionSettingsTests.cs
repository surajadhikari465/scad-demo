//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Configuration;
//using System.Security.Cryptography.X509Certificates;
//using TIBCO.EMS;
//using System.Collections.Generic;

//namespace Icon.Esb.Tests
//{
//    [TestClass]
//    public class EsbConnectionSettingsTests
//    {
//        Dictionary<string, string> settingsCopy;

//        [TestInitialize]
//        public void Initialize()
//        {
//            settingsCopy = new Dictionary<string, string>();
//            TestHelpers.CopyOfAppSettings(settingsCopy);
//            TestHelpers.ClearAppSettings(settingsCopy);
//        }

//        [TestCleanup]
//        public void Cleanup()
//        {
//            TestHelpers.SetAppSettings(settingsCopy);
//        }

//        [TestCategory("EsbConnectionSettings")]
//        [TestMethod]
//        public void LoadFromConfig_AllSettingsExistInConfig_LoadsAllPropertiesToSettings()
//        {
//            //Given
//            ConfigurationManager.AppSettings.Set("ServerUrl", "TestServerUrl");
//            ConfigurationManager.AppSettings.Set("JndiUsername", "TestJndiUsername");
//            ConfigurationManager.AppSettings.Set("JndiPassword", "TestJndiPassword");
//            ConfigurationManager.AppSettings.Set("ConnectionFactoryName", "TestConnectionFactoryName");
//            ConfigurationManager.AppSettings.Set("SslPassword", "TestSslPassword");
//            ConfigurationManager.AppSettings.Set("JmsUsername", "TestJmsUsername");
//            ConfigurationManager.AppSettings.Set("JmsPassword", "TestJmsPassword");
//            ConfigurationManager.AppSettings.Set("TargetHostName", "TestTargetHostName");
//            ConfigurationManager.AppSettings.Set("CertificateName", "TestCertificateName");
//            ConfigurationManager.AppSettings.Set("CertificateStoreName", "Root");
//            ConfigurationManager.AppSettings.Set("CertificateStoreLocation", "LocalMachine");
//            ConfigurationManager.AppSettings.Set("ConnAttempts", "111");
//            ConfigurationManager.AppSettings.Set("ConnDelayMilliSeconds", "112");
//            ConfigurationManager.AppSettings.Set("ConnTimeoutMilliSeconds", "113");
//            ConfigurationManager.AppSettings.Set("ReconnAttempts", "114");
//            ConfigurationManager.AppSettings.Set("ReconnDelaySecondsMilliSeconds", "115");
//            ConfigurationManager.AppSettings.Set("ReconnTimeoutMilliSeconds", "116");
//            ConfigurationManager.AppSettings.Set("TestQueueName", "TestEsbConnectionQueue");
//            ConfigurationManager.AppSettings.Set("TestSessionMode", "ExplicitClientDupsOkAcknowledge");

//            //When
//            EsbConnectionSettings settings = new EsbConnectionSettings();
//            settings.LoadFromConfig("TestQueueName", "TestSessionMode");

//            //Then
//            Assert.AreEqual("TestServerUrl", settings.ServerUrl);
//            Assert.AreEqual("TestJndiUsername", settings.JndiUsername);
//            Assert.AreEqual("TestJndiPassword", settings.JndiPassword);
//            Assert.AreEqual("TestConnectionFactoryName", settings.ConnectionFactoryName);
//            Assert.AreEqual("TestSslPassword", settings.SslPassword);
//            Assert.AreEqual("TestJmsUsername", settings.JmsUsername);
//            Assert.AreEqual("TestJmsPassword", settings.JmsPassword);
//            Assert.AreEqual("TestTargetHostName", settings.TargetHostName);
//            Assert.AreEqual("TestCertificateName", settings.CertificateName);
//            Assert.AreEqual(StoreName.Root, settings.CertificateStoreName);
//            Assert.AreEqual(StoreLocation.LocalMachine, settings.CertificateStoreLocation);
//            Assert.AreEqual(111, settings.ConnAttempts);
//            Assert.AreEqual(112, settings.ConnDelay);
//            Assert.AreEqual(113, settings.ConnTimeout);
//            Assert.AreEqual(114, settings.ReconnAttempts);
//            Assert.AreEqual(115, settings.ReconnDelay);
//            Assert.AreEqual(116, settings.ReconnTimeout);
//            Assert.AreEqual("TestEsbConnectionQueue", settings.QueueName);
//            Assert.AreEqual(SessionMode.ExplicitClientDupsOkAcknowledge, settings.SessionMode);
//        }

//        [TestCategory("EsbConnectionSettings")]
//        [TestMethod]
//        public void LoadFromConfig_AllSettingsDontExistInConfig_AllPropertiesAreSetToDefaultValues()
//        {
//            //When
//            EsbConnectionSettings settings = new EsbConnectionSettings();
//            settings.LoadFromConfig("TestQueueName", "TestSessionMode");

//            //Then
//            Assert.AreEqual(String.Empty, settings.ServerUrl);
//            Assert.AreEqual(String.Empty, settings.JndiUsername);
//            Assert.AreEqual(String.Empty, settings.JndiPassword);
//            Assert.AreEqual(String.Empty, settings.ConnectionFactoryName);
//            Assert.AreEqual(String.Empty, settings.SslPassword);
//            Assert.AreEqual(String.Empty, settings.JmsUsername);
//            Assert.AreEqual(String.Empty, settings.JmsPassword);
//            Assert.AreEqual(String.Empty, settings.TargetHostName);
//            Assert.AreEqual(String.Empty, settings.CertificateName);
//            Assert.AreEqual(default(StoreName), settings.CertificateStoreName);
//            Assert.AreEqual(default(StoreLocation), settings.CertificateStoreLocation);
//            Assert.AreEqual(2, settings.ConnAttempts);
//            Assert.AreEqual(500, settings.ConnDelay);
//            Assert.AreEqual(0, settings.ConnTimeout);
//            Assert.AreEqual(4, settings.ReconnAttempts);
//            Assert.AreEqual(500, settings.ReconnDelay);
//            Assert.AreEqual(0, settings.ReconnTimeout);
//            Assert.AreEqual(String.Empty, settings.QueueName);
//            Assert.AreEqual(default(SessionMode), settings.SessionMode);
//        }

//        [TestCategory("EsbConnectionSettings")]
//        [TestMethod]
//        public void LoadFromConfig_QueueNameAndSessionModeAreNull_QueueNameAndSessionModeAreSetToDefaultValues()
//        {
//            //When
//            EsbConnectionSettings settings = new EsbConnectionSettings();
//            settings.LoadFromConfig(null, null);

//            //Then
//            Assert.AreEqual(String.Empty, settings.QueueName);
//            Assert.AreEqual(default(SessionMode), settings.SessionMode);
//        }
//    }
//}
