using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.UnitTests.TestData;
using Icon.Dashboard.Mvc.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ViewModelTests
{
    [TestClass]
    public class ServiceViewModelUnitTests
    {
        RemoteServiceTestData serviceTestData = new RemoteServiceTestData();

        [TestMethod]
        public void EsbEnvironmentEnum_WhenNoEsbConnection_ReturnsNone()
        {
            // Arrange
            var viewModel = serviceTestData.Services.GloconViewModel;
            var expectedEsbEnvironmentEnum = EsbEnvironmentEnum.None;
            // Act
            var esbConnectionType = viewModel.EsbEnvironmentEnum;
            // Assert
            Assert.AreEqual(expectedEsbEnvironmentEnum, esbConnectionType);
        }

        [TestMethod]
        public void EsbEnvironmentEnum_WhenIconEsbConnectionToTst_ReturnsTEST()
        {
            // Arrange
            var viewModel = serviceTestData.Services.IconR10ListenerViewModel;
            var expectedEsbEnvironmentEnum = EsbEnvironmentEnum.TEST;
            // Act
            var esbConnectionType = viewModel.EsbEnvironmentEnum;
            // Assert
            Assert.AreEqual(expectedEsbEnvironmentEnum, esbConnectionType);
        }

        [TestMethod]
        public void EsbEnvironmentEnum_WhenMammothEsbConnectionToQA_ReturnsQAFunc()
        {
            // Arrange
            var viewModel = serviceTestData.Services.MammothItemLocaleControllerViewModel;
            var expectedEsbEnvironmentEnum = EsbEnvironmentEnum.QA_FUNC;
            // Act
            var esbConnectionType = viewModel.EsbEnvironmentEnum;
            // Assert
            Assert.AreEqual(expectedEsbEnvironmentEnum, esbConnectionType);
        }

        [TestMethod]
        public void EsbEnvironmentEnum_WhenMultipleMammothEsbConnectionsToTST_ReturnsTEST()
        {
            // Arrange
            var viewModel = serviceTestData.Services.MammothProductListenerViewModel;
            var expectedEsbEnvironmentEnum = EsbEnvironmentEnum.TEST;
            // Act
            var esbConnectionType = viewModel.EsbEnvironmentEnum;
            // Assert
            Assert.AreEqual(expectedEsbEnvironmentEnum, esbConnectionType);
        }
    }
}
