using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.RemoteServicesAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ServiceTests
{
    [TestClass]
    public class RemoteWmiServiceWrapperUnitTests
    {
        TestData testData = new TestData();

        Mock<IRemoteWmiAccessService> mockWMiSvc = new Mock<IRemoteWmiAccessService>();
        Mock<IIconDatabaseServiceWrapper> mockIconDbService = new Mock<IIconDatabaseServiceWrapper>();
        Mock<IMammothDatabaseServiceWrapper> mockMammothDbService = new Mock<IMammothDatabaseServiceWrapper>();
        Mock<IEsbEnvironmentManager> mockEsbEnvironmentManager = new Mock<IEsbEnvironmentManager>();


        [TestMethod]
        public void CreateViewModel_WhenReturnsGloconServiceObject_PopulatesViewModelWithExpectedProperties()
        {
            // Arrange
            string server = "vm-test1";
            string application = "GlobalEventControllerService";
            bool commandsEnabled = true;

            mockIconDbService.Setup(s => s.GetApps()).Returns(testData.IconApps);
            mockMammothDbService.Setup(s => s.GetApps()).Returns(testData.MammothApps);
            mockEsbEnvironmentManager.Setup(s => s.GetEsbEnvironmentDefinitions()).Returns(testData.EsbEnvironments);
            mockWMiSvc.Setup(s => s.LoadRemoteService(server, application)) .Returns(testData.SampleGloconService);

            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var actualViewModel = wmiServiceWrapper.CreateViewModel(server, testData.SampleGloconService, commandsEnabled);

            // Assert
            Assert.AreEqual(testData.SampleGloconService.FullName, actualViewModel.Name);
            Assert.AreEqual(testData.SampleGloconService.DisplayName, actualViewModel.DisplayName);
            Assert.AreEqual(server, actualViewModel.Server);
            Assert.AreEqual(@"SampleAppConfig_A.exe.config", actualViewModel.ConfigFilePath);
            Assert.AreEqual(commandsEnabled, actualViewModel.CommandsEnabled);
            Assert.AreEqual(testData.SampleGloconService.State, actualViewModel.Status);
            Assert.AreEqual(1, actualViewModel.ValidCommands.Count);
            Assert.AreEqual("Stop", actualViewModel.ValidCommands[0]);
            Assert.AreEqual(true, actualViewModel.StatusIsGreen);
            Assert.AreEqual(testData.SampleGloconService.SystemName, actualViewModel.HostName);
            Assert.AreEqual("IconTestUserDev", actualViewModel.AccountName);
            //from sample config file
            Assert.AreEqual(true, actualViewModel.ConfigFilePathIsValid);
            Assert.AreEqual(48, actualViewModel.AppSettings.Count);
            Assert.AreEqual(0, actualViewModel.EsbConnectionSettings.Count);
            Assert.AreEqual(7, actualViewModel.LoggingID);
            Assert.AreEqual("Global Controller", actualViewModel.LoggingName);
        }


        [TestMethod]
        public void GetConfigUncPath_WhenReturnsApiControllerRemoteServiceObject_PopulatesViewModelWithExpectedProperties()
        {
            // Arrange
            string server = "vm-test1";
            string pathName = @"""E:\Icon\API Controller Phase 2\Hierarchy\Icon.ApiController.Controller.exe""  -displayname ""Icon API Controller - Hierarchy"" -servicename ""IconAPIController-Hierarchy""";
            string expectedUncPath = @"\\vm-test1\E$\Icon\API Controller Phase 2\Hierarchy\Icon.ApiController.Controller.exe.config";
            
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var uncPath = wmiServiceWrapper.GetConfigUncPath(server, pathName);

            // Assert
            Assert.AreEqual(expectedUncPath, uncPath);
        }
    }
}
