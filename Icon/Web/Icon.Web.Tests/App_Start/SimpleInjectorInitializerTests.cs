using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Web.Tests.Unit.App_Start
{
    [TestClass]
    public class SimpleInjectorInitializerTests
    {
        [TestMethod]
        public void Initialize_AllTypesAreRegistered_ShouldVerifyContainer()
        {
            // When.
            SimpleInjectorInitializer.Initialize();

            // Then no exception should be thrown.
        }
    }
}
