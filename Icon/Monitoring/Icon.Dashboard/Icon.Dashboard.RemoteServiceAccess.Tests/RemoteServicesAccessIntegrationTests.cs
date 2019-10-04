using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceProcess;
using System.Management;
using Moq;

namespace Icon.Dashboard.RemoteServicesAccess.Tests
{
    [TestClass]
    public class RemoteServiceAccessIntegrationTests
    {
        [TestMethod]
        public void LoadRemoteService_ForVmIconTest1Glocon_ReturnsExpectedRemoteServiceModel()
        {
            // Arrange            
            var host = "vm-icon-test1";
            var application = "GlobalEventControllerService";
            var wmiService = new RemoteWmiAccessService();

            // Act
            var actualServiceModel = wmiService.LoadRemoteService(host, application);

            // Assert
            Assert.AreEqual("CEWD6592", actualServiceModel.SystemName);
            Assert.AreEqual("GlobalEventControllerService", actualServiceModel.FullName);
            Assert.AreEqual("Icon Global Event Controller Service", actualServiceModel.DisplayName);
            Assert.AreEqual("Processes Global Event Controller from IRMA to Icon.", actualServiceModel.Description);
            Assert.IsTrue(actualServiceModel.ProcessId > 0);
            Assert.AreEqual("Auto", actualServiceModel.StartMode);
            Assert.AreEqual(@"wfm\IconInterfaceUserTes", actualServiceModel.RunningAs);
            Assert.AreEqual("Running", actualServiceModel.State);
            Assert.AreEqual(@"""E:\Icon\Global Controller\GlobalEventController.Controller.exe""  -displayname ""Icon Global Event Controller Service"" -servicename ""GlobalEventControllerService""",
                actualServiceModel.ConfigFilePath);
            Assert.AreEqual("WindowsService", actualServiceModel.TypeOfApplication);
        }

        [TestMethod]
        public void LoadRemoteServices_ForVmIconDev1_ReturnsListOfPopulatedServiceModels()
        {
            // Arrange            
            var host = "vm-icon-dev1";
            var wmiService = new RemoteWmiAccessService();

            // Act
            var actualServiceModels = wmiService.LoadRemoteServices(host);

            // Assert
            Assert.IsTrue(actualServiceModels.Count > 12); // rough minimum
            foreach (var actualServiceModel in actualServiceModels)
            {
                // check that some basic properties were populated
                Assert.IsNotNull(actualServiceModel.SystemName);
                Assert.IsNotNull(actualServiceModel.FullName);
                Assert.IsNotNull(actualServiceModel.DisplayName);
                Assert.IsNotNull(actualServiceModel.RunningAs);
                Assert.IsNotNull(actualServiceModel.State);
                Assert.IsNotNull(actualServiceModel.ConfigFilePath);
            }
        }

        [TestMethod]
        public void LoadRemoteServices_ForVmIconDev1_ReturnsListOfPopulatedServiceModelsIncludingGlobalController()
        {
            // Arrange            
            var host = "vm-icon-dev1";
            var wmiService = new RemoteWmiAccessService();

            // Act
            var actualServiceModels = wmiService.LoadRemoteServices(host);

            // Assert
            // make sure one of the services we got was GloCon
            var pushControllerServiceModel = actualServiceModels.FirstOrDefault(s => s.FullName == "GlobalEventControllerService");
            Assert.IsNotNull(pushControllerServiceModel);
        }

        [TestMethod]
        public void LoadRemoteServices_ForVmIconDev1_ReturnsListOfPopulatedServiceModelsIncludingPushController()
        {
            // Arrange            
            var host = "vm-icon-dev1";
            var wmiService = new RemoteWmiAccessService();

            // Act
            var actualServiceModels = wmiService.LoadRemoteServices(host);

            // Assert
            // the POS Push Controller service does not have "Icon" in its name, so make sure our query is getting this one
            var pushControllerServiceModel = actualServiceModels.FirstOrDefault(s => s.FullName == "PushController");
            Assert.IsNotNull(pushControllerServiceModel);
        }

        [TestMethod]
        public void LoadRemoteServices_ForVmIconDev1_ReturnsListOfPopulatedServiceModelsIncludingEwicListener()
        {
            // Arrange            
            var host = "vm-icon-dev1";
            var wmiService = new RemoteWmiAccessService();

            // Act
            var actualServiceModels = wmiService.LoadRemoteServices(host);

            // Assert
            // the eWic listener service does not have "Icon" in its name, so make sure our query is getting this one
            var pushControllerServiceModel = actualServiceModels.FirstOrDefault(s => s.FullName == "eWIC APL Listener");
            Assert.IsNotNull(pushControllerServiceModel);
        }
    }
}
