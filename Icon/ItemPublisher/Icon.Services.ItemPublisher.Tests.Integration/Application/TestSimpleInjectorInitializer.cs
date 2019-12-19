using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Services.ItemPublisher.Application.Tests
{
    [TestClass()]
    public class SimpleInjectorInitializerTests
    {
        [TestMethod]
        public void InitializeContainer_ContainerShouldBeVerifiedWIthoutError()
        {
            // Given.
            SimpleInjector.Container container = SimpleInjectorInitializer.InitializeContainer();

            // Then.
            container.Verify();
        }
    }
}