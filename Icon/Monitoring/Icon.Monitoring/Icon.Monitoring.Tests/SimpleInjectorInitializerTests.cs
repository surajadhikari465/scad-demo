using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Monitoring.Tests
{
    [TestClass]
    public class SimpleInjectorInitializerTests
    {
        [TestMethod]
        public void Initialize_ValidConfiguration_ShouldNotThrowException()
        {
            //When
            SimpleInjectorInitializer.InitializeContainer();
        }
    }
}
