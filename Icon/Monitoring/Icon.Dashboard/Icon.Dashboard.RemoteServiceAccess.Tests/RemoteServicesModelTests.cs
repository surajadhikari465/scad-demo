using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceProcess;
using Moq;
using System.Management;
using System.Net;

namespace Icon.Dashboard.RemoteServicesAccess.Tests
{

    [TestClass]
    public class RemoteServiceModelTests
    {
        [TestMethod]
        public void RemoteServiceModelConstructor_ForVmIconTest1Glocon_PopulatesExpectedProperties()
        {
            // Arrange
            // find a remote icon service to test with
            var host = "vm-icon-test1";
            var application = "GlobalEventControllerService";         
            string providerPath = $@"\\{host}\root\cimv2";
            string wqlQuery = $"SELECT * FROM Win32_Service WHERE Name = '{application}'";
           
            // Act
            ObjectQuery objectQuery = new ObjectQuery(wqlQuery);
            ConnectionOptions options = new ConnectionOptions();
            ManagementScope mgmtScope = new ManagementScope(providerPath, options);
            mgmtScope.Connect();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(mgmtScope, objectQuery);
            ManagementObjectCollection retObjectCollection = searcher.Get();

            // Assert
            Assert.AreEqual(1, retObjectCollection.Count);
            foreach (ManagementObject mgmtObj in retObjectCollection)
            {
                var remoteServiceModel = new RemoteServiceModel(mgmtObj);
                Assert.AreEqual("WindowsService", remoteServiceModel.TypeOfApplication);
                Assert.AreEqual("CEWD6592", remoteServiceModel.SystemName);
                Assert.AreEqual("GlobalEventControllerService", remoteServiceModel.FullName);        
                Assert.AreEqual("Icon Global Event Controller Service", remoteServiceModel.DisplayName);        
                Assert.AreEqual("Processes Global Event Controller from IRMA to Icon.", remoteServiceModel.Description);        
                Assert.IsTrue(remoteServiceModel.ProcessId>0);        
                Assert.AreEqual("Auto", remoteServiceModel.StartMode);        
                Assert.AreEqual(@"wfm\IconInterfaceUserTes", remoteServiceModel.RunningAs);        
                Assert.AreEqual("Running", remoteServiceModel.State);        
                Assert.AreEqual(@"""E:\Icon\Global Controller\GlobalEventController.Controller.exe""  -displayname ""Icon Global Event Controller Service"" -servicename ""GlobalEventControllerService""",
                    remoteServiceModel.ConfigFilePath);
            }
                   
        }
    }
}
