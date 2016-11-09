using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infor.Services.NewItem.Tests
{
    [TestClass]
    public class SimpleInjectorInitializerTests
    {
        [TestMethod]
        public void InitializeContainer_ContainerShouldVerifySuccessfully()
        {
            SimpleInjectorInitializer.InitializeContainer().Verify();
        }
    }
}
