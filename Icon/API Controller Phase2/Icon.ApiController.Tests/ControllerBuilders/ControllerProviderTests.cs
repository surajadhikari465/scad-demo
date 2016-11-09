using Icon.ApiController.Common;
using Icon.ApiController.Controller.ControllerBuilders;
using Icon.RenewableContext;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.ApiController.Tests.ControllerBuilders
{
    [TestClass]
    public class ControllerProviderTests
    {
        private IRenewableContext<IconContext> globalContext = new GlobalIconContext(new IconContext());

        [TestMethod]
        public void ControllerProvider_ProductControllerType_ReturnsProductQueueProcessor()
        {
            // Given.
            string controllerType = "p";

            // When.
            var controller = ControllerProvider.GetController(controllerType, globalContext);

            // Then.
            Assert.AreEqual("Product", ControllerType.Type);
        }

        [TestMethod]
        public void ControllerProvider_ItemLocaleControllerType_ReturnsItemLocaleQueueProcessor()
        {
            // Given.
            string controllerType = "i";

            // When.
            var controller = ControllerProvider.GetController(controllerType, globalContext);

            // Then.
            Assert.AreEqual("ItemLocale", ControllerType.Type);
        }

        [TestMethod]
        public void ControllerProvider_PriceControllerType_ReturnsPriceQueueProcessor()
        {
            // Given.
            string controllerType = "r";

            // When.
            var controller = ControllerProvider.GetController(controllerType, globalContext);

            // Then.
            Assert.AreEqual("Price", ControllerType.Type);
        }

        [TestMethod]
        public void ControllerProvider_HierarchyControllerType_ReturnsHierarchyQueueProcessor()
        {
            // Given.
            string controllerType = "h";

            // When.
            var controller = ControllerProvider.GetController(controllerType, globalContext);

            // Then.
            Assert.AreEqual("Hierarchy", ControllerType.Type);
        }

        [TestMethod]
        public void ControllerProvider_LocaleControllerType_ReturnsLocaleQueueProcessor()
        {
            // Given.
            string controllerType = "l";

            // When.
            var controller = ControllerProvider.GetController(controllerType, globalContext);

            // Then.
            Assert.AreEqual("Locale", ControllerType.Type);
        }

        [TestMethod]
        public void ControllerProvider_InvalidControllerType_ControllerShouldBeNull()
        {
            // Given.
            string controllerType = "z";

            // When.
            var controller = ControllerProvider.GetController(controllerType, globalContext);

            // Then.
            Assert.IsNull(controller);
        }
    }
}
