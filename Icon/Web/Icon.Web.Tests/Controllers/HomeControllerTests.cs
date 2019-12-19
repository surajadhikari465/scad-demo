using Icon.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass] [Ignore]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_InitialPageLoad_ShouldReturnViewResult()
        {
            // Given.
            HomeController controller = new HomeController();

            // When.
            ViewResult result = controller.Index() as ViewResult;

            // Then.
            Assert.IsNotNull(result);
        }
    }
}
