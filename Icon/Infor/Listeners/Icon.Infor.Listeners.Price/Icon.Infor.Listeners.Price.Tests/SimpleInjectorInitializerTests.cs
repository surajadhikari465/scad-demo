using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.Tests
{
    [TestClass]
    public class SimpleInjectorInitializerTests
    {
        [TestMethod]
        public void SimpleInjectorInitializer_Initialize_ContainerVerifiesWithNoErrors()
        {
            var container = SimpleInjectorInitializer.CreateContainer();
            container.Verify();
        }
    }
}
