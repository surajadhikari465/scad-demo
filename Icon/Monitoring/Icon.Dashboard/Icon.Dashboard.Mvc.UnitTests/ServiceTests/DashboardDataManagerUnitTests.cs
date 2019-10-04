using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.UnitTests.TestData;
using Icon.Dashboard.Mvc.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ServiceTests
{
    [TestClass]
    public class DashboardDataManagerUnitTests
    {
        DbWrapperTestData dbWrapperTestData = new DbWrapperTestData();
        RemoteServiceTestData serviceTestData = new RemoteServiceTestData();
        ConfigTestData configTestData = new ConfigTestData();

        [TestMethod]
        public void GetEnvironmentDefinitonBasedOnCookie_WhenCookieIsNull_ShouldReturnNullModel()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var altEnv = dashboardDataManager.GetEnvironmentDefinitonBasedOnCookie(null);
            // Assert
            Assert.IsNull(altEnv);
        }

        [TestMethod]
        public void GetEnvironmentDefinitonBasedOnCookie_WhenCookieIsStandardEnvironment_ShouldReturnModelWithExpectedValue()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expectedEnvironmentEnum = EnvironmentEnum.Tst1;
            var expectedEnvironmentName = expectedEnvironmentEnum.ToString();
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);
            // Act
            var altEnv = dashboardDataManager.GetEnvironmentDefinitonBasedOnCookie(cookieModel);
            // Assert
            Assert.IsNotNull(altEnv);
            CustomAsserts.EnvironmentModelsEqual(configTestData.Models.Tst1, altEnv);
        }

        [TestMethod]
        public void GetEnvironmentDefinitonBasedOnCookie_WhenCookieIsCustomEnvironment_ShouldReturnModelWithExpectedValue()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var expectedEnvironmentName = "Ed's Env";
            var expectedAppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" };
            var cookieModel = new EnvironmentCookieModel(expectedEnvironmentName, expectedEnvironmentEnum);
            cookieModel.AppServers = expectedAppServers.ToList();
            // Act
            var altEnv = dashboardDataManager.GetEnvironmentDefinitonBasedOnCookie(cookieModel);
            // Assert
            Assert.IsNotNull(altEnv);
            Assert.AreEqual(expectedEnvironmentName, altEnv.Name);
            Assert.AreEqual(expectedEnvironmentEnum, altEnv.EnvironmentEnum);
            CustomAsserts.ListsAreEqual(expectedAppServers, altEnv.AppServers);
        }

        [TestMethod]
        public void GetEnvironmentCookieModelFromEnum_WhenUndefined_ShouldReturnNull()
        {
            // Arrange
            var expectedEnvironmentEnum = EnvironmentEnum.Undefined;
            var expectedEnvironmentName = expectedEnvironmentEnum.ToString();
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var cookieModel = dashboardDataManager.GetEnvironmentCookieModelFromEnum(expectedEnvironmentEnum);
            // Assert
            Assert.IsNull(cookieModel);
        }

        [TestMethod]
        public void GetEnvironmentCookieModelFromEnum_WhenMatchingDefinitionNotFound_ShouldReturnNull()
        {
            // Arrange
            var expectedEnvironmentEnum = EnvironmentEnum.Undefined;
            var expectedEnvironmentName = expectedEnvironmentEnum.ToString();
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var cookieModel = dashboardDataManager.GetEnvironmentCookieModelFromEnum(expectedEnvironmentEnum);
            // Assert
            Assert.IsNull(cookieModel);
        }

        [TestMethod]
        public void GetEnvironmentCookieModelFromEnum_WhenCustomEnvironment_ShouldReturnNull()
        {
            // Arrange
            var expectedEnvironmentEnum = EnvironmentEnum.Custom;
            var expectedEnvironmentName = expectedEnvironmentEnum.ToString();
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var cookieModel = dashboardDataManager.GetEnvironmentCookieModelFromEnum(expectedEnvironmentEnum);
            // Assert
            Assert.IsNull(cookieModel);
        }

        [TestMethod]
        public void GetEnvironmentCookieModelFromEnum_MatchingDefinitionFound_ShouldReturnExpectedModel()
        {
            // Arrange
            var expectedEnvironmentEnum = EnvironmentEnum.QA;
            var expectedEnvironmentName = expectedEnvironmentEnum.ToString();
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var cookieModel = dashboardDataManager.GetEnvironmentCookieModelFromEnum(expectedEnvironmentEnum);
            // Assert
            Assert.IsNotNull(cookieModel);
            Assert.AreEqual(expectedEnvironmentName, cookieModel.Name);
            Assert.AreEqual(expectedEnvironmentEnum, cookieModel.EnvironmentEnum);
        }

        [TestMethod]
        public void GetEnvironmentCookieModelFromEnum_MatchingDefinitionFound_ShouldReturnModelWithExpectedAppServers()
        {
            // Arrange
            var expectedEnvironmentEnum = EnvironmentEnum.QA;
            var expectedEnvironmentName = expectedEnvironmentEnum.ToString();
            var expectedAppServers = configTestData.Models.QA.AppServers;
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var cookieModel = dashboardDataManager.GetEnvironmentCookieModelFromEnum(expectedEnvironmentEnum);
            // Assert
            CustomAsserts.ListsAreEqual(expectedAppServers, cookieModel.AppServers);
        }

        [TestMethod]
        public void GetEnvironmentViewModels_WhenStandardDefinitionsInConfig_ReturnsCollectionWithExpectedCount()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expectedEnvironmentCount = configTestData.ConfigDataModel.EnvironmentDefinitions.Count + 1;
            // Act
            var envViewModels = dashboardDataManager.GetEnvironmentViewModels();
            // Assert
            Assert.IsNotNull(envViewModels);
            Assert.IsNotNull(envViewModels.Environments);
            Assert.AreEqual(expectedEnvironmentCount, envViewModels.Environments.Count);
        }

        [TestMethod]
        public void GetEnvironmentViewModels_WhenStandardDefinitionsInConfig_ReturnsCollectionWithExpectedSelectionIndex()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expectedSelectionIndex = 2;
            // Act
            var envViewModels = dashboardDataManager.GetEnvironmentViewModels();
            // Assert
            Assert.AreEqual(expectedSelectionIndex, envViewModels.SelectedEnvIndex);
        }

        [TestMethod]
        public void GetEnvironmentViewModels_WhenStandardDefinitionsInConfig_ReturnsCollectionWithExpectedViewModelForHostingEnv()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var hostingEnvironmentEnum = EnvironmentEnum.Tst0;
            var expectedEnvironmentCount = configTestData.ConfigDataModel.EnvironmentDefinitions.Count;
            // Act
            var envViewModels = dashboardDataManager.GetEnvironmentViewModels();
            // Assert
            var hostingEnvViewModel = envViewModels.Environments
                .Single(e => e.EnvironmentEnum == hostingEnvironmentEnum);
            Assert.IsNotNull(hostingEnvViewModel);
            Assert.AreEqual(hostingEnvironmentEnum.ToString(), hostingEnvViewModel.Name);
            Assert.AreEqual(hostingEnvironmentEnum, hostingEnvViewModel.EnvironmentEnum);
            Assert.AreEqual(true, hostingEnvViewModel.IsHostingEnvironment);
            Assert.AreEqual(false, hostingEnvViewModel.IsProduction);
            Assert.AreEqual("primary", hostingEnvViewModel.BootstrapClass);
            Assert.IsNotNull(hostingEnvViewModel.AppServers);
            var expectedAppServers = new List<AppServerViewModel>
            {
                new AppServerViewModel("vm-icon-test1"),
                new AppServerViewModel("vm-icon-test2")
            };
            CustomAsserts.ListsAreEqual(expectedAppServers, hostingEnvViewModel.AppServers);
        }

        [TestMethod]
        public void GetEnvironmentViewModels_WhenStandardDefinitionsInConfig_ReturnsCollectionWithExpectedViewModelForNonProdNonHostingEnv()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var environmentEnum = EnvironmentEnum.QA;
            // Act
            var envViewModels = dashboardDataManager.GetEnvironmentViewModels();
            // Assert
            var envViewModel = envViewModels.Environments
                .Single(e => e.EnvironmentEnum == environmentEnum);
            Assert.IsNotNull(envViewModel);
            Assert.AreEqual(environmentEnum.ToString(), envViewModel.Name);
            Assert.AreEqual(environmentEnum, envViewModel.EnvironmentEnum);
            Assert.AreEqual(false, envViewModel.IsHostingEnvironment);
            Assert.AreEqual(false, envViewModel.IsProduction);
            Assert.AreEqual("warning", envViewModel.BootstrapClass);
            Assert.IsNotNull(envViewModel.AppServers);
            var expectedAppServers = new List<AppServerViewModel>
            {
                new AppServerViewModel("vm-icon-qa1"),
                new AppServerViewModel("vm-icon-qa2"),
                new AppServerViewModel("mammoth-app01-qa"),
            };
            CustomAsserts.ListsAreEqual(expectedAppServers, envViewModel.AppServers);
        }

        [TestMethod]
        public void GetEnvironmentViewModels_WhenStandardDefinitionsInConfig_ReturnsCollectionWithExpectedViewModelForProdNonHostingEnv()
        {
            // Arrange
            var environmentEnum = EnvironmentEnum.Prd;
            configTestData.ConfigDataModel.EnvironmentDefinitions
                .Single(e => e.EnvironmentEnum == environmentEnum)
                .IsEnabled = true;
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var envViewModels = dashboardDataManager.GetEnvironmentViewModels();
            // Assert
            var envViewModel = envViewModels.Environments
                .Single(e => e.EnvironmentEnum == environmentEnum);
            Assert.IsNotNull(envViewModel);
            Assert.AreEqual(environmentEnum.ToString(), envViewModel.Name);
            Assert.AreEqual(environmentEnum, envViewModel.EnvironmentEnum);
            Assert.AreEqual(false, envViewModel.IsHostingEnvironment);
            Assert.AreEqual(true, envViewModel.IsProduction);
            Assert.AreEqual("danger", envViewModel.BootstrapClass);
            Assert.IsNotNull(envViewModel.AppServers);
            var expectedAppServers = new List<AppServerViewModel>
            {
                new AppServerViewModel("vm-icon-prd1"),
                new AppServerViewModel("vm-icon-prd2"),
                new AppServerViewModel("vm-icon-prd3"),
                new AppServerViewModel("vm-icon-prd4"),
                new AppServerViewModel("vm-icon-prd5"),
                new AppServerViewModel("vm-icon-prd6"),
                new AppServerViewModel("mammoth-app01-prd"),
                new AppServerViewModel("mammoth-app02-prd")
            };
            CustomAsserts.ListsAreEqual(expectedAppServers, envViewModel.AppServers);
        }

        [TestMethod]
        public void GetAllEsbEnvironmentViewModels_WhenNoConfigData_ShouldReturnEmptyList()
        {
            // Arrange
            configTestData.ConfigDataModel.EsbEnvironmentDefinitions = null;
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var esbEnvViewModels = dashboardDataManager.GetAllEsbEnvironmentViewModels();
            // Assert
            Assert.IsNotNull(esbEnvViewModels);
            Assert.AreEqual(0, esbEnvViewModels.Count);
        }

        [TestMethod]
        public void GetAllEsbEnvironmentViewModels_WhenNoEsbEnvironmentDataInConfig_ShouldReturnEmptyList()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            configTestData.ConfigDataModel.EsbEnvironmentDefinitions = null;
            // Act
            var esbEnvViewModels = dashboardDataManager.GetAllEsbEnvironmentViewModels();
            // Assert
            Assert.IsNotNull(esbEnvViewModels);
            Assert.AreEqual(0, esbEnvViewModels.Count);
        }

        [TestMethod]
        public void GetAllEsbEnvironmentViewModels_WhenValidEsbEnvironmentDataInConfig_ShouldReturnExpectedModelCount()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expectedCount = 6;
            // Act
            var esbEnvViewModels = dashboardDataManager.GetAllEsbEnvironmentViewModels();
            // Assert
            Assert.IsNotNull(esbEnvViewModels);
            Assert.AreEqual(expectedCount, esbEnvViewModels.Count);
        }

        [TestMethod]
        public void GetAllEsbEnvironmentViewModels_WhenValidEsbEnvironmentDataInConfig_ShouldReturnExpectedModelsInOrder()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var esbEnvViewModels = dashboardDataManager.GetAllEsbEnvironmentViewModels();
            // Assert
            var x = Enum.GetValues(typeof(EsbEnvironmentEnum)).Cast<EsbEnvironmentEnum>().OrderBy(e => e).ToList();
            Assert.AreEqual("DEV", esbEnvViewModels[0].Name);
            Assert.AreEqual("TEST", esbEnvViewModels[1].Name);
            Assert.AreEqual("TEST-DUP", esbEnvViewModels[2].Name);
            Assert.AreEqual("QA-DUP", esbEnvViewModels[3].Name);
            Assert.AreEqual("QA-PERF", esbEnvViewModels[4].Name);
            Assert.AreEqual("QA-FUNC", esbEnvViewModels[5].Name);
        }

        [TestMethod]
        public void GetEsbEnvironmentViewModelsWithAppsPopulated_WhenServiceListIsNull_ShouldReturnEsbEnvironmentsWithoutApps()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            List<ServiceViewModel> apps = null;
            // Act
            var esbEnvViewModels = dashboardDataManager.GetEsbEnvironmentViewModelsWithAssignedServices(apps);
            // Assert
            Assert.IsNotNull(esbEnvViewModels);
            Assert.AreEqual(configTestData.ConfigDataModel.EsbEnvironmentDefinitions.Count, esbEnvViewModels.Count);
            foreach (var esbEnv in esbEnvViewModels)
            {
                Assert.IsNotNull(esbEnv.AppsInEnvironment);
                Assert.AreEqual(0, esbEnv.AppsInEnvironment.Count);
            }
        }

        [TestMethod]
        public void GetEsbEnvironmentViewModelsWithAppsPopulated_WhenServicesHaveEsbSettings_ShouldReturnExpectedEsbEnvironmentsWithApps()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var apps = serviceTestData.Services.ServiceViewModelList;
            // Act
            var esbEnvViewModels = dashboardDataManager.GetEsbEnvironmentViewModelsWithAssignedServices(apps);
            // Assert
            Assert.IsNotNull(esbEnvViewModels);
            Assert.AreEqual(configTestData.ConfigDataModel.EsbEnvironmentDefinitions.Count, esbEnvViewModels.Count);
            foreach (var esbEnv in esbEnvViewModels)
            {
                Assert.IsNotNull(esbEnv.AppsInEnvironment);
                switch (esbEnv.EsbEnvironment)
                {
                    case EsbEnvironmentEnum.DEV:
                        Assert.AreEqual(0, esbEnv.AppsInEnvironment.Count);
                        break;
                    case EsbEnvironmentEnum.TEST:
                        Assert.AreEqual(2, esbEnv.AppsInEnvironment.Count);
                        break;
                    case EsbEnvironmentEnum.QA_FUNC:
                        Assert.AreEqual(1, esbEnv.AppsInEnvironment.Count);
                        break;
                    case EsbEnvironmentEnum.QA_DUP:
                        Assert.AreEqual(0, esbEnv.AppsInEnvironment.Count);
                        break;
                    case EsbEnvironmentEnum.QA_PERF:
                        Assert.AreEqual(0, esbEnv.AppsInEnvironment.Count);
                        break;
                    case EsbEnvironmentEnum.None:
                    case EsbEnvironmentEnum.DEV_DUP:
                    case EsbEnvironmentEnum.TEST_DUP:
                    case EsbEnvironmentEnum.PRD:
                    default:
                        break;
                }
            }
        }

        [TestMethod]
        public void GetEsbEnvironmentViewModelsWithAppsPopulated_ShouldOrderViewModelsByEnumValue()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var apps = serviceTestData.Services.ServiceViewModelList;
            // Act
            var esbEnvViewModels = dashboardDataManager.GetEsbEnvironmentViewModelsWithAssignedServices(apps);
            // Assert
            Assert.AreEqual("DEV", esbEnvViewModels[0].Name);
            Assert.AreEqual("TEST", esbEnvViewModels[1].Name);
            Assert.AreEqual("TEST-DUP", esbEnvViewModels[2].Name);
            Assert.AreEqual("QA-DUP", esbEnvViewModels[3].Name);
            Assert.AreEqual("QA-PERF", esbEnvViewModels[4].Name);
            Assert.AreEqual("QA-FUNC", esbEnvViewModels[5].Name);
        }


        [TestMethod]
        public void GetEnvironmentForRemoteServer_WhenNullAppServer_ShouldReturnNull()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            string appServer = null;
            // Act
            var actualEnvironment = dashboardDataManager.GetEnvironmentForRemoteServer(appServer);
            // Assert
            Assert.IsNull(actualEnvironment);
        }

        [TestMethod]
        public void GetEnvironmentForRemoteServer_WhenWhitespaceAppServer_ShouldReturnNull()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            string appServer = "\t";
            // Act
            var actualEnvironment = dashboardDataManager.GetEnvironmentForRemoteServer(appServer);
            // Assert
            Assert.IsNull(actualEnvironment);
        }

        [TestMethod]
        public void GetEnvironmentForRemoteServer_WhenUnknownAppServer_ShouldReturnNull()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var appServer = "aldsfkjao23";
            // Act
            var actualEnvironment = dashboardDataManager.GetEnvironmentForRemoteServer(appServer);
            // Assert
            Assert.IsNull(actualEnvironment);
        }

        [TestMethod]
        public void GetEnvironmentForRemoteServer_WhenAppServerForTst0_ShouldReturnExpectedEnvironment()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expectedEnvironmentEnum = EnvironmentEnum.Tst0;
            var appServer = configTestData.Models.Tst0.AppServers[0];
            // Act
            var actualEnvironment = dashboardDataManager.GetEnvironmentForRemoteServer(appServer);
            // Assert
            Assert.AreEqual(expectedEnvironmentEnum, actualEnvironment.EnvironmentEnum);
        }

        [TestMethod]
        public void GetEnvironmentForRemoteServer_WhenAppServerForPerf_ShouldReturnExpectedEnvironment()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expectedEnvironmentEnum = EnvironmentEnum.Perf;
            var appServer = configTestData.Models.Perf.AppServers[1];
            // Act
            var actualEnvironment = dashboardDataManager.GetEnvironmentForRemoteServer(appServer);
            // Assert
            Assert.AreEqual(expectedEnvironmentEnum, actualEnvironment.EnvironmentEnum);
        }

        [TestMethod]
        public void GetEnvironmentForRemoteServer_WhenAppServerNameContainsPrd_ShouldReturnPrd()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expectedEnvironmentEnum = EnvironmentEnum.Prd;
            var appServer = "mammoth-app02-prd";
            // Act
            var actualEnvironment = dashboardDataManager.GetEnvironmentForRemoteServer(appServer);
            // Assert
            Assert.AreEqual(expectedEnvironmentEnum, actualEnvironment.EnvironmentEnum);
        }

        [TestMethod]
        public void EnvironmentHasProductionServers_WhenModelIsNull_ShouldReturnFalse()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expected = false;
            EnvironmentModel envModel = null;
            // Act
            var actual = dashboardDataManager.EnvironmentHasProductionServers(envModel);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvironmentHasProductionServers_WhenModelIsEmpty_ShouldReturnFalse()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expected = false;
            var envModel = new EnvironmentModel();
            // Act
            var actual = dashboardDataManager.EnvironmentHasProductionServers(envModel);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvironmentHasProductionServers_WhenModelEnvironmentEnumIsPrd_ShouldReturnTrue()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expected = true;
            var envModel = new EnvironmentModel()
            {
                EnvironmentEnum = EnvironmentEnum.Prd
            };
            // Act
            var actual = dashboardDataManager.EnvironmentHasProductionServers(envModel);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvironmentHasProductionServers_WhenModelIsNonPrdWithoutAppServerDefinedForPrd_ShouldReturnFalse()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expected = false;
            var envModel = configTestData.Models.Tst0;
            // Act
            var actual = dashboardDataManager.EnvironmentHasProductionServers(envModel);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvironmentHasProductionServers_WhenModelIsNonPrdWithAppServerDefinedForPrd_ShouldReturnFalse()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expected = true;
            var envModel = configTestData.Models.Tst0;
            envModel.AppServers.Add(configTestData.Models.Prd.AppServers[0]);
            // Act
            var actual = dashboardDataManager.EnvironmentHasProductionServers(envModel);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvironmentHasProductionServers_WhenModelIsNonPrdWithoutAppServerDefinedForPrdButAppServerHasPrdInName_ShouldReturnFalse()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expected = true;
            var envModel = configTestData.Models.Tst0;
            envModel.AppServers.Add("mammoth-app02-prd");
            // Act
            var actual = dashboardDataManager.EnvironmentHasProductionServers(envModel);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetPermissionsAndActiveEnvironmentBasedOnCookieSettings_WhenUserCannotEditAndCookieForTestEnvironment_ChangesAreNotAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = false;
            var environmentCookieModel = new EnvironmentCookieModel
            {
                Name = "Tst0",
                EnvironmentEnum = EnvironmentEnum.Tst0,
                AppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" }
            };
            // Act
            dashboardDataManager.SetPermissionsForActiveEnvironment(userCanEdit, environmentCookieModel);
            // Assert
            Assert.IsFalse(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsAndActiveEnvironmentBasedOnCookieSettings_WhenUserCanEditAndCookieForTestEnvironment_ChangesAreAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = true;
            var environmentCookieModel = new EnvironmentCookieModel
            {
                Name = "Tst0",
                EnvironmentEnum = EnvironmentEnum.Tst0,
                AppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" }
            };
            // Act
            dashboardDataManager.SetPermissionsForActiveEnvironment(userCanEdit, environmentCookieModel);
            // Assert
            Assert.IsTrue(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsAndActiveEnvironmentBasedOnCookieSettings_WhenUserCannotEditAndCookieForQAEnvironment_ChangesAreNotAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = false;
            var environmentCookieModel = new EnvironmentCookieModel
            {
                Name = "QA",
                EnvironmentEnum = EnvironmentEnum.QA,
                AppServers = new List<string> { "vm-icon-qa1", "vm-icon-qa2", "mammoth-app01-qa" }
            };
            // Act
            dashboardDataManager.SetPermissionsForActiveEnvironment(userCanEdit, environmentCookieModel);
            // Assert
            Assert.IsFalse(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsAndActiveEnvironmentBasedOnCookieSettings_WhenUserCanEditAndCookieForQAEnvironment_ChangesAreAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = true;
            var environmentCookieModel = new EnvironmentCookieModel
            {
                Name = "QA",
                EnvironmentEnum = EnvironmentEnum.QA,
                AppServers = new List<string> { "vm-icon-qa1", "vm-icon-qa2", "mammoth-app01-qa" }
            };
            // Act
            dashboardDataManager.SetPermissionsForActiveEnvironment(userCanEdit, environmentCookieModel);
            // Assert
            Assert.IsTrue(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsAndActiveEnvironmentBasedOnCookieSettings_WhenUserCannotEditAndCookieForProdEnvironment_ChangesAreNotAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = false;
            var environmentCookieModel = new EnvironmentCookieModel
            {
                Name = "Prd",
                EnvironmentEnum = EnvironmentEnum.Prd,
                AppServers = new List<string> { "vm-icon-prd1", "vm-icon-prd2", "vm-icon-prd3", "vm-icon-prd4", "vm-icon-prd5", "vm-icon-prd6", "mammoth-app01-prd", "mammoth-app02-prd" }
            };
            // Act
            dashboardDataManager.SetPermissionsForActiveEnvironment(userCanEdit, environmentCookieModel);
            // Assert
            Assert.IsFalse(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsAndActiveEnvironmentBasedOnCookieSettings_WhenUserCanEditAndCookieForProdEnvironment_ChangesAreNotAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = false;
            var environmentCookieModel = new EnvironmentCookieModel
            {
                Name = "Prd",
                EnvironmentEnum = EnvironmentEnum.Prd,
                AppServers = new List<string> { "vm-icon-prd1", "vm-icon-prd2", "vm-icon-prd3", "vm-icon-prd4", "vm-icon-prd5", "vm-icon-prd6", "mammoth-app01-prd", "mammoth-app02-prd" }
            };
            // Act
            dashboardDataManager.SetPermissionsForActiveEnvironment(userCanEdit, environmentCookieModel);
            // Assert
            Assert.IsFalse(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsAndActiveEnvironmentBasedOnCookieSettings_WhenUserCannotEditAndCookieForCustomNonPrdEnvironment_ChangesAreNotAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = false;
            var environmentCookieModel = new EnvironmentCookieModel
            {
                Name = "Special",
                EnvironmentEnum = EnvironmentEnum.Custom,
                AppServers = new List<string> { "vm-icon-qa1", "vm-icon-qa2", "vm-icon-qa3", "vm-icon-qa4" }
            };
            // Act
            dashboardDataManager.SetPermissionsForActiveEnvironment(userCanEdit, environmentCookieModel);
            // Assert
            Assert.IsFalse(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsAndActiveEnvironmentBasedOnCookieSettings_WhenUserCanEditAndCookieForCustomNonPrdEnvironment_ChangesAreAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = true;
            var environmentCookieModel = new EnvironmentCookieModel
            {
                Name = "Special",
                EnvironmentEnum = EnvironmentEnum.Custom,
                AppServers = new List<string> { "vm-icon-qa1", "vm-icon-qa2", "vm-icon-qa3", "vm-icon-qa4" }
            };
            // Act
            dashboardDataManager.SetPermissionsForActiveEnvironment(userCanEdit, environmentCookieModel);
            // Assert
            Assert.IsTrue(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsAndActiveEnvironmentBasedOnCookieSettings_WhenUserCannotEditAndCookieForCustomPrdEnvironment_ChangesAreNotAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = false;
            var environmentCookieModel = new EnvironmentCookieModel
            {
                Name = "Special",
                EnvironmentEnum = EnvironmentEnum.Custom,
                AppServers = new List<string> { "vm-icon-qa2", "vm-icon-qa3", "mammoth-app01-qa", "mammoth-app01-prd" }
            };
            // Act
            dashboardDataManager.SetPermissionsForActiveEnvironment(userCanEdit, environmentCookieModel);
            // Assert
            Assert.IsFalse(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsAndActiveEnvironmentBasedOnCookieSettings_WhenUserCanEditAndCookieForCustomPrdEnvironment_ChangesAreNotAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = true;
            var environmentCookieModel = new EnvironmentCookieModel
            {
                Name = "Special",
                EnvironmentEnum = EnvironmentEnum.Custom,
                AppServers = new List<string> { "vm-icon-qa2", "vm-icon-qa3", "mammoth-app01-qa", "mammoth-app01-prd" }
            };
            // Act
            dashboardDataManager.SetPermissionsForActiveEnvironment(userCanEdit, environmentCookieModel);
            // Assert
            Assert.IsFalse(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsAndActiveEnvironmentBasedOnCookieSettings_WhenCustomDifferentFromEnvironment_DoesSetAlternateEnvironment()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = true;
            var environmentCookieModel = new EnvironmentCookieModel
            {
                Name = "QA",
                EnvironmentEnum = EnvironmentEnum.QA,
                AppServers = new List<string> { "vm-icon-qa1", "vm-icon-qa2", "mammoth-app01-qa" }
            };
            // Act
            dashboardDataManager.SetPermissionsForActiveEnvironment(userCanEdit, environmentCookieModel);
            // Assert
            Assert.IsNotNull(dashboardDataManager.AlternateEnvironment);
            Assert.AreEqual(EnvironmentEnum.QA, dashboardDataManager.AlternateEnvironment.EnvironmentEnum);
        }

        [TestMethod]
        public void SetPermissionsAndActiveEnvironmentBasedOnCookieSettings_WhenCookieTheSameAsHostingEnvironment_DoesNotSetAlternateEnvironment()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = true;
            var environmentCookieModel = new EnvironmentCookieModel
            {
                Name = "Tst0",
                EnvironmentEnum = EnvironmentEnum.Tst0,
                AppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" }
            };
            // Act
            dashboardDataManager.SetPermissionsForActiveEnvironment(userCanEdit, environmentCookieModel);
            // Assert
            Assert.IsNull(dashboardDataManager.AlternateEnvironment);
        }

        [TestMethod]
        public void SetPermissionsAndActiveEnvironmentBasedOnCookieSettings_WhenCustomIsSubsetOfHostingEnvironment_DoesSetAlternateEnvironment()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = true;
            var environmentCookieModel = new EnvironmentCookieModel
            {
                Name = "MyCustom",
                EnvironmentEnum = EnvironmentEnum.Custom,
                AppServers = new List<string> { "vm-icon-test1" }
            };
            // Act
            dashboardDataManager.SetPermissionsForActiveEnvironment(userCanEdit, environmentCookieModel);
            // Assert
            Assert.IsNotNull(dashboardDataManager.AlternateEnvironment);
            Assert.AreEqual(EnvironmentEnum.Custom, dashboardDataManager.AlternateEnvironment.EnvironmentEnum);
        }

        [TestMethod]
        public void SetPermissionsAndActiveEnvironmentBasedOnCookieSettings_WhenCustomPrdEnvironment_DoesSetAlternateEnvironment()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = false;
            var environmentCookieModel = new EnvironmentCookieModel
            {
                Name = "Special",
                EnvironmentEnum = EnvironmentEnum.Custom,
                AppServers = new List<string> { "vm-icon-qa2", "vm-icon-qa3", "mammoth-app01-qa", "mammoth-app01-prd" }
            };
            // Act
            dashboardDataManager.SetPermissionsForActiveEnvironment(userCanEdit, environmentCookieModel);
            // Assert
            Assert.IsNotNull(dashboardDataManager.AlternateEnvironment);
            Assert.AreEqual(EnvironmentEnum.Custom, dashboardDataManager.AlternateEnvironment.EnvironmentEnum);
        }

        [TestMethod]
        public void SetPermissionsForRemoteEnvironment_WhenUserCannotEditAndServiceInTestEnvironment_ChangesAreNotAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = false;
            var appServer = "vm-icon-test1";
            // Act
            dashboardDataManager.SetPermissionsForRemoteEnvironment(userCanEdit, appServer);
            // Assert
            Assert.IsFalse(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsForRemoteEnvironment_WhenUserCanEditAndServiceInTestEnvironment_ChangesAreAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = true;
            var appServer = "vm-icon-test1";
            // Act
            dashboardDataManager.SetPermissionsForRemoteEnvironment(userCanEdit, appServer);
            // Assert
            Assert.IsTrue(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsForRemoteEnvironment_WhenUserCannotEditAndServiceInQAEnvironment_ChangesAreNotAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = false;
            var appServer = "mammoth-app01-qa";
            // Act
            dashboardDataManager.SetPermissionsForRemoteEnvironment(userCanEdit, appServer);
            // Assert
            Assert.IsFalse(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsForRemoteEnvironment_WhenUserCanEditAndServiceInQAEnvironment_ChangesAreAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = true;
            var appServer = "mammoth-app01-qa";
            // Act
            dashboardDataManager.SetPermissionsForRemoteEnvironment(userCanEdit, appServer);
            // Assert
            Assert.IsTrue(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsForRemoteEnvironment_WhenUserCannotEditAndServiceInProdEnvironment_ChangesAreNotAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = false;
            var appServer = "vm-icon-prd6";
            // Act
            dashboardDataManager.SetPermissionsForRemoteEnvironment(userCanEdit, appServer);
            // Assert
            Assert.IsFalse(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsForRemoteEnvironment_WhenUserCanEditAndServiceInProdEnvironment_ChangesAreNotAllowed()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = true;
            var appServer = "vm-icon-prd6";
            // Act
            dashboardDataManager.SetPermissionsForRemoteEnvironment(userCanEdit, appServer);
            // Assert
            Assert.IsFalse(dashboardDataManager.AreChangesAllowed);
        }

        [TestMethod]
        public void SetPermissionsForRemoteEnvironment_WhenServiceInHostingEnvironment_DoesNotSetAlternateEnvironment()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = true;
            var appServer = "vm-icon-test1";
            // Act
            dashboardDataManager.SetPermissionsForRemoteEnvironment(userCanEdit, appServer);
            // Assert
            Assert.IsNull(dashboardDataManager.AlternateEnvironment);
        }


        [TestMethod]
        public void SetPermissionsForRemoteEnvironment_WhenServiceNotInHostingEnvironment_DoesSetAlternateEnvironment()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var userCanEdit = true;
            var appServer = "vm-icon-qa3";
            // Act
            dashboardDataManager.SetPermissionsForRemoteEnvironment(userCanEdit, appServer);
            // Assert
            Assert.IsNotNull(dashboardDataManager.AlternateEnvironment);
            Assert.AreEqual(EnvironmentEnum.Perf, dashboardDataManager.AlternateEnvironment.EnvironmentEnum);
        }


        [TestMethod]
        public void BuildSubMenuForSupportApps_Dev0_ShouldReturnsExpectedUrls()
        {
            // Arrange
            var environment = EnvironmentEnum.Dev0;
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expectedMwsServerNameDev = $"Mammoth Web Support Dev0";
            var expectedIconWebServerNameDev = $"Icon Web Dev0";
            var expectedTibcoServerNameDev = $"TIBCO Admin Dev0";
            var expectedMwsServerDev = $"http://irmadevapp1/MammothWebSupport";
            var expectedIconWebServerDev = $"http://icon-dev/";
            var expectedTibcoServerDev = $"https://cerd1668:8090/";
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForSupportApps(environment);
            // Assert
            Assert.IsNotNull(subMenu);
            var subSubMenuForEnv = subMenu.EnvironmentSubMenus.Single(m => m.EnvironmentEnum == environment);
            Assert.IsNotNull(subSubMenuForEnv);
            Assert.IsTrue(subSubMenuForEnv.SubMenuItems.Any(s => s.VisibleText.Equals(expectedMwsServerNameDev)));
            Assert.AreEqual(expectedMwsServerDev, subSubMenuForEnv.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedMwsServerNameDev)).Link);
            Assert.IsTrue(subSubMenuForEnv.SubMenuItems.Any(s => s.VisibleText.Equals(expectedIconWebServerNameDev)));
            Assert.AreEqual(expectedIconWebServerDev, subSubMenuForEnv.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedIconWebServerNameDev)).Link);
            Assert.IsTrue(subSubMenuForEnv.SubMenuItems.Any(s => s.VisibleText.Equals(expectedTibcoServerNameDev)));
            Assert.AreEqual(expectedTibcoServerDev, subSubMenuForEnv.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedTibcoServerNameDev)).Link);
        }

        [TestMethod]
        public void BuildSubMenuForSupportApps_Tst0_ShouldReturnsExpectedUrls()
        {
            // Arrange
            var environment = EnvironmentEnum.Tst0;
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expectedMwsServerNameTest = $"Mammoth Web Support Tst0";
            var expectedIconWebServerNameTest = $"Icon Web Tst0";
            var expectedTibcoServerNameTest1 = $"TIBCO Admin Tst0 1";
            var expectedTibcoServerNameTest2 = $"TIBCO Admin Tst0 2";
            var expectedMwsServerTest = $"http://irmatestapp1/MammothWebSupport";
            var expectedIconWebServerTest = $"http://icon-test/";
            var expectedTibcoServerTest1 = $"https://cerd1669:18090/";
            var expectedTibcoServerTest2 = $"https://cerd1670:18090/";
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForSupportApps(environment);
            // Assert
            Assert.IsNotNull(subMenu);
            var subSubMenuForEnv = subMenu.EnvironmentSubMenus.Single(m => m.EnvironmentEnum == environment);
            Assert.IsNotNull(subSubMenuForEnv);
            Assert.IsTrue(subSubMenuForEnv.SubMenuItems.Any(s => s.VisibleText.Equals(expectedMwsServerNameTest)));
            Assert.AreEqual(expectedMwsServerTest, subSubMenuForEnv.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedMwsServerNameTest)).Link);
            Assert.IsTrue(subSubMenuForEnv.SubMenuItems.Any(s => s.VisibleText.Equals(expectedIconWebServerNameTest)));
            Assert.AreEqual(expectedIconWebServerTest, subSubMenuForEnv.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedIconWebServerNameTest)).Link);
            Assert.IsTrue(subSubMenuForEnv.SubMenuItems.Any(s => s.VisibleText.Equals(expectedTibcoServerNameTest1)));
            Assert.AreEqual(expectedTibcoServerTest1, subSubMenuForEnv.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedTibcoServerNameTest1)).Link);
            Assert.IsTrue(subSubMenuForEnv.SubMenuItems.Any(s => s.VisibleText.Equals(expectedTibcoServerNameTest2)));
            Assert.AreEqual(expectedTibcoServerTest2, subSubMenuForEnv.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedTibcoServerNameTest2)).Link);
        }

        [TestMethod]
        public void BuildSubMenuForSupportApps_Perf_ShouldReturnsExpectedUrls()
        {
            // Arrange
            var environment = EnvironmentEnum.Perf;
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expectedMwsServerNamePerf = $"Mammoth Web Support Perf";
            var expectedIconWebServerNamePerf = $"Icon Web Perf";
            var expectedTibcoServerNamePerf1 = $"TIBCO Admin Perf 1";
            var expectedTibcoServerNamePerf2 = $"TIBCO Admin Perf 2";
            var expectedMwsServerPerf = $"http://irmaqaapp1/MammothWebSupportPerf";
            var expectedIconWebServerPerf = $"http://icon-perf/";
            var expectedTibcoServerPerf1 = $"https://cerd1666:28090/";
            var expectedTibcoServerPerf2 = $"https://cerd1667:28090/";
            string actual = string.Empty;
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForSupportApps(environment);
            // Assert
            Assert.IsNotNull(subMenu);
            var subSubMenuForEnv = subMenu.EnvironmentSubMenus.Single(m => m.EnvironmentEnum == environment);
            Assert.IsNotNull(subSubMenuForEnv);
            Assert.IsTrue(subSubMenuForEnv.SubMenuItems.Any(s => s.VisibleText.Equals(expectedMwsServerNamePerf)));
            Assert.AreEqual(expectedMwsServerPerf, subSubMenuForEnv.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedMwsServerNamePerf)).Link);
            Assert.IsTrue(subSubMenuForEnv.SubMenuItems.Any(s => s.VisibleText.Equals(expectedIconWebServerNamePerf)));
            Assert.AreEqual(expectedIconWebServerPerf, subSubMenuForEnv.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedIconWebServerNamePerf)).Link);
            Assert.IsTrue(subSubMenuForEnv.SubMenuItems.Any(s => s.VisibleText.Equals(expectedTibcoServerNamePerf1)));
            Assert.AreEqual(expectedTibcoServerPerf1, subSubMenuForEnv.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedTibcoServerNamePerf1)).Link);
            Assert.IsTrue(subSubMenuForEnv.SubMenuItems.Any(s => s.VisibleText.Equals(expectedTibcoServerNamePerf2)));
            Assert.AreEqual(expectedTibcoServerPerf2, subSubMenuForEnv.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedTibcoServerNamePerf2)).Link);
        }

        [TestMethod]
        public void BuildSubMenuForSupportApps_QA_ShouldReturnsExpectedUrls()
        {
            // Arrange
            var environment = EnvironmentEnum.QA;
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var expectedMwsServerNameQa = $"Mammoth Web Support QA";
            var expectedIconWebServerNameQa = $"Icon Web QA";
            var expectedTibcoServerNameQa1 = $"TIBCO Admin QA 1";
            var expectedTibcoServerNameQa2 = $"TIBCO Admin QA 2";
            var expectedMwsServerQa = $"http://irmaqaapp1/MammothWebSupport";
            var expectedIconWebServerQa = $"http://icon-qa/";
            var expectedTibcoServerQa1 = $"https://cerd1673:28090/";
            var expectedTibcoServerQa2 = $"https://cerd1674:28090/";
            string actual = string.Empty;
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForSupportApps(environment);
            // Assert
            Assert.IsNotNull(subMenu);
            var subSubMenuForEnv = subMenu.EnvironmentSubMenus.Single(m => m.EnvironmentEnum == environment);
            Assert.IsNotNull(subSubMenuForEnv);
            Assert.IsTrue(subSubMenuForEnv.SubMenuItems.Any(s => s.VisibleText.Equals(expectedMwsServerNameQa)));
            Assert.AreEqual(expectedMwsServerQa, subSubMenuForEnv.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedMwsServerNameQa)).Link);
            Assert.IsTrue(subSubMenuForEnv.SubMenuItems.Any(s => s.VisibleText.Equals(expectedIconWebServerNameQa)));
            Assert.AreEqual(expectedIconWebServerQa, subSubMenuForEnv.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedIconWebServerNameQa)).Link);
            Assert.IsTrue(subSubMenuForEnv.SubMenuItems.Any(s => s.VisibleText.Equals(expectedTibcoServerNameQa1)));
            Assert.AreEqual(expectedTibcoServerQa1, subSubMenuForEnv.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedTibcoServerNameQa1)).Link);
            Assert.IsTrue(subSubMenuForEnv.SubMenuItems.Any(s => s.VisibleText.Equals(expectedTibcoServerNameQa2)));
            Assert.AreEqual(expectedTibcoServerQa2, subSubMenuForEnv.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedTibcoServerNameQa2)).Link);
        }

        [TestMethod]
        public void BuildSubMenuForEnvironments_WithStandardEnvironmentsHostedInTst0_ShouldReturnExpectedRootItem()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForEnvironments("Home", "Index", null);
            // Assert
            Assert.IsNotNull(subMenu);
            Assert.AreEqual("Environment", subMenu.Header);
            Assert.AreEqual("Hosting Environment: Tst0", subMenu.TextForRootItem);
            Assert.AreEqual("Home", subMenu.ControllerForRootItem);
            Assert.AreEqual("SetAlternateEnvironment", subMenu.ActionForRootItem);
            Assert.AreEqual("primary", subMenu.RootItemTextBootstrapClass);
            Assert.AreEqual(true, subMenu.RootItemIsActive);
        }

        [TestMethod]
        public void BuildSubMenuForEnvironments_WithStandardEnvironmentsHostedInPerf_ShouldReturnExpectedRootItem()
        {
            // Arrange
            configTestData.ConfigDataModel.HostingEnvironmentSetting = EnvironmentEnum.Perf;
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForEnvironments("Home", "Index", null);
            // Assert
            Assert.IsNotNull(subMenu);
            Assert.AreEqual("Environment", subMenu.Header);
            Assert.AreEqual("Hosting Environment: Perf", subMenu.TextForRootItem);
            Assert.AreEqual("Home", subMenu.ControllerForRootItem);
            Assert.AreEqual("SetAlternateEnvironment", subMenu.ActionForRootItem);
            Assert.AreEqual("info", subMenu.RootItemTextBootstrapClass);
            Assert.AreEqual(true, subMenu.RootItemIsActive);
        }

        [TestMethod]
        public void BuildSubMenuForEnvironments_WithStandardEnvironmentsHostedInTst0_ShouldReturnExpectedSubItemCount()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            const int expectedEnvironmentCount = 7;
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForEnvironments("Home", "Index", null);
            // Assert
            Assert.IsNotNull(subMenu);
            Assert.IsNotNull(subMenu.Items);
            Assert.AreEqual(expectedEnvironmentCount, subMenu.Items.Count);
        }

        [TestMethod]
        public void BuildSubMenuForEnvironments_WithStandardEnvironmentsHostedInTst0_ShouldReturnExpectedSubItems()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForEnvironments("Home", "Index", null);
            // Assert
            Assert.IsNotNull(subMenu);
            Assert.IsNotNull(subMenu.Items);
            var subItem = subMenu.Items.Single(i => i.VisibleText.Equals(EnvironmentEnum.Dev0.ToString()));
            Assert.IsNotNull(subItem);
            Assert.AreEqual("Dev0", subItem.VisibleText);
            Assert.AreEqual("default", subItem.BoostrapTextClass);
            Assert.AreEqual(false, subItem.IsActiveListItem);
            dynamic anonRouteVal = subItem.RouteValuesForItemLink;
            Assert.AreEqual("Dev0", anonRouteVal.environment);
        }
        [TestMethod]
        public void BuildSubMenuForEnvironments_WithStandardEnvironmentsHostedInTst0WhenDev0IsActive_ShouldReturnExpectedSubItems()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var environmentCookieModel = new EnvironmentCookieModel
            {
                 Name = "Dev0",
                 EnvironmentEnum = EnvironmentEnum.Dev0,
                 AppServers = new List<string> { "irmadevapp1" }
            };
            dashboardDataManager.SetPermissionsForActiveEnvironment(true, environmentCookieModel);
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForEnvironments("Home", "Index", null);
            // Assert
            Assert.IsNotNull(subMenu);
            Assert.IsNotNull(subMenu.Items);
            var subItem = subMenu.Items.Single(i => i.VisibleText.Equals(EnvironmentEnum.Dev0.ToString()));
            Assert.IsNotNull(subItem);
            Assert.AreEqual("Dev0", subItem.VisibleText);
            Assert.AreEqual("default", subItem.BoostrapTextClass);
            Assert.AreEqual(true, subItem.IsActiveListItem);
            dynamic anonRouteVal = subItem.RouteValuesForItemLink;
            Assert.AreEqual("Dev0", anonRouteVal.environment);
        }

        [TestMethod]
        public void BuildSubMenuForEnvironments_WithStandardEnvironmentsHostedInTst1_ShouldReturnExpectedSubItemForQA()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForEnvironments("Home", "Index", null);
            // Assert
            Assert.IsNotNull(subMenu);
            Assert.IsNotNull(subMenu.Items);
            var subItem = subMenu.Items.Single(i => i.VisibleText.Equals(EnvironmentEnum.QA.ToString()));
            Assert.IsNotNull(subItem);
            Assert.AreEqual("QA", subItem.VisibleText);
            Assert.AreEqual("warning", subItem.BoostrapTextClass);
            Assert.AreEqual(false, subItem.IsActiveListItem);
            dynamic anonRouteVal = subItem.RouteValuesForItemLink;
            Assert.AreEqual("QA", anonRouteVal.environment);
        }

        [TestMethod]
        public void BuildSubMenuForIconAppLogs_WithStandardEnvironmentsHostedInTst0_ShouldReturnExpectedRootItem()
        {
            // Arrange
            var loggedApps = dbWrapperTestData.IconApps;
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForAppLogs("IconLogs", "Index", IconOrMammothEnum.Icon, loggedApps, null);
            // Assert
            Assert.IsNotNull(subMenu);
            Assert.AreEqual("Icon Log Viewer", subMenu.Header);
            Assert.AreEqual("All", subMenu.TextForRootItem);
            Assert.AreEqual("IconLogs", subMenu.ControllerForRootItem);
            Assert.AreEqual("Index", subMenu.ActionForRootItem);
            Assert.AreEqual("default", subMenu.RootItemTextBootstrapClass);
            Assert.AreEqual(true, subMenu.RootItemIsActive);
        }

        [TestMethod]
        public void BuildSubMenuForIconAppLogs_WithStandardEnvironmentsHostedInTst0_ShouldReturnExpectedSubItemForGloCon()
        {
            // Arrange
            var loggedApps = dbWrapperTestData.IconApps;
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForAppLogs("IconLogs", "Index", IconOrMammothEnum.Icon, loggedApps, null);
            // Assert
            Assert.IsNotNull(subMenu);
            Assert.IsNotNull(subMenu.Items);
            var subItem = subMenu.Items.Single(i => i.VisibleText.Equals("Global Controller"));
            Assert.IsNotNull(subItem);
            Assert.AreEqual("Global Controller", subItem.VisibleText);
            Assert.AreEqual("default", subItem.BoostrapTextClass);
            Assert.AreEqual(false, subItem.IsActiveListItem);
            dynamic anonRouteVal = subItem.RouteValuesForItemLink;
            Assert.AreEqual("Global Controller", anonRouteVal.appName);
        }

        [TestMethod]
        public void BuildSubMenuForMammothAppLogs_WithStandardEnvironmentsHostedInQA_ShouldReturnExpectedRootItem()
        {
            // Arrange
            var loggedApps = dbWrapperTestData.MammothApps;
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForAppLogs("MammothLogs", "Index", IconOrMammothEnum.Mammoth, loggedApps, null);
            // Assert
            Assert.IsNotNull(subMenu);
            Assert.AreEqual("Mammoth Log Viewer", subMenu.Header);
            Assert.AreEqual("All", subMenu.TextForRootItem);
            Assert.AreEqual("MammothLogs", subMenu.ControllerForRootItem);
            Assert.AreEqual("Index", subMenu.ActionForRootItem);
            Assert.AreEqual("default", subMenu.RootItemTextBootstrapClass);
            Assert.AreEqual(true, subMenu.RootItemIsActive);
        }

        [TestMethod]
        public void BuildSubMenuForMammothAppLogs_WithStandardEnvironmentsHostedInQA_ShouldReturnExpectedSubItemForPriceListener()
        {
            // Arrange
            var loggedApps = dbWrapperTestData.MammothApps;
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForAppLogs("MammothLogs", "Index", IconOrMammothEnum.Mammoth, loggedApps, null);
            // Assert
            Assert.IsNotNull(subMenu);
            Assert.IsNotNull(subMenu.Items);
            var subItem = subMenu.Items.Single(i => i.VisibleText.Equals("Price Listener"));
            Assert.IsNotNull(subItem);
            Assert.AreEqual("Price Listener", subItem.VisibleText);
            Assert.AreEqual("default", subItem.BoostrapTextClass);
            Assert.AreEqual(false, subItem.IsActiveListItem);
            dynamic anonRouteVal = subItem.RouteValuesForItemLink;
            Assert.AreEqual("Price Listener", anonRouteVal.appName);
        }

        [TestMethod]
        public void BuildSubMenuForIconApiJobs_WithStandardEnvironmentsHostedInDev0_ShouldReturnExpectedRootItem()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForApiJobs("ApiJobs", "Index", null);
            // Assert
            Assert.IsNotNull(subMenu);
            Assert.AreEqual("API Job Monitor", subMenu.Header);
            Assert.AreEqual("All", subMenu.TextForRootItem);
            Assert.AreEqual("ApiJobs", subMenu.ControllerForRootItem);
            Assert.AreEqual("Index", subMenu.ActionForRootItem);
            Assert.AreEqual("default", subMenu.RootItemTextBootstrapClass);
            Assert.AreEqual(true, subMenu.RootItemIsActive);
        }

        [TestMethod]
        public void BuildSubMenuForIconApiJobs_WithStandardEnvironmentsHostedInDev0_ShouldReturnExpectedSubItemForProduct()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForApiJobs("Home", "Index", null);
            // Assert
            Assert.IsNotNull(subMenu);
            Assert.IsNotNull(subMenu.Items);
            var subItem = subMenu.Items.Single(i => i.VisibleText.Equals(ApiJobMessageTypeEnum.Product.ToString()));
            Assert.IsNotNull(subItem);
            Assert.AreEqual("Product", subItem.VisibleText);
            Assert.AreEqual("default", subItem.BoostrapTextClass);
            Assert.AreEqual(false, subItem.IsActiveListItem);
            dynamic anonRouteVal = subItem.RouteValuesForItemLink;
            Assert.AreEqual("Product", anonRouteVal.jobType);
        }

        [TestMethod]
        public void BuildSubMenuForIconApiJobs_WithStandardEnvironmentsHostedInTst0_ShouldReturnExpectedSubItemForPending()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            // Act
            var subMenu = dashboardDataManager.BuildSubMenuForApiJobs("Home", "Index", null);
            // Assert
            Assert.IsNotNull(subMenu);
            Assert.IsNotNull(subMenu.Items);
            var subItem = subMenu.Items.Single(i => i.VisibleText.Equals("Pending"));
            Assert.IsNotNull(subItem);
            Assert.AreEqual("Pending", subItem.VisibleText);
            Assert.AreEqual("default", subItem.BoostrapTextClass);
            Assert.AreEqual(false, subItem.IsActiveListItem);
            dynamic anonRouteVal = subItem.RouteValuesForItemLink;
            Assert.AreEqual("Pending", anonRouteVal.jobType);
        }

        [TestMethod]
        public void BuildGlobalViewModel_SetsViewModelControllerAndActionNames()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "Blhome";
            var actionName = "Blindex";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            Assert.AreEqual(controllerName, globalViewModel.ControllerName);
            Assert.AreEqual(actionName, globalViewModel.ActionName);
        }

        [TestMethod]
        public void BuildGlobalViewModel_SetsSimpleConfigProperties()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "Home";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            Assert.AreEqual(
                configTestData.ConfigDataModel.ServiceCommandTimeout,
                globalViewModel.ServiceCommandTimeout);
            Assert.AreEqual(
                configTestData.ConfigDataModel.HoursForRecentErrors,
                globalViewModel.HoursForRecentErrors);
            Assert.AreEqual(
                configTestData.ConfigDataModel.MillisecondsForRecentErrorsPolling,
                globalViewModel.MillisecondsForRecentErrorsPolling);
        }

        [TestMethod]
        public void BuildGlobalViewModel_SetsHostingEnvironment()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "Home";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            Assert.IsNotNull(globalViewModel.HostingEnvironment);
            Assert.AreEqual(EnvironmentEnum.Tst0, globalViewModel.HostingEnvironment.EnvironmentEnum);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WhenHostingEnvironmentIsActive_SetsActiveEnvironment()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "Home";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            Assert.IsNotNull(globalViewModel.ActiveEnvironment);
            Assert.AreEqual(EnvironmentEnum.Tst0, globalViewModel.ActiveEnvironment.EnvironmentEnum);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WhenNonHostingEnvironmentIsActive_SetsActiveEnvironment()
        {
            // Arrange
            var activeEnvironmentEnum = EnvironmentEnum.Perf;
            var activeEnvironmentName = activeEnvironmentEnum.ToString();
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var cookieModel = new EnvironmentCookieModel(activeEnvironmentName, activeEnvironmentEnum);
            dashboardDataManager.SetPermissionsForActiveEnvironment(true, cookieModel);
            var controllerName = "Home";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            Assert.IsNotNull(globalViewModel.ActiveEnvironment);
            Assert.AreEqual(activeEnvironmentEnum, globalViewModel.ActiveEnvironment.EnvironmentEnum);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WhenUserCanEdit_SetsServiceCommandsAreEnabled_ToTrue()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "Home";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            Assert.IsTrue(globalViewModel.ServiceCommandsAreEnabled);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WhenUserCanEditButHostingEnvironmentIsProd_SetsServiceCommandsAreEnabled_ToFalse()
        {
            // Arrange
            configTestData.ConfigDataModel.HostingEnvironmentSetting = EnvironmentEnum.Prd;
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "Home";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            Assert.IsFalse(globalViewModel.ServiceCommandsAreEnabled);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WhenUserCanEditButActiveEnvironmentIsProd_SetsServiceCommandsAreEnabled_ToFalse()
        {
            // Arrange
            var activeEnvironmentEnum = EnvironmentEnum.Prd;
            var activeEnvironmentName = activeEnvironmentEnum.ToString();
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var cookieModel = new EnvironmentCookieModel(activeEnvironmentName, activeEnvironmentEnum);
            dashboardDataManager.SetPermissionsForActiveEnvironment(true, cookieModel);
            var controllerName = "Home";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            Assert.IsFalse(globalViewModel.ServiceCommandsAreEnabled);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WhenUserCanEditButActiveEnvironmentIsCustomWithProdServers_SetsServiceCommandsAreEnabled_ToFalse()
        {
            // Arrange
            var activeEnvironmentEnum = EnvironmentEnum.Custom;
            var activeEnvironmentName = activeEnvironmentEnum.ToString();
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var cookieModel = new EnvironmentCookieModel(activeEnvironmentName, activeEnvironmentEnum);
            cookieModel.AppServers = new List<string> { "vm-icon-qa3", "vm-icon-prd3" };
            dashboardDataManager.SetPermissionsForActiveEnvironment(true, cookieModel);
             var controllerName = "Home";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            Assert.IsFalse(globalViewModel.ServiceCommandsAreEnabled);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WhenUserCannotEdit_SetsServiceCommandsAreEnabled_ToFalse()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "Home";
            var actionName = "Index";
            var userCanEdit = false;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            Assert.IsFalse(globalViewModel.ServiceCommandsAreEnabled);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WithStandardEnvironmentsHostedInTst0_BuildsSubMenuForIconAppLogs()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "Home";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            var subMenu = globalViewModel.SubMenuForIconLogs;
            Assert.IsNotNull(subMenu);
            Assert.AreEqual("Icon Log Viewer", subMenu.Header);
            Assert.AreEqual("All", subMenu.TextForRootItem);
            Assert.AreEqual("IconLogs", subMenu.ControllerForRootItem);
            Assert.AreEqual("Index", subMenu.ActionForRootItem);
            Assert.AreEqual("default", subMenu.RootItemTextBootstrapClass);
            Assert.AreEqual(false, subMenu.RootItemIsActive);
            Assert.IsNotNull(subMenu.Items);
            var subItemForGlocon = subMenu.Items.Single(i => i.VisibleText.Equals("Global Controller"));
            Assert.IsNotNull(subItemForGlocon);
            Assert.AreEqual("Global Controller", subItemForGlocon.VisibleText);
            Assert.AreEqual("default", subItemForGlocon.BoostrapTextClass);
            Assert.AreEqual(false, subItemForGlocon.IsActiveListItem);
            dynamic anonRouteVal = subItemForGlocon.RouteValuesForItemLink;
            Assert.AreEqual("Global Controller", anonRouteVal.appName);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WithStandardEnvironmentsHostedInTst0_SubMenuForIconAppLogsWhenIconLogsIndexAction_HasExpectedActiveItem()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "IconLogs";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            var subMenu = globalViewModel.SubMenuForIconLogs;
            Assert.AreEqual(true, subMenu.RootItemIsActive);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WithStandardEnvironmentsHostedInTst0_SubMenuForIconAppLogsWhenIconLogsIndexActionForGlocon_HasExpectedActiveItem()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "IconLogs";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = "Global Controller";
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            var subMenu = globalViewModel.SubMenuForIconLogs;
            var subItemForGlocon = subMenu.Items.Single(i => i.VisibleText.Equals("Global Controller"));
            Assert.AreEqual(true, subItemForGlocon.IsActiveListItem);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WithStandardEnvironmentsHostedInTst0_BuildsSubMenuForMammothAppLogs()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "Home";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            var subMenu = globalViewModel.SubMenuForMammothLogs;
            Assert.AreEqual("Mammoth Log Viewer", subMenu.Header);
            Assert.AreEqual("All", subMenu.TextForRootItem);
            Assert.AreEqual("MammothLogs", subMenu.ControllerForRootItem);
            Assert.AreEqual("Index", subMenu.ActionForRootItem);
            Assert.AreEqual("default", subMenu.RootItemTextBootstrapClass);
            Assert.AreEqual(false, subMenu.RootItemIsActive);
            Assert.IsNotNull(subMenu.Items);
            var subItemForPriceListener = subMenu.Items.Single(i => i.VisibleText.Equals("Price Listener"));
            Assert.IsNotNull(subItemForPriceListener);
            Assert.AreEqual("Price Listener", subItemForPriceListener.VisibleText);
            Assert.AreEqual("default", subItemForPriceListener.BoostrapTextClass);
            Assert.AreEqual(false, subItemForPriceListener.IsActiveListItem);
            dynamic anonRouteVal = subItemForPriceListener.RouteValuesForItemLink;
            Assert.AreEqual("Price Listener", anonRouteVal.appName);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WithStandardEnvironmentsHostedInTst0_SubMenuForMammothAppLogsWhenMammothLogsIndexAction_HasExpectedActiveItem()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "MammothLogs";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            var subMenu = globalViewModel.SubMenuForMammothLogs;
            Assert.AreEqual(true, subMenu.RootItemIsActive);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WithStandardEnvironmentsHostedInTst0_SubMenuForMammothAppLogsWhenMammothLogsIndexActionForPriceListener_HasExpectedActiveItem()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "MammothLogs";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = "Price Listener";
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            var subMenu = globalViewModel.SubMenuForMammothLogs;
            var subMenuItemForPriceListener = subMenu.Items.Single(i => i.VisibleText.Equals("Price Listener"));
            Assert.AreEqual(true, subMenuItemForPriceListener.IsActiveListItem);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WithStandardEnvironmentsHostedInTst0_BuildsSubMenuForIconApiJobs()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "Home";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            var subMenu = globalViewModel.SubMenuForIconApiJobs;
            Assert.IsNotNull(subMenu);
            Assert.AreEqual("API Job Monitor", subMenu.Header);
            Assert.AreEqual("All", subMenu.TextForRootItem);
            Assert.AreEqual("ApiJobs", subMenu.ControllerForRootItem);
            Assert.AreEqual("Index", subMenu.ActionForRootItem);
            Assert.AreEqual("default", subMenu.RootItemTextBootstrapClass);
            Assert.AreEqual(false, subMenu.RootItemIsActive);
            Assert.IsNotNull(subMenu.Items);
            var subItemForProductJobs = subMenu.Items.Single(i => i.VisibleText.Equals(ApiJobMessageTypeEnum.Product.ToString()));
            Assert.IsNotNull(subItemForProductJobs);
            Assert.AreEqual("Product", subItemForProductJobs.VisibleText);
            Assert.AreEqual("default", subItemForProductJobs.BoostrapTextClass);
            Assert.AreEqual(false, subItemForProductJobs.IsActiveListItem);
            dynamic anonRouteVal1 = subItemForProductJobs.RouteValuesForItemLink;
            Assert.AreEqual("Product", anonRouteVal1.jobType);
            var subItemForPendingJobs = subMenu.Items.Single(i => i.VisibleText.Equals("Pending"));
            Assert.IsNotNull(subItemForPendingJobs);
            Assert.AreEqual("Pending", subItemForPendingJobs.VisibleText);
            Assert.AreEqual("default", subItemForPendingJobs.BoostrapTextClass);
            Assert.AreEqual(false, subItemForPendingJobs.IsActiveListItem);
            dynamic anonRouteVal2 = subItemForPendingJobs.RouteValuesForItemLink;
            Assert.AreEqual("Pending", anonRouteVal2.jobType);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WithStandardEnvironmentsHostedInTst0_SubMenuForApiJobsWhenApiJobsIndexAction_HasExpectedActiveItem()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "ApiJobs";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            var subMenu = globalViewModel.SubMenuForIconApiJobs;
            Assert.AreEqual(true, subMenu.RootItemIsActive);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WithStandardEnvironmentsHostedInTst0_SubMenuForApiJobssWhenApiJobsIndexActionForProduct_HasExpectedActiveItem()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "ApiJobs";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = "Product";
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            var subMenu = globalViewModel.SubMenuForIconApiJobs;
            var subItemForProductJobs = subMenu.Items.Single(i => i.VisibleText.Equals(ApiJobMessageTypeEnum.Product.ToString()));
            Assert.AreEqual(true, subItemForProductJobs.IsActiveListItem);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WithStandardEnvironmentsHostedInTst0_SubMenuForApiJobssWhenApiJobsIndexActionForPending_HasExpectedActiveItem()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "ApiJobs";
            var actionName = "Pending";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            var subMenu = globalViewModel.SubMenuForIconApiJobs;
            var subItemForPendingJobs = subMenu.Items.Single(i => i.VisibleText.Equals("Pending"));
            Assert.AreEqual(true, subItemForPendingJobs.IsActiveListItem);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WithStandardEnvironmentsHostedInTst0_BuildsSubMenuForEnvironments()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "Home";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            var subMenu = globalViewModel.SubMenuForEnvironments;
            Assert.IsNotNull(subMenu);
            Assert.AreEqual("Environment", subMenu.Header);
            Assert.AreEqual("Hosting Environment: Tst0", subMenu.TextForRootItem);
            Assert.AreEqual("Home", subMenu.ControllerForRootItem);
            Assert.AreEqual("SetAlternateEnvironment", subMenu.ActionForRootItem);
            Assert.AreEqual("primary", subMenu.RootItemTextBootstrapClass);
            Assert.AreEqual(true, subMenu.RootItemIsActive);
            Assert.IsNotNull(subMenu.Items);
            Assert.AreEqual(7, subMenu.Items.Count);
            var subItemForDev0 = subMenu.Items.Single(i => i.VisibleText.Equals(EnvironmentEnum.Dev0.ToString()));
            Assert.IsNotNull(subItemForDev0);
            Assert.AreEqual("Dev0", subItemForDev0.VisibleText);
            Assert.AreEqual("default", subItemForDev0.BoostrapTextClass);
            Assert.AreEqual(false, subItemForDev0.IsActiveListItem);
            dynamic anonRouteVal1 = subItemForDev0.RouteValuesForItemLink;
            Assert.AreEqual("Dev0", anonRouteVal1.environment);
            var subItemForQA = subMenu.Items.Single(i => i.VisibleText.Equals(EnvironmentEnum.QA.ToString()));
            Assert.IsNotNull(subItemForQA);
            Assert.AreEqual("QA", subItemForQA.VisibleText);
            Assert.AreEqual("warning", subItemForQA.BoostrapTextClass);
            Assert.AreEqual(false, subItemForQA.IsActiveListItem);
            dynamic anonRouteVal2 = subItemForQA.RouteValuesForItemLink;
            Assert.AreEqual("QA", anonRouteVal2.environment);
        }

        [TestMethod]
        public void BuildGlobalViewModel_WithStandardEnvironmentsHostedInTst0_BuildsSubMenuForSupportApps()
        {
            // Arrange
            var dashboardDataManager = new DashboardDataManager(configTestData.ConfigDataModel);
            var controllerName = "Home";
            var actionName = "Index";
            var userCanEdit = true;
            var iconServiceList = dbWrapperTestData.IconApps;
            var mammothServiceList = dbWrapperTestData.MammothApps;
            string queryParam = null;
            var expectedMwsServerNameDev = $"Mammoth Web Support Dev0";
            var expectedIconWebServerNameDev = $"Icon Web Dev0";
            var expectedTibcoServerNameDev = $"TIBCO Admin Dev0";
            var expectedMwsServerDev = $"http://irmadevapp1/MammothWebSupport";
            var expectedIconWebServerDev = $"http://icon-dev/";
            var expectedTibcoServerDev = $"https://cerd1668:8090/";
            var expectedMwsServerNameTest = $"Mammoth Web Support Tst0";
            var expectedIconWebServerNameTest = $"Icon Web Tst0";
            var expectedTibcoServerNameTest1 = $"TIBCO Admin Tst0 1";
            var expectedTibcoServerNameTest2 = $"TIBCO Admin Tst0 2";
            var expectedMwsServerTest = $"http://irmatestapp1/MammothWebSupport";
            var expectedIconWebServerTest = $"http://icon-test/";
            var expectedTibcoServerTest1 = $"https://cerd1669:18090/";
            var expectedTibcoServerTest2 = $"https://cerd1670:18090/";
            var expectedMwsServerNamePerf = $"Mammoth Web Support Perf";
            var expectedIconWebServerNamePerf = $"Icon Web Perf";
            var expectedTibcoServerNamePerf1 = $"TIBCO Admin Perf 1";
            var expectedTibcoServerNamePerf2 = $"TIBCO Admin Perf 2";
            var expectedMwsServerPerf = $"http://irmaqaapp1/MammothWebSupportPerf";
            var expectedIconWebServerPerf = $"http://icon-perf/";
            var expectedTibcoServerPerf1 = $"https://cerd1666:28090/";
            var expectedTibcoServerPerf2 = $"https://cerd1667:28090/";
            var expectedMwsServerNameQa = $"Mammoth Web Support QA";
            var expectedIconWebServerNameQa = $"Icon Web QA";
            var expectedTibcoServerNameQa1 = $"TIBCO Admin QA 1";
            var expectedTibcoServerNameQa2 = $"TIBCO Admin QA 2";
            var expectedMwsServerQa = $"http://irmaqaapp1/MammothWebSupport";
            var expectedIconWebServerQa = $"http://icon-qa/";
            var expectedTibcoServerQa1 = $"https://cerd1673:28090/";
            var expectedTibcoServerQa2 = $"https://cerd1674:28090/";
            // Act
            var globalViewModel = dashboardDataManager.BuildGlobalViewModel(
                controllerName,
                actionName,
                userCanEdit,
                iconServiceList,
                mammothServiceList,
                queryParam);
            // Assert
            var subMenu = globalViewModel.SubMenuForSupportApps;
            Assert.IsNotNull(subMenu);
            var subSubMenuForDev0 = subMenu.EnvironmentSubMenus.Single(m => m.EnvironmentEnum == EnvironmentEnum.Dev0);
            Assert.IsNotNull(subSubMenuForDev0);
            Assert.IsTrue(subSubMenuForDev0.SubMenuItems.Any(s => s.VisibleText.Equals(expectedMwsServerNameDev)));
            Assert.AreEqual(expectedMwsServerDev, subSubMenuForDev0.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedMwsServerNameDev)).Link);
            Assert.IsTrue(subSubMenuForDev0.SubMenuItems.Any(s => s.VisibleText.Equals(expectedIconWebServerNameDev)));
            Assert.AreEqual(expectedIconWebServerDev, subSubMenuForDev0.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedIconWebServerNameDev)).Link);
            Assert.IsTrue(subSubMenuForDev0.SubMenuItems.Any(s => s.VisibleText.Equals(expectedTibcoServerNameDev)));
            Assert.AreEqual(expectedTibcoServerDev, subSubMenuForDev0.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedTibcoServerNameDev)).Link);
            var subSubMenuForTst0 = subMenu.EnvironmentSubMenus.Single(m => m.EnvironmentEnum == EnvironmentEnum.Tst0);
            Assert.IsNotNull(subSubMenuForTst0);
            Assert.IsTrue(subSubMenuForTst0.SubMenuItems.Any(s => s.VisibleText.Equals(expectedMwsServerNameTest)));
            Assert.AreEqual(expectedMwsServerTest, subSubMenuForTst0.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedMwsServerNameTest)).Link);
            Assert.IsTrue(subSubMenuForTst0.SubMenuItems.Any(s => s.VisibleText.Equals(expectedIconWebServerNameTest)));
            Assert.AreEqual(expectedIconWebServerTest, subSubMenuForTst0.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedIconWebServerNameTest)).Link);
            Assert.IsTrue(subSubMenuForTst0.SubMenuItems.Any(s => s.VisibleText.Equals(expectedTibcoServerNameTest1)));
            Assert.AreEqual(expectedTibcoServerTest1, subSubMenuForTst0.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedTibcoServerNameTest1)).Link);
            Assert.IsTrue(subSubMenuForTst0.SubMenuItems.Any(s => s.VisibleText.Equals(expectedTibcoServerNameTest2)));
            Assert.AreEqual(expectedTibcoServerTest2, subSubMenuForTst0.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedTibcoServerNameTest2)).Link);
            var subSubMenuForPerf = subMenu.EnvironmentSubMenus.Single(m => m.EnvironmentEnum == EnvironmentEnum.Perf);
            Assert.IsNotNull(subSubMenuForPerf);
            Assert.IsTrue(subSubMenuForPerf.SubMenuItems.Any(s => s.VisibleText.Equals(expectedMwsServerNamePerf)));
            Assert.AreEqual(expectedMwsServerPerf, subSubMenuForPerf.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedMwsServerNamePerf)).Link);
            Assert.IsTrue(subSubMenuForPerf.SubMenuItems.Any(s => s.VisibleText.Equals(expectedIconWebServerNamePerf)));
            Assert.AreEqual(expectedIconWebServerPerf, subSubMenuForPerf.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedIconWebServerNamePerf)).Link);
            Assert.IsTrue(subSubMenuForPerf.SubMenuItems.Any(s => s.VisibleText.Equals(expectedTibcoServerNamePerf1)));
            Assert.AreEqual(expectedTibcoServerPerf1, subSubMenuForPerf.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedTibcoServerNamePerf1)).Link);
            Assert.IsTrue(subSubMenuForPerf.SubMenuItems.Any(s => s.VisibleText.Equals(expectedTibcoServerNamePerf2)));
            Assert.AreEqual(expectedTibcoServerPerf2, subSubMenuForPerf.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedTibcoServerNamePerf2)).Link);
            var subSubMenuForQA = subMenu.EnvironmentSubMenus.Single(m => m.EnvironmentEnum == EnvironmentEnum.QA);
            Assert.IsNotNull(subSubMenuForQA);
            Assert.IsTrue(subSubMenuForQA.SubMenuItems.Any(s => s.VisibleText.Equals(expectedMwsServerNameQa)));
            Assert.AreEqual(expectedMwsServerQa, subSubMenuForQA.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedMwsServerNameQa)).Link);
            Assert.IsTrue(subSubMenuForQA.SubMenuItems.Any(s => s.VisibleText.Equals(expectedIconWebServerNameQa)));
            Assert.AreEqual(expectedIconWebServerQa, subSubMenuForQA.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedIconWebServerNameQa)).Link);
            Assert.IsTrue(subSubMenuForQA.SubMenuItems.Any(s => s.VisibleText.Equals(expectedTibcoServerNameQa1)));
            Assert.AreEqual(expectedTibcoServerQa1, subSubMenuForQA.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedTibcoServerNameQa1)).Link);
            Assert.IsTrue(subSubMenuForQA.SubMenuItems.Any(s => s.VisibleText.Equals(expectedTibcoServerNameQa2)));
            Assert.AreEqual(expectedTibcoServerQa2, subSubMenuForQA.SubMenuItems.FirstOrDefault(s => s.VisibleText.Equals(expectedTibcoServerNameQa2)).Link);
        }
    }
}
