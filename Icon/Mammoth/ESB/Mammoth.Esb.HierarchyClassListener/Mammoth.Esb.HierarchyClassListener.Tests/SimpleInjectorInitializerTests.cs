using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mammoth.Esb.HierarchyClassListener.Tests
{
    [TestClass]
    public class SimpleInjectorInitializerTests
    {
        [TestMethod]
        public void Initialize_CallingVerify_ShouldNotThrowException()
        {
            //When
            var container = SimpleInjectorInitializer.Initialize();

            //Then
            container.Verify();
        }
    }
}
