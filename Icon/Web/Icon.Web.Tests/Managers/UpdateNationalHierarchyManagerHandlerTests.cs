using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass]
    public class UpdateNationalHierarchyManagerHandlerTests
    {
        private UpdateNationalHierarchyManagerHandler managerHandler;
        private UpdateNationalHierarchyManager manager;

        private IconContext context;
        private Mock<ICommandHandler<UpdateNationalHierarchyCommand>> updateNationalHierarchyCommandHandler;
        private Mock<ICommandHandler<UpdateNationalHierarchyTraitsCommand>> updateNationalHierarchyClassTraitsCommandHandler;
        private Mock<ICommandHandler<AddVimEventCommand>> addVimHierarchyClassEventCommandHandler;
        private Mock<IQueryHandler<GetNationalClassByClassCodeParameters, List<HierarchyClass>>> getNationalClassQuery;
        private Mock<ICommandHandler<AddHierarchyClassMessageCommand>> addHierarchyClassMessageHandler;

        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();
            updateNationalHierarchyCommandHandler = new Mock<ICommandHandler<UpdateNationalHierarchyCommand>>();
            updateNationalHierarchyClassTraitsCommandHandler = new Mock<ICommandHandler<UpdateNationalHierarchyTraitsCommand>>();
            addVimHierarchyClassEventCommandHandler = new Mock<ICommandHandler<AddVimEventCommand>>();
            getNationalClassQuery = new Mock<IQueryHandler<GetNationalClassByClassCodeParameters, List<HierarchyClass>>>();
            addHierarchyClassMessageHandler = new Mock<ICommandHandler<AddHierarchyClassMessageCommand>>();

            manager = new UpdateNationalHierarchyManager
            {
                NationalClassCode = "12345",
                UserName = "test"
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            context.Dispose();
        }

        [TestMethod]
        public void UpdateNationalHierarchyManager_ShouldCallCommandHandlers()
        {
            //Given
            HierarchyClass hc = context.HierarchyClass.Add(new HierarchyClass
            {
                hierarchyClassName = "testHCName",
                hierarchyID = Hierarchies.National,
                hierarchyLevel = HierarchyLevels.NationalClass
            });
            context.SaveChanges();

            manager.NationalHierarchy = hc;
            getNationalClassQuery.Setup(m => m.Search(It.IsAny<GetNationalClassByClassCodeParameters>()))
                .Returns(new List<HierarchyClass>());
            CreateManagerHandler();

            //When
            managerHandler.Execute(manager);

            //Then
            updateNationalHierarchyCommandHandler.Verify(cm => cm.Execute(It.IsAny<UpdateNationalHierarchyCommand>()), Times.Once);
        }

        private void CreateManagerHandler()
        {
            managerHandler = new UpdateNationalHierarchyManagerHandler(
                context,
                getNationalClassQuery.Object,
                updateNationalHierarchyCommandHandler.Object,
                updateNationalHierarchyClassTraitsCommandHandler.Object,
                addVimHierarchyClassEventCommandHandler.Object,
                addHierarchyClassMessageHandler.Object,
                new Web.Common.AppSettings());
        }
    }
}
