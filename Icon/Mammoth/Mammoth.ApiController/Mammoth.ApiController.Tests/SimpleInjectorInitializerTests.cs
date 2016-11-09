using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mammoth.ApiController.Tests
{
    [TestClass]
    public class SimpleInjectorInitializerTests
    {
        [TestMethod]
        public void Initialize_ShouldNotThrowError()
        {
            //When
            var container = SimpleInjectorInitializer.InitializeContainer(1, "i");

            //Then
            container.Verify();
        }
    }
}
