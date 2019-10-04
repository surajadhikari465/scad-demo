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
    public class SubMenuViewModelUnitTests
    {
        [TestMethod]
        public void Constructor_InitializesItemsList()
        {
            // Arrange
            // Act
            var viewModel = new SubMenuViewModel();
            // Assert
            Assert.IsNotNull(viewModel.Items);
        }

        [TestMethod]
        public void Constructor_SetsRootItemTextBootstrapClassToExpectedDefaultValue()
        {
            // Arrange
            // Act
            var viewModel = new SubMenuViewModel();
            // Assert
            Assert.AreEqual("default", viewModel.RootItemTextBootstrapClass);
        }
    }
}
