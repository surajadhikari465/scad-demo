using Icon.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_InitialPageLoad_ShouldReturnReditectResult()
        {
            // Given.
            HomeController controller = new HomeController();

            // When.
            var result = controller.Index() as RedirectToRouteResult;

            // Then.
            Assert.IsNotNull(result);
        }
    }
}
