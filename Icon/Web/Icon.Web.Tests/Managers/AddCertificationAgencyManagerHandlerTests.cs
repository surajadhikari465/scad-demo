using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass] [Ignore]
    public class AddCertificationAgencyManagerHandlerTests
    {
        private AddCertificationAgencyManagerHandler managerHandler;
        private AddCertificationAgencyManager manager;
        private IconContext context;
        private Mock<ICommandHandler<AddCertificationAgencyCommand>> mockAddCertificationAgencyCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mockAddCertificationAgencyCommandHandler = new Mock<ICommandHandler<AddCertificationAgencyCommand>>();
            
            manager = new AddCertificationAgencyManager();
            managerHandler = new AddCertificationAgencyManagerHandler(this.context, mockAddCertificationAgencyCommandHandler.Object);
            AutoMapperWebConfiguration.Configure();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Mapper.Reset();
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
