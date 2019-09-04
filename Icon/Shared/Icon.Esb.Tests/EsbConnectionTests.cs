using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography.X509Certificates;
using TIBCO.EMS;
using System.Collections.Generic;

namespace Icon.Esb.Tests
{
    [TestClass]
    public class EsbConnectionTests
    {
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

        // tests below added for finding esb certificates to verify that services will find the correct cert
        // when looking by name. [Ignore] was added so that tests won't fail on build servers if the certs are
        // not installed there but left in for future reference by developers
        [Ignore]
        [TestMethod]
        public void FindEsbCertificateInLocalMachineStore_NonPrd_CertificateCanBeFound()
        {
            //Given
            var expectedSubject = @"E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=""Austin "", S=TX, C=US";
            var expectedSimpleName = "Uday Bhaskar";
            var expectedSerialNumber = "380000E5AF8904145B15102C8200000000E5AF";
            var expectedThumbprint = "FF6D5EF854A2C57EE4D306777BABD00ACF4AE857";
            var expectedSubjectAlternativeNames = @"DNS Name=DEV-ESB-EMS-1.wfm.pvt, DNS Name=TST-ESB-EMS-1.wfm.pvt, DNS Name=TST-ESB-EMS-2.wfm.pvt, DNS Name=TST-ESB-EMS-2.wfm.pvt, DNS Name=QA-ESB-EMS-1.wfm.pvt, DNS Name=QA-ESB-EMS-2.wfm.pvt, DNS Name=DUP-ESB-EMS-1.wfm.pvt, DNS Name=DUP-ESB-EMS-2.wfm.pvt";

            StoreName storeName = StoreName.Root;
            StoreLocation storeLocation = StoreLocation.LocalMachine;

            //When
            var store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, expectedSubject, true)[0];
            store.Close();

            //Then
            Assert.IsNotNull(cert);
            Assert.AreEqual(expectedSubject, cert.Subject);
            Assert.AreEqual(expectedSimpleName, cert.GetNameInfo(X509NameType.SimpleName, false));
            Assert.AreEqual(expectedSerialNumber, cert.SerialNumber);
            Assert.AreEqual(expectedThumbprint, cert.Thumbprint);
            Assert.AreEqual(expectedSubjectAlternativeNames, GetCertSubjectAlternativeNames(cert));
        }

        [Ignore]
        [TestMethod]
        public void FindEsbCertificateInLocalMachineStore_Perf_CertificateCanBeFound()
        {
            //Given
            var expectedSubject = @"E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US";
            var expectedSimpleName = "uday.bhaskar";
            var expectedSerialNumber = "380000E7EB0599E8BE4DC4C51200000000E7EB";
            var expectedThumbprint = "778F07EE3C7FE7C161AE04F13A6A27E980449EDE";
            var expectedSubjectAlternativeNames = @"DNS Name=PERF-ESB-EMS-1.wfm.pvt, DNS Name=PERF-ESB-EMS-2.wfm.pvt, DNS Name=PERF-ESB-EMS-3.wfm.pvt, DNS Name=PERF-ESB-EMS-4.wfm.pvt";

            StoreName storeName = StoreName.Root;
            StoreLocation storeLocation = StoreLocation.LocalMachine;

            //When
            var store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, expectedSubject, true)[0];
            store.Close();

            //Then
            Assert.IsNotNull(cert);
            Assert.AreEqual(expectedSubject, cert.Subject);
            Assert.AreEqual(expectedSimpleName, cert.GetNameInfo(X509NameType.SimpleName, false));
            Assert.AreEqual(expectedSerialNumber, cert.SerialNumber);
            Assert.AreEqual(expectedThumbprint, cert.Thumbprint);
            Assert.AreEqual(expectedSubjectAlternativeNames, GetCertSubjectAlternativeNames(cert));
        }

        [Ignore]
        [TestMethod]
        public void FindEsbCertificateInLocalMachineStore_Prod_CertificateCanBeFound()
        {
            //Given
            var expectedSubject = @"E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US";
            var expectedSimpleName = "Uday Bhaskar";
            var expectedSerialNumber = "380000F0EBED183CF80FB0CAB900000000F0EB";
            var expectedThumbprint = "740ED118FFA82B281CE63B0A1FE94DEA93390A80";
            var expectedSubjectAlternativeNames = @"DNS Name=PROD-ESB-EMS-1.wfm.pvt, DNS Name=PROD-ESB-EMS-2.wfm.pvt, DNS Name=PROD-ESB-EMS-3.wfm.pvt, DNS Name=PROD-ESB-EMS-4.wfm.pvt";
            StoreName storeName = StoreName.Root;
            StoreLocation storeLocation = StoreLocation.LocalMachine;

            //When
            var store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            //var allCerts = GetInstalledCertSubjects(store);
            //var candidates = GetInstalledCertSubjects(store, "Uday");
            var cert = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, expectedSubject, true)[0];
            store.Close();

            //Then
            Assert.IsNotNull(cert);
            Assert.AreEqual(expectedSubject, cert.Subject);
            Assert.AreEqual(expectedSimpleName, cert.GetNameInfo(X509NameType.SimpleName, false));
            Assert.AreEqual(expectedSerialNumber, cert.SerialNumber);
            Assert.AreEqual(expectedThumbprint, cert.Thumbprint);
            Assert.AreEqual(expectedSubjectAlternativeNames, GetCertSubjectAlternativeNames(cert));
        }

        [Ignore]
        [TestMethod]
        public void FindEsbCertificateInLocalMachineStore_TestDup_CertificateCanBeFound()
        {
            //Given
            var expectedSubject = @"E=suchetha.aleti@wholefoods.com, CN=cerd1617.wfm.pvt, OU=InfraESBAdmins@wholefoods.com, O=""Whole Foods Market "", L=Austin, S=TX, C=US";
            var expectedSimpleName = "cerd1617.wfm.pvt";
            var expectedSerialNumber = "380000DDF80B9DAC3DD2E8225F00000000DDF8";
            var expectedThumbprint = "29142FE82C0E2EB00A58B4593F534534FD018E2F";
            var expectedSubjectAlternativeNames = @"DNS Name=cerd1617.wfm.pvt";

            StoreName storeName = StoreName.Root;
            StoreLocation storeLocation = StoreLocation.LocalMachine;

            //When
            var store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, expectedSubject, true)[0];
            store.Close();

            //Then
            Assert.IsNotNull(cert);
            Assert.AreEqual(expectedSubject, cert.Subject);
            Assert.AreEqual(expectedSimpleName, cert.GetNameInfo(X509NameType.SimpleName, false));
            Assert.AreEqual(expectedSerialNumber, cert.SerialNumber);
            Assert.AreEqual(expectedThumbprint, cert.Thumbprint);
            Assert.AreEqual(expectedSubjectAlternativeNames, GetCertSubjectAlternativeNames(cert));
        }

        private string GetCertSubjectAlternativeNames(X509Certificate2 cert)
        {
            var sanNames = string.Empty;
            foreach (var ext in cert.Extensions)
            {
                if (ext.Oid.Value == "2.5.29.17")
                {
                    sanNames = ext.Format(false);
                }
            }
            return sanNames;
        }

        // useful if you're trying to figure out the subject name to use when a new cert is issued
        private string GetInstalledCertSubjects(X509Store openStore, string containedSubString = null)
        {
            var allCertSubjects = new List<string>();
            foreach (var cert in openStore.Certificates)
            {
                if (string.IsNullOrEmpty(containedSubString)
                    || (!string.IsNullOrEmpty(containedSubString)
                        && cert.Subject
                            .IndexOf(containedSubString, StringComparison.CurrentCultureIgnoreCase) >= 0))
                {
                    allCertSubjects.Add(cert.Subject);
                }
            }
            return string.Join(Environment.NewLine, allCertSubjects.ToArray());
        }
    }
}
