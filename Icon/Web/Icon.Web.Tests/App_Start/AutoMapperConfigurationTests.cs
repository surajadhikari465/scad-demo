using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Web.Tests.Unit.App_Start
{
    [TestClass]
    public class AutoMapperConfigurationTests
    {
        //[Ignore("TODO: PBI-39840 - Fix Icon Unit and Integration tests in Icon Web")]
        [TestMethod]
        public void Configure_IsCalled_ConfigurationIsValid()
        {
            //When
            AutoMapperWebConfiguration.Configure().ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
