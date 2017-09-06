namespace Icon.Dashboard.DataFileAccess.Tests.Models
{
    using DataFileAccess.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    [TestClass]
    public class WindowsServiceModelTests
    {
        [TestMethod]
        public void WhenCreatingWindowsServiceModel_ThenConstructor_ShouldSetApplicationType()
        {
            // Given

            // When
            var winService = new IconService();

            // Then
            Assert.IsNotNull(winService);
            Assert.AreEqual(ApplicationTypeEnum.WindowsService, winService.TypeOfApplication);
        }
    }
}
