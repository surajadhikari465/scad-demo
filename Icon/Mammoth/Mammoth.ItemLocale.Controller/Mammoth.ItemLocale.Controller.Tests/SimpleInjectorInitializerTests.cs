using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ItemLocale.Controller.Tests
{
    [TestClass]
    public class SimpleInjectorInitializerTests
    {
        [TestMethod]
        public void InitializeSimpleInjectorContainer_ProgramIsNotInitialized_ShouldVerifyContentsAndNotThrowException()
        {
            //When
            var container = SimpleInjectorInitializer.InitializeContainer();
            container.Verify();

            //Then 
            Assert.IsNotNull(container);
        }
    }
}
