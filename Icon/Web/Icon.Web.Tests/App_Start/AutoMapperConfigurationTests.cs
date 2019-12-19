using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Web.Tests.Unit.App_Start
{
    [TestClass]
    public class AutoMapperConfigurationTests
    {
        [TestMethod]
        public void Configure_IsCalled_ConfigurationIsValid()
        {
            //When
            AutoMapperWebConfiguration.Configure().ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
