using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Managers;
using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass]
    public class AddCertificationAgencyManagerHandlerTests
    {
        private AddCertificationAgencyManagerHandler managerHandler;
        private AddCertificationAgencyManager manager;
        private IconContext context;
        private Mock<ICommandHandler<AddCertificationAgencyCommand>> mockAddCertificationAgencyCommandHandler;
        private IMapper mapper;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mapper = AutoMapperWebConfiguration.Configure();
            mockAddCertificationAgencyCommandHandler = new Mock<ICommandHandler<AddCertificationAgencyCommand>>();
            
            manager = new AddCertificationAgencyManager();
            managerHandler = new AddCertificationAgencyManagerHandler(
                this.context, 
                mockAddCertificationAgencyCommandHandler.Object, 
                mapper);
        }

        [TestMethod]
        public void AddCertificationAgencyManager_ValidManager_ShouldCallCommandHandler()
        {
            //When
            managerHandler.Execute(manager);

            //Then
            mockAddCertificationAgencyCommandHandler.Verify(m => m.Execute(It.IsAny<AddCertificationAgencyCommand>()));
        }
    }
}
