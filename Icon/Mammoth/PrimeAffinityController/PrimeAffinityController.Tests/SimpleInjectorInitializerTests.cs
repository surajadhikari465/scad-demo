using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PrimeAffinityController.Tests
{
    [TestClass]
    public class SimpleInjectorInitializerTests
    {
        [TestMethod]
        public void CreateContainer_ContainerShouldSuccessfullyVerify()
        {
            //When
            var container = SimpleInjectorInitializer.CreateContainer();

            //Then
            container.Verify();
        }
    }
}
