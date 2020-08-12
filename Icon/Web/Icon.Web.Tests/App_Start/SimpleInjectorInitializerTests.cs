using AutoMapper;
using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Icon.Web.Tests.Unit.App_Start
{
    [TestClass]
    public class SimpleInjectorInitializerTests
    {
        [TestMethod]
        public void Initialize_AllTypesAreRegistered_ShouldVerifyContainer()
        {
            Mock<IMapper> mapper = new Mock<IMapper>();
            // When.
            SimpleInjectorInitializer.Initialize(mapper.Object);

            // Then no exception should be thrown.
        }
    }
}
