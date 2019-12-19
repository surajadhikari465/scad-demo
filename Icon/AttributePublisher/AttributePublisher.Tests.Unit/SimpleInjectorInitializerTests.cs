using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AttributePublisher.Tests.Unit
{
    [TestClass]
    public class SimpleInjectorInitializerTests
    {
        [TestMethod]
        public void SimpleInjectorInitializer_Initialize_ContainerIsVerified()
        {
            //When
            var container = SimpleInjectorInitializer.Initialize();

            //Then
            container.Verify();
        }
    }
}
