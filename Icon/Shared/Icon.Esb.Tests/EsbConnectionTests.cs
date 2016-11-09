//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Security.Cryptography.X509Certificates;
//using TIBCO.EMS;

//namespace Icon.Esb.Tests
//{
//    [TestClass]
//    public class EsbConnectionTests
//    {
//        [TestMethod]
//        public void Constructor_EsbConnectionSettingsHasValuesOfSettingsParameter()
//        {
//            //Given
//            EsbConnectionSettings settings = new EsbConnectionSettings
//            {
//                CertificateName = "Test CertificateName",
//                CertificateStoreLocation = StoreLocation.LocalMachine,
//                CertificateStoreName = StoreName.Root,
//                ConnAttempts = 5,
//                ConnDelay = 10,
//                ConnectionFactoryName = "Test ConnectionFactoryName",
//                ConnTimeout = 55,
//                JmsPassword = "JmsPassword",
//                JmsUsername = "JmsUsername",
//                JndiPassword = "JndiPassword",
//                JndiUsername = "JndiUsername",
//                QueueName = "QueueName",
//                ReconnAttempts = 56,
//                ReconnDelay = 57,
//                ReconnTimeout = 58,
//                ServerUrl = "ServerUtl",
//                SessionMode = SessionMode.ExplicitClientDupsOkAcknowledge,
//                SslPassword = "SslPassword",
//                TargetHostName = "TargetHostName"
//            };

//            //When
//            EsbConnection connection = new EsbConnection(settings);

//            //Then
//            Assert.AreEqual(settings.CertificateName, connection.Settings.CertificateName);
//            Assert.AreEqual(settings.CertificateStoreLocation, connection.Settings.CertificateStoreLocation);
//            Assert.AreEqual(settings.CertificateStoreName, connection.Settings.CertificateStoreName);
//            Assert.AreEqual(settings.ConnAttempts, connection.Settings.ConnAttempts);
//            Assert.AreEqual(settings.ConnDelay, connection.Settings.ConnDelay);
//            Assert.AreEqual(settings.ConnectionFactoryName, connection.Settings.ConnectionFactoryName);
//            Assert.AreEqual(settings.ConnTimeout, connection.Settings.ConnTimeout);
//            Assert.AreEqual(settings.JmsPassword, connection.Settings.JmsPassword);
//            Assert.AreEqual(settings.JmsUsername, connection.Settings.JmsUsername);
//            Assert.AreEqual(settings.JndiPassword, connection.Settings.JndiPassword);
//            Assert.AreEqual(settings.JndiUsername, connection.Settings.JndiUsername);
//            Assert.AreEqual(settings.QueueName, connection.Settings.QueueName);
//            Assert.AreEqual(settings.ReconnAttempts, connection.Settings.ReconnAttempts);
//            Assert.AreEqual(settings.ReconnDelay, connection.Settings.ReconnDelay);
//            Assert.AreEqual(settings.ReconnTimeout, connection.Settings.ReconnTimeout);
//            Assert.AreEqual(settings.ServerUrl, connection.Settings.ServerUrl);
//            Assert.AreEqual(settings.SessionMode, connection.Settings.SessionMode);
//            Assert.AreEqual(settings.SslPassword, connection.Settings.SslPassword);
//            Assert.AreEqual(settings.TargetHostName, connection.Settings.TargetHostName);
//        }

//        [TestMethod]
//        public void IsConnected_OpenConnectionHasNotBeenCalled_ReturnsFalse()
//        {
//            //Given
//            EsbConnection connection = new EsbConnection(new EsbConnectionSettings());

//            //When
//            bool isConnected = connection.IsConnected;

//            Assert.IsFalse(isConnected);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(NullReferenceException))]
//        public void OpenConnection_SettingsAreIncomplete_ThrowsException()
//        {
//            //Given
//            EsbConnection connection = new EsbConnection(new EsbConnectionSettings());

//            //When
//            connection.OpenConnection();
//        }
//    }
//}
