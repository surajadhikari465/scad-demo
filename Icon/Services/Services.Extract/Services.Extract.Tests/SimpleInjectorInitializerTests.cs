using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Services.Extract.Tests
{
    [TestClass]
    public class SimpleInjectorInitializerTests
    {
        [TestMethod]
        public void SimpleInjector_Initialize_ShouldVerify()
        {
            var container = SimpleInjectorInitializer.Init();
            container.Verify();
        }
    }
}
