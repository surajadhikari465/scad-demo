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
    public class EsbEnvironmentViewModelUnitTests
    {
        ConfigTestData configTestData = new ConfigTestData();

        [TestMethod]
        public void ConstructorFromElement_WhenElementWithEsbDev_ShouldSetEsbEnvironmentEnum()
        {
            // Arrange
            var esbElement = configTestData.Elements.EsbDev;
            // Act
            var viewModel = new EsbEnvironmentViewModel(esbElement);
            // Asssert
            Assert.AreEqual(EsbEnvironmentEnum.DEV, viewModel.EsbEnvironment);
        }

        [TestMethod]
        public void ConstructorFromElement_WhenElementWithEsbTest_ShouldSetEsbEnvironmentEnum()
        {
            // Arrange
            var esbElement = configTestData.Elements.EsbTst;
            // Act
            var viewModel = new EsbEnvironmentViewModel(esbElement);
            // Asssert
            Assert.AreEqual(EsbEnvironmentEnum.TEST, viewModel.EsbEnvironment);
        }

        [TestMethod]
        public void ConstructorFromElement_WhenElementWithEsbTestDup_ShouldSetEsbEnvironmentEnum()
        {
            // Arrange
            var esbElement = configTestData.Elements.EsbTstDup;
            // Act
            var viewModel = new EsbEnvironmentViewModel(esbElement);
            // Asssert
            Assert.AreEqual(EsbEnvironmentEnum.TEST_DUP, viewModel.EsbEnvironment);
        }

        [TestMethod]
        public void ConstructorFromElement_WhenElementWithEsbQaDup_ShouldSetEsbEnvironmentEnum()
        {
            // Arrange
            var esbElement = configTestData.Elements.EsbQaDup;
            // Act
            var viewModel = new EsbEnvironmentViewModel(esbElement);
            // Asssert
            Assert.AreEqual(EsbEnvironmentEnum.QA_DUP, viewModel.EsbEnvironment);
        }

        [TestMethod]
        public void ConstructorFromElement_WhenElementWithEsbQaFunc_ShouldSetEsbEnvironmentEnum()
        {
            // Arrange
            var esbElement = configTestData.Elements.EsbQaFunc;
            // Act
            var viewModel = new EsbEnvironmentViewModel(esbElement);
            // Asssert
            Assert.AreEqual(EsbEnvironmentEnum.QA_FUNC, viewModel.EsbEnvironment);
        }

        [TestMethod]
        public void ConstructorFromElement_WhenElementWithEsbQaPerf_ShouldSetEsbEnvironmentEnum()
        {
            // Arrange
            var esbElement = configTestData.Elements.EsbQaPerf;
            // Act
            var viewModel = new EsbEnvironmentViewModel(esbElement);
            // Asssert
            Assert.AreEqual(EsbEnvironmentEnum.QA_PERF, viewModel.EsbEnvironment);
        }

        [TestMethod]
        public void ConstructorFromElement_WhenElementWithEsbProd_ShouldSetEsbEnvironmentEnum()
        {
            // Arrange
            var esbElement = configTestData.Elements.EsbPrd;
            // Act
            var viewModel = new EsbEnvironmentViewModel(esbElement);
            // Asssert
            Assert.AreEqual(EsbEnvironmentEnum.PRD, viewModel.EsbEnvironment);
        }
    }
}
