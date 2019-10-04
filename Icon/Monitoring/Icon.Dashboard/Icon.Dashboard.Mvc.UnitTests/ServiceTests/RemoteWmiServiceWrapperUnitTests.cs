using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.UnitTests.TestData;
using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.RemoteServicesAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ServiceTests
{
    [TestClass]
    public class RemoteWmiServiceWrapperUnitTests
    {
        RemoteServiceTestData serviceTestData = new RemoteServiceTestData();
        DbWrapperTestData dbWrapperTestData = new DbWrapperTestData();
        ConfigTestData configTestData = new ConfigTestData();
        Mock<IRemoteWmiAccessService> mockWMiSvc = new Mock<IRemoteWmiAccessService>();
        string testDataFolder = @"TestData";

        [TestMethod]
        public void LoadRemoteService_WhenReturnsGloconServiceObject_PopulatesExpectedProperties()
        {
            // Arrange
            string server = "vm-test1";
            string application = "GlobalEventControllerService";
            bool commandsEnabled = true;
            var configTestData = new ConfigTestData();
            var iconApps = dbWrapperTestData.IconApps;
            var mammothApps = dbWrapperTestData.MammothApps;
            var environments = configTestData.Models.EnvironmentsList;
            var esbEnvironments = configTestData.Models.EsbEnvironmentsList;
            var fakeService = dbWrapperTestData.SampleGloconService;
            mockWMiSvc.Setup(s => s.LoadRemoteService(server, application)).Returns(fakeService);
            var serviceConfigReader = new ExternalConfigXmlManager(fakeService.ConfigFilePath);

            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);

            // Act
            var actualViewModel = wmiServiceWrapper.LoadRemoteService(
                server,
                application,
                commandsEnabled,
                iconApps,
                mammothApps,
                environments,
                esbEnvironments,
                serviceConfigReader);

            // Assert
            Assert.AreEqual(dbWrapperTestData.SampleGloconService.FullName, actualViewModel.Name);
            Assert.AreEqual(dbWrapperTestData.SampleGloconService.DisplayName, actualViewModel.DisplayName);
            Assert.AreEqual(server, actualViewModel.Server);
            Assert.AreEqual($"{testDataFolder}\\SampleAppConfig_NoEsb_GloCon.exe.config", actualViewModel.ConfigFilePath);
            Assert.AreEqual(commandsEnabled, actualViewModel.CommandsEnabled);
            Assert.AreEqual(dbWrapperTestData.SampleGloconService.State, actualViewModel.Status);
            Assert.AreEqual(1, actualViewModel.ValidCommands.Count);
            Assert.AreEqual("Stop", actualViewModel.ValidCommands[0]);
            Assert.AreEqual(true, actualViewModel.StatusIsGreen);
            Assert.AreEqual(dbWrapperTestData.SampleGloconService.SystemName, actualViewModel.HostName);
            Assert.AreEqual("IconTestUserDev", actualViewModel.AccountName);
            //from sample config file
            Assert.AreEqual(true, actualViewModel.ConfigFilePathIsValid);
            Assert.AreEqual(48, actualViewModel.AppSettings.Count);
            Assert.AreEqual(0, actualViewModel.EsbConnections.Count);
            Assert.AreEqual(7, actualViewModel.LoggingID);
            Assert.AreEqual("Global Controller", actualViewModel.LoggingName);
        }

        [TestMethod]
        public void CreateServiceViewModel_WhenReturnsMammothServiceObject_PopulatesExpectedProperties()
        {
            // Arrange
            string server = "vm-test1";
            bool commandsEnabled = true;
            var configTestData = new ConfigTestData();
            var iconApps = dbWrapperTestData.IconApps;
            var mammothApps = dbWrapperTestData.MammothApps;
            var environments = configTestData.Models.EnvironmentsList;
            var esbEnvironments = configTestData.Models.EsbEnvironmentsList;
            var fakeService = dbWrapperTestData.SampleMammothItemLocaleControllerMAService;
            string appConfigPath = $"TestData\\SampleAppConfig_NoEsb_MammothItemLocale.exe.config";
            Assert.IsTrue(File.Exists(appConfigPath));
            fakeService.ConfigFilePath = appConfigPath;
            //mockWMiSvc.Setup(s => s.LoadRemoteService(server, application)).Returns(fakeService);
            //var serviceConfigReader = new ExternalConfigXmlManager(fakeService.ConfigFilePath);
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);

            // Act
            var actualViewModel = wmiServiceWrapper.CreateServiceViewModel(
                server,
                fakeService,
                commandsEnabled,
                iconApps,
                mammothApps,
                environments,
                esbEnvironments);
                //serviceConfigReader);

            // Assert 
            Assert.AreEqual(fakeService.FullName, actualViewModel.Name);
            Assert.AreEqual(fakeService.DisplayName, actualViewModel.DisplayName);
            Assert.AreEqual(server, actualViewModel.Server);
            Assert.AreEqual(appConfigPath, actualViewModel.ConfigFilePath);
            Assert.AreEqual(commandsEnabled, actualViewModel.CommandsEnabled);
            Assert.AreEqual(fakeService.State, actualViewModel.Status);
            Assert.AreEqual(1, actualViewModel.ValidCommands.Count);
            Assert.AreEqual("Stop", actualViewModel.ValidCommands[0]);
            Assert.AreEqual(true, actualViewModel.StatusIsGreen);
            Assert.AreEqual(fakeService.SystemName, actualViewModel.HostName);
            Assert.AreEqual("MammothTest", actualViewModel.AccountName);
            //from sample config file
            Assert.AreEqual(true, actualViewModel.ConfigFilePathIsValid);
            Assert.AreEqual(14, actualViewModel.AppSettings.Count);
            Assert.AreEqual(0, actualViewModel.EsbConnections.Count);
            Assert.AreEqual(2, actualViewModel.LoggingID);
            Assert.AreEqual("ItemLocale Controller", actualViewModel.LoggingName);

        }

        [TestMethod]
        public void GetConfigUncPath_WhenReturnsApiControllerRemoteServiceObject_PopulatesViewModelWithExpectedProperties()
        {
            // Arrange
            string server = "vm-test1";
            string pathName = @"""E:\Icon\API Controller Phase 2\Hierarchy\Icon.ApiController.Controller.exe""  -displayname ""Icon API Controller - Hierarchy"" -servicename ""IconAPIController-Hierarchy""";
            string expectedUncPath = @"\\vm-test1\E$\Icon\API Controller Phase 2\Hierarchy\Icon.ApiController.Controller.exe.config";
            
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);

            // Act
            var uncPath = wmiServiceWrapper.PrependConfigUncPath(server, pathName);

            // Assert
            Assert.AreEqual(expectedUncPath, uncPath);
        }


        [TestMethod]
        public void DetermineEsbEnvironment_WhenNoEsbSettings_ShouldReturnNone()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnvironments = configTestData.Models.EsbEnvironmentsList;
            var esbConnectionSettings = new List<EsbConnectionViewModel>();
            var expectedEsbEnvironmentEnum = EsbEnvironmentEnum.None;
            // Act
            var actualEsbEnvironmentEnum = wmiServiceWrapper
                .DetermineEsbEnvironment(esbEnvironments, esbConnectionSettings);
            // Assert
            Assert.AreEqual(expectedEsbEnvironmentEnum, actualEsbEnvironmentEnum);
        }

        [TestMethod]
        public void DetermineEsbEnvironment_WhenSettingsForEsbTest_ShouldReturnTEST()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnvironments = configTestData.Models.EsbEnvironmentsList;
            var esbConnectionSettings = serviceTestData.Services.EsbConnections.TST_Icon_Single;
            var expectedEsbEnvironmentEnum = EsbEnvironmentEnum.TEST;
            // Act
            var actualEsbEnvironmentEnum = wmiServiceWrapper
                .DetermineEsbEnvironment(esbEnvironments, esbConnectionSettings);
            // Assert
            Assert.AreEqual(expectedEsbEnvironmentEnum, actualEsbEnvironmentEnum);
        }

        [TestMethod]
        public void DetermineEsbEnvironment_WhenSettingsForEsbPrd_ShouldReturnPROD()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnvironments = configTestData.Models.EsbEnvironmentsList;
            esbEnvironments.Add(configTestData.Models.EsbProd);
            var esbConnectionSettings = serviceTestData.Services.EsbConnections.TST_Icon_Single;
            esbConnectionSettings[0].ServerUrl = "ssl://PROD-ESB-EMS-1.wfm.pvt:37293,ssl://PROD-ESB-EMS-2.wfm.pvt:37293,ssl://PROD-ESB-EMS-3.wfm.pvt:37293,ssl://PROD-ESB-EMS-4.wfm.pvt:37293";
            var expectedEsbEnvironmentEnum = EsbEnvironmentEnum.PRD;
            // Act
            var actualEsbEnvironmentEnum = wmiServiceWrapper
                .DetermineEsbEnvironment(esbEnvironments, esbConnectionSettings);
            // Assert
            Assert.AreEqual(expectedEsbEnvironmentEnum, actualEsbEnvironmentEnum);
        }

        [TestMethod]
        public void DetermineEsbEnvironment_WhenUnexpectedEsbServer_ShouldReturnNone()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnvironments = configTestData.Models.EsbEnvironmentsList;
            var esbConnectionSettings = serviceTestData.Services.EsbConnections.TST_Icon_Single;
            esbConnectionSettings[0].ServerUrl = "ssl://POOP-1.wfm.pvt:17293,ssl://POOP-2.wfm.pvt:17293";
            var expectedEsbEnvironmentEnum = EsbEnvironmentEnum.None;
            // Act
            var actualEsbEnvironmentEnum = wmiServiceWrapper
                .DetermineEsbEnvironment(esbEnvironments, esbConnectionSettings);
            // Assert
            Assert.AreEqual(expectedEsbEnvironmentEnum, actualEsbEnvironmentEnum);
        }

        [TestMethod]
        public void DetermineEsbEnvironment_WhenNoEsbDefinitionsInConfig_ShouldNotThrow()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            List<EsbEnvironmentModel> esbEnvironments = null;
            var esbConnectionSettings = serviceTestData.Services.EsbConnections.TST_Icon_Single;
            // Act
            var actualEsbEnvironmentEnum = wmiServiceWrapper
                .DetermineEsbEnvironment(esbEnvironments, esbConnectionSettings);
            // Assert
            // (...no exception=good
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenEsbEnvironmentNull_ShouldReturnEmptyList()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            EsbEnvironmentViewModel esbEnv = null;
            var esbConnectionType = EsbConnectionTypeEnum.Icon;
            // Act
            var settingsToUpdate = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.IsNotNull(settingsToUpdate);
            Assert.AreEqual(0, settingsToUpdate.Count);
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenEsbConnectionTypeNone_ShouldReturnEmptyList()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaPerf;
            var esbConnectionType = EsbConnectionTypeEnum.None;
            // Act
            var settingsToUpdate = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.IsNotNull(settingsToUpdate);
            Assert.AreEqual(0, settingsToUpdate.Count);
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenUpdatingIconServiceWithAllSettingsToPerf_ShouldNotIncludeNonEsbSettings()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaPerf;
            var esbAppSettings = serviceTestData.Services.EsbConnections.TST_Icon_Single.First();
            var esbConnectionType = esbAppSettings.ConnectionType;
            // Act
            var updatedEsbSettings = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.IsFalse(updatedEsbSettings.ContainsKey("ReconnectDelay"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("SendEmails"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("EmailHost"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("EmailPort"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("EmailUsername"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("EmailPassword"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("Sender"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("Recipients"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("NumberOfListenerThreads"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("ResendMessageCount"));
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenUpdatingIconServiceWithAllSettingsToPerf_ShouldNotIncludeFixedEsbSettings()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaPerf;
            var esbAppSettings = serviceTestData.Services.EsbConnections.TST_Icon_Single.First();
            var esbConnectionType = esbAppSettings.ConnectionType;
            // Act
            var updatedEsbSettings = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.IsFalse(updatedEsbSettings.ContainsKey("ConnectionFactoryName"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("SslPassword"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("QueueName"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("CertificateStoreName"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("CertificateStoreLocation"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("SslPassword"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("JmsUsername"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("JndiUsername"));
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenUpdatingIconServiceWithAllSettingsToPerf_ShouldUpdateEsbServerSettings()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaPerf;
            var esbAppSettings = serviceTestData.Services.EsbConnections.TST_Icon_Single.First();
            var esbConnectionType = esbAppSettings.ConnectionType;
            // Act
            var updatedEsbSettings = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.AreEqual("ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293", updatedEsbSettings["ServerUrl"]);
            Assert.AreEqual("PERF-ESB-EMS-1.wfm.pvt", updatedEsbSettings["TargetHostName"]);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US", updatedEsbSettings["CertificateName"]);
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenUpdatingIconServiceWithAllSettingsToPerf_ShouldUpdateEsbUserSettings()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaPerf;
            var esbAppSettings = serviceTestData.Services.EsbConnections.TST_Icon_Single.First();
            var esbConnectionType = esbAppSettings.ConnectionType;
            // Act
            var updatedEsbSettings = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.AreEqual("jms!C0nPERF", updatedEsbSettings["JmsPassword"]);
            Assert.AreEqual("jndiIconUserPERF", updatedEsbSettings["JndiPassword"]);
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenUpdatingIconServiceToPerf_ShouldNotUpdateNonEsbSettings()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaPerf;
            var esbAppSettings = serviceTestData.Services.EsbConnections.TST_Icon_Single.First();
            var esbConnectionType = esbAppSettings.ConnectionType;
             // Act
            var updatedEsbSettings = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.IsFalse(updatedEsbSettings.ContainsKey("ReconnectDelay"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("SendEmails"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("EmailHost"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("EmailPort"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("EmailUsername"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("EmailPassword"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("Sender"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("Recipients"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("NumberOfListenerThreads"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("ResendMessageCount"));
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenUpdatingIconServiceToPerf_ShouldNotIncludeFixedEsbSettings()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaPerf;
            var esbAppSettings = serviceTestData.Services.EsbConnections.TST_Icon_Single.First();
            var esbConnectionType = esbAppSettings.ConnectionType;
            // Act
            var updatedEsbSettings = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.IsFalse(updatedEsbSettings.ContainsKey("ConnectionFactoryName"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("SslPassword"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("QueueName"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("CertificateStoreName"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("CertificateStoreLocation"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("SslPassword"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("JmsUsername"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("JndiUsername"));
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenUpdatingIconServiceToPerf_ShouldUpdateEsbServerSettings()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaPerf;
            var esbAppSettings = serviceTestData.Services.EsbConnections.TST_Icon_Single.First();
            var esbConnectionType = esbAppSettings.ConnectionType;
            // Act
            var updatedEsbSettings = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.AreEqual("ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293", updatedEsbSettings["ServerUrl"]);
            Assert.AreEqual("PERF-ESB-EMS-1.wfm.pvt", updatedEsbSettings["TargetHostName"]);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US", updatedEsbSettings["CertificateName"]);
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenUpdatingIconServiceToPerf_ShouldUpdateEsbUserSettings()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaPerf;
            var esbAppSettings = serviceTestData.Services.EsbConnections.TST_Icon_Single.First();
            var esbConnectionType = esbAppSettings.ConnectionType;
            // Act
            var updatedEsbSettings = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.AreEqual("jms!C0nPERF", updatedEsbSettings["JmsPassword"]);
            Assert.AreEqual("jndiIconUserPERF", updatedEsbSettings["JndiPassword"]);
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenUpdatingMammothServiceToQA_ShouldNotUpdateNonEsbSettings()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaFunc;
            var esbAppSettings = serviceTestData.Services.EsbConnections.TST_Icon_Single.First();
            var esbConnectionType = esbAppSettings.ConnectionType;
            // Act
            var updatedEsbSettings = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.IsFalse(updatedEsbSettings.ContainsKey("SendEmails"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("EmailHost"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("EmailPort"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("EmailUsername"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("EmailPassword"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("Sender"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("Recipients"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("EnablePrimeAffinityMessages"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("ExcludedPSNumbers"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("EligiblePriceTypes"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("NonReceivingSystems"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("PrimeAffinityPsgName"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("PrimeAffinityPsgType"));
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenUpdatingMammothServiceToQA_ShouldNotIncludeFixedEsbSettings()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaFunc;
            var esbAppSettings = serviceTestData.Services.EsbConnections.TST_Mammoth;
            var esbConnectionType = esbAppSettings.ConnectionType;
            // Act
            var updatedEsbSettings = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.IsFalse(updatedEsbSettings.ContainsKey("ConnectionFactoryName"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("SslPassword"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("QueueName"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("CertificateStoreName"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("CertificateStoreLocation"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("SslPassword"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("JmsUsername"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("JndiUsername"));
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenUpdatingMammothServiceToQA_ShouldUpdateEsbServerSettings()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaFunc;
            var esbAppSettings = serviceTestData.Services.EsbConnections.TST_Mammoth;
            var esbConnectionType = esbAppSettings.ConnectionType;
            // Act
            var updatedEsbSettings = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.AreEqual("ssl://QA-ESB-EMS-1.wfm.pvt:27293,ssl://QA-ESB-EMS-2.wfm.pvt:27293", updatedEsbSettings["ServerUrl"]);
            Assert.AreEqual("QA-ESB-EMS-1.wfm.pvt", updatedEsbSettings["TargetHostName"]);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US", updatedEsbSettings["CertificateName"]);
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenUpdatingMammothServiceToQA_ShouldUpdateEsbUserSettings()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaFunc;
            var esbAppSettings = serviceTestData.Services.EsbConnections.TST_Mammoth;
            var esbConnectionType = esbAppSettings.ConnectionType;
            // Act
            var updatedEsbSettings = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.AreEqual("jmsM@mm#!QA", updatedEsbSettings["JmsPassword"]);
            Assert.AreEqual("jndiM@mm$$$QA", updatedEsbSettings["JndiPassword"]);
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenUpdatingEwicServiceToQADup_ShouldNotIncludeFixedEsbSettings()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaDup;
            var esbAppSettings = serviceTestData.Services.EsbConnections.TST_Ewic_Listener;
            var esbConnectionType = esbAppSettings.ConnectionType;
            // Act
            var updatedEsbSettings = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.IsFalse(updatedEsbSettings.ContainsKey("ConnectionFactoryName"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("SslPassword"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("QueueName"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("CertificateStoreName"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("CertificateStoreLocation"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("SslPassword"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("JmsUsername"));
            Assert.IsFalse(updatedEsbSettings.ContainsKey("JndiUsername"));
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenUpdatingEwicServiceToQADup_ShouldUpdateEsbServerSettings()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaDup;
            var esbAppSettings = serviceTestData.Services.EsbConnections.TST_Ewic_Listener;
            var esbConnectionType = esbAppSettings.ConnectionType;
            // Act
            var updatedEsbSettings = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.AreEqual("ssl://DUP-ESB-EMS-1.wfm.pvt:27293,ssl://DUP-ESB-EMS-2.wfm.pvt:27293", updatedEsbSettings["ServerUrl"]);
            Assert.AreEqual("DUP-ESB-EMS-1.wfm.pvt", updatedEsbSettings["TargetHostName"]);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US", updatedEsbSettings["CertificateName"]);
        }

        [TestMethod]
        public void BuildEsbAppSettingsForUpdate_WhenUpdatingEwicServiceToQADup_ShouldUpdateEsbUserSettings()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnv = configTestData.ViewModels.EsbQaDup;
            var esbAppSettings = serviceTestData.Services.EsbConnections.TST_Ewic_Producer;
            var esbConnectionType = esbAppSettings.ConnectionType;
            // Act
            var updatedEsbSettings = wmiServiceWrapper.BuildEsbAppSettingsForUpdate(esbEnv, esbConnectionType);
            // Assert
            Assert.AreEqual("ewic\"#*8DUP", updatedEsbSettings["JmsPassword"]);
            Assert.AreEqual("jndiEw!cUserDUP", updatedEsbSettings["JndiPassword"]);
        }

        [TestMethod]
        public void ReconfigureServicesEsbEnvironmentSettings_WhenEsbModelsNull_ShouldDoNothing()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var apps = new List<ServiceViewModel>();
            List<EsbEnvironmentViewModel> esbEnvironments = null;
            var mockUpdaterService = new Mock<IExternalConfigXmlManager>();
            // Act
            wmiServiceWrapper.ReconfigureServiceEsbEnvironmentSettings(esbEnvironments, apps, mockUpdaterService.Object);
            // Assert
            // (... no exception = good)
        }

        [TestMethod]
        public void ReconfigureServicesEsbEnvironmentSettings_WhenServiceModelsNull_ShouldDoNothing()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var esbEnvironments = configTestData.ViewModels.EsbEnvironmentsList;
            List<ServiceViewModel> apps = null;
            var mockUpdaterService = new Mock<IExternalConfigXmlManager>();
            // Act
            wmiServiceWrapper.ReconfigureServiceEsbEnvironmentSettings(esbEnvironments, apps, mockUpdaterService.Object);
            // Assert
            // (... no exception = good)
        }

        [TestMethod]
        public void ReconfigureServicesEsbEnvironmentSettings_WhenValidParameters_ShouldCallUpdateExternalAppSettingsOnlyForEsbServices()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var mockUpdaterService = new Mock<IExternalConfigXmlManager>();
            var esbEnvironments = configTestData.ViewModels.EsbEnvironmentsList;
            var apps = serviceTestData.Services.ServiceViewModelList;
            // update the services with esb connections to use a new environment
            esbEnvironments.Single(e => e.EsbEnvironment == EsbEnvironmentEnum.QA_PERF)
                .AppsInEnvironment = apps.ToList();
            // Act
            wmiServiceWrapper.ReconfigureServiceEsbEnvironmentSettings(esbEnvironments, apps, mockUpdaterService.Object);
            // Assert
            mockUpdaterService.Verify(a =>
                a.UpdateExternalAppSettings(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()),
                Times.Exactly(2));
            mockUpdaterService.Verify(a =>
                a.ReconfigureEsbEnvironmentCustomConfigSection(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()),
                Times.Exactly(2));
        }

        [TestMethod]
        public void ReconfigureServicesEsbEnvironmentSettings_WhenValidParameters_ShouldCallUpdateExternalAppSettingsForEsbServices_WithExpectedUpdates()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var mockUpdaterService = new Mock<IExternalConfigXmlManager>();
            var esbEnvironments = configTestData.ViewModels.EsbEnvironmentsList;
            var apps = serviceTestData.Services.ServiceViewModelList;
            // update the services with esb connections to use a new environment
            esbEnvironments.Single(e => e.EsbEnvironment == EsbEnvironmentEnum.QA_PERF)
                .AppsInEnvironment = apps.ToList();
            var expectedEsbSettingsToUpdate_Icon = new Dictionary<string, string>
            {
                {"ServerUrl", "ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293"},
                {"TargetHostName", "PERF-ESB-EMS-1.wfm.pvt"},
                {"CertificateName", "E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US"},
                {"JmsPassword", "jms!C0nPERF"},
                {"JndiPassword", "jndiIconUserPERF"},
            };
            var expectedEsbSettingsToUpdate_Mammoth = new Dictionary<string, string>
            {
                {"ServerUrl", "ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293"},
                {"TargetHostName", "PERF-ESB-EMS-1.wfm.pvt"},
                {"CertificateName", "E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US"},
                {"JmsPassword", "jmsM@mm24;PERF"},
                {"JndiPassword", "jndiM@mm$$$PERF"},
            };
            // Act
            wmiServiceWrapper.ReconfigureServiceEsbEnvironmentSettings(esbEnvironments, apps, mockUpdaterService.Object);
            // Assert
            mockUpdaterService.Verify(a =>
                a.UpdateExternalAppSettings(
                    serviceTestData.Services.IconR10ListenerViewModel.ConfigFilePath, 
                    It.Is<Dictionary<string, string>>(
                        d => CustomAsserts.AllExpectedEntriesInDictionary(expectedEsbSettingsToUpdate_Icon, d))),
                Times.Once());
            mockUpdaterService.Verify(a =>
                a.UpdateExternalAppSettings(
                    serviceTestData.Services.MammothItemLocaleControllerViewModel.ConfigFilePath,
                    It.Is<Dictionary<string, string>>(
                        d => CustomAsserts.AllExpectedEntriesInDictionary(expectedEsbSettingsToUpdate_Mammoth, d))),
                Times.Once());
            mockUpdaterService.Verify(a =>
                a.ReconfigureEsbEnvironmentCustomConfigSection(
                    serviceTestData.Services.MammothProductListenerViewModel.ConfigFilePath,
                    "ESB",
                    It.Is<Dictionary<string, string>>(
                        d => CustomAsserts.AllExpectedEntriesInDictionary(expectedEsbSettingsToUpdate_Mammoth, d))),
                Times.Once());
            mockUpdaterService.Verify(a =>
                a.ReconfigureEsbEnvironmentCustomConfigSection(
                    serviceTestData.Services.MammothProductListenerViewModel.ConfigFilePath,
                    "R10",
                    It.Is<Dictionary<string, string>>(
                        d => CustomAsserts.AllExpectedEntriesInDictionary(expectedEsbSettingsToUpdate_Icon, d))),
                Times.Once());
        }

        [TestMethod]
        public void ReconfigureServiceEsbEnvironmentSettings_WhenValidParameters_ShouldNotAttemptToUpdateServicesWithInvalidConfigPaths()
        {
            // Arrange
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(mockWMiSvc.Object);
            var mockUpdaterService = new Mock<IExternalConfigXmlManager>();
            var esbEnvironments = configTestData.ViewModels.EsbEnvironmentsList;
            var apps = serviceTestData.Services.ServiceViewModelList;
            // update the services with esb connections to use a new environment
            esbEnvironments.Single(e => e.EsbEnvironment == EsbEnvironmentEnum.QA_PERF)
                .AppsInEnvironment = apps.ToList();
            apps.Single(a => a.Name == "Mammoth.ItemLocale.Controller")
                .ConfigFilePathIsValid = false;
            // Act
            wmiServiceWrapper.ReconfigureServiceEsbEnvironmentSettings(esbEnvironments, apps, mockUpdaterService.Object);
            // Assert
            mockUpdaterService.Verify(a =>
                a.UpdateExternalAppSettings(It.IsAny<Dictionary<string, string>>()),
                Times.Never());
        }
    }
}
