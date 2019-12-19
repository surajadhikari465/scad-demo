using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Managers;
using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass]
    public class DeleteNationalHierarchyManagerHandlerTests
    {
        private DeleteNationalHierarchyManagerHandler managerHandler;
        private DeleteNationalHierarchyManager manager;

        private IconContext context;
        private Mock<ICommandHandler<DeleteHierarchyClassCommand>> deleteHierarchyClassCommandHandler;
        private Mock<ICommandHandler<AddHierarchyClassMessageCommand>> addHierarchyClassMessageCommandHandler;
        private IMapper mapper;

        [TestInitialize]
        public void Initialize()
        {
            this.context = new IconContext();
            this.deleteHierarchyClassCommandHandler = new Mock<ICommandHandler<DeleteHierarchyClassCommand>>();
            this.addHierarchyClassMessageCommandHandler = new Mock<ICommandHandler<AddHierarchyClassMessageCommand>>();
            mapper = AutoMapperWebConfiguration.Configure();

            manager = new DeleteNationalHierarchyManager
            {
                DeletedHierarchyClass = new TestHierarchyClassBuilder()
            };
            managerHandler = new DeleteNationalHierarchyManagerHandler(
                context,
                deleteHierarchyClassCommandHandler.Object,
                addHierarchyClassMessageCommandHandler.Object,
                mapper);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }

        [TestMethod]
        public void DeleteNationHiearchyManger_ShouldCallCommandHandlers()
        {
            //When
            managerHandler.Execute(manager);

            //Then
            deleteHierarchyClassCommandHandler.Verify(cm => cm.Execute(It.IsAny<DeleteHierarchyClassCommand>()), Times.Once);
            addHierarchyClassMessageCommandHandler.Verify(cm => cm.Execute(It.IsAny<AddHierarchyClassMessageCommand>()), Times.Once);
        }
    }
}
