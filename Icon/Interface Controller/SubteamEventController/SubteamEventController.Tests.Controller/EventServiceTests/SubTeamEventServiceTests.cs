using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Testing.Builders;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SubteamEventController.Controller.EventServices;
using SubteamEventController.DataAccess.Commands;
using SubteamEventController.DataAccess.Queries;

namespace SubteamEventController.Tests.Controller
{
    [TestClass]
    public class SubTeamEventServiceTests
    {
        private IrmaContext irmaContext;
        private Mock<IQueryHandler<GetSubTeamParameters, HierarchyClass>> getSubTeamQuery;
        private Mock<ICommandHandler<UpdateSubTeamCommand>> updateSubTeamCommandHandler;
        

        private SubTeamEventService eventService;


        [TestInitialize]
        public void InitializeData()
        {
            irmaContext = new IrmaContext();
            getSubTeamQuery = new Mock<IQueryHandler<GetSubTeamParameters, HierarchyClass>>();
            updateSubTeamCommandHandler = new Mock<ICommandHandler<UpdateSubTeamCommand>>();
            eventService = new SubTeamEventService(irmaContext, getSubTeamQuery.Object, updateSubTeamCommandHandler.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            irmaContext.Dispose();
          
        }

        [TestMethod]
        public void SubTeamEventService_HandlersCalledOneTime()
        {
            //Given
            eventService.Message = "ItemTestScanCode";
            eventService.ReferenceId = 1;
            getSubTeamQuery.Setup(h => h.Search(It.IsAny<GetSubTeamParameters>())).Returns(GetTestHierarchyClass());
            updateSubTeamCommandHandler.Setup(h => h.Execute(It.IsAny<UpdateSubTeamCommand>()));

            //When
            eventService.Run();

            //Then           
            getSubTeamQuery.Verify(command => command.Search(It.IsAny<GetSubTeamParameters>()), Times.Once);
            updateSubTeamCommandHandler.Verify(command => command.Execute(It.IsAny<UpdateSubTeamCommand>()), Times.Once);
        }

        private HierarchyClass GetTestHierarchyClass()
        {

            TestHierarchyClassBuilder testHierArchyClassBuilder = new TestHierarchyClassBuilder().WithHierarchyClassId(3).WithHierarchyClassName("TestFinancial(100)").WithPosDeptNumberTrait("100")
                .WithTeamNameTrait("Test Team Name").WithTeamNumberTrait("100");
            return testHierArchyClassBuilder;
        }
    }
}
