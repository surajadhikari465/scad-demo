using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Commands;
using Icon.Common.DataAccess;
using Moq;
using Icon.Framework;
using AutoMapper;
using Icon.Web.Mvc.App_Start;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass]
    public class UpdateCertificationAgencyManagerHandlerTests
    {
        private UpdateCertificationAgencyManagerHandler managerHandler;
        private UpdateCertificationAgencyManager manager;
        private IconContext context;
        private Mock<ICommandHandler<UpdateCertificationAgencyCommand>> mockUpdateCertificationAgencyCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mockUpdateCertificationAgencyCommandHandler = new Mock<ICommandHandler<UpdateCertificationAgencyCommand>>();
            
            manager = new UpdateCertificationAgencyManager();
            managerHandler = new UpdateCertificationAgencyManagerHandler(this.context, mockUpdateCertificationAgencyCommandHandler.Object);
            AutoMapperWebConfiguration.Configure();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Mapper.Reset();
        }

        [TestMethod]
        public void UpdateCertificationAgencyManager_ValidManager_ShouldCallCommandHandler()
        {
            //When
            managerHandler.Execute(manager);

            //Then
            mockUpdateCertificationAgencyCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateCertificationAgencyCommand>()));
        }
    }
}
