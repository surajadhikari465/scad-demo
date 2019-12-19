using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BulkItemUploadProcessor.Service.Tests
{
    [TestClass]
    public class SimpleInjectorInitializerTests
    {
        [TestMethod]
        public void SimpleInjectorInitializer_Initialize_ContainerVerifiesWithoutErrors()
        {
            var container = new SimpleInjectorInitializer().Initialize();
            container.Verify();
        }
    }
}
