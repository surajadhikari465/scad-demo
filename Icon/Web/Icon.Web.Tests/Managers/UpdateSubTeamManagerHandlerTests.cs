using AutoMapper;
using Icon.Framework;
using Icon.Web.Common;
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
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Common.DataAccess;

namespace Icon.Web.Tests.Unit.ManagerHandlers
{
    [TestClass]
    public class UpdateSubTeamManagerHandlerTests
    {
        private UpdateSubTeamManagerHandler managerHandler;
        private UpdateSubTeamManager manager;

        private IconContext context;
        private Mock<ICommandHandler<UpdateSubTeamCommand>> updateSubTeamCommandHandler;
        private Mock<ICommandHandler<AddHierarchyClassMessageCommand>> addHierarchyClassMessageCommandHandler;
        private Mock<ICommandHandler<AddProductMessagesBySubTeamCommand>> associatedItemsCommandHandler;
        private Mock<ICommandHandler<UpdateHierarchyClassTraitCommand>> updateHierarchyClassTraitHandler;
        private Mock<ICommandHandler<AddSubTeamEventsCommand>> addSubTeamEventsCommandHandler;
        private Mock<IQueryHandler<GetRegionalSettingsBySettingsKeyNameParameters, List<RegionalSettingsModel>>> getRegionalSettingsBySettingsKeyNameQuery;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            updateSubTeamCommandHandler = new Mock<ICommandHandler<UpdateSubTeamCommand>>();
            addHierarchyClassMessageCommandHandler = new Mock<ICommandHandler<AddHierarchyClassMessageCommand>>();
            associatedItemsCommandHandler = new Mock<ICommandHandler<AddProductMessagesBySubTeamCommand>>();
            updateHierarchyClassTraitHandler = new Mock<ICommandHandler<UpdateHierarchyClassTraitCommand>>();
            addSubTeamEventsCommandHandler = new Mock<ICommandHandler<AddSubTeamEventsCommand>>();
            getRegionalSettingsBySettingsKeyNameQuery = new Mock<IQueryHandler<GetRegionalSettingsBySettingsKeyNameParameters, System.Collections.Generic.List<RegionalSettingsModel>>>();

            manager = new UpdateSubTeamManager
                {
                    HierarchyClassId = 55,
                    HierarchyId = 56,
                    HierarchyLevel = HierarchyLevels.SubBrick,
                    HierarchyParentClassId = 57,
                    PeopleSoftNumber = "Test People Soft Number",
                    SubTeamName = "Test SubTeam Name",
                    PosDeptNumber = "123",
                    TeamName = "Test Team Name",
                    TeamNumber = "123"
                };

            AutoMapperWebConfiguration.Configure();
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
            Mapper.Reset();
        }

        [TestMethod]
        public void UpdateSubTeamManager_PeopleSoftChanged_ShouldCreateALocaleMessageAndMessagesForAssociatedProducts()
        {
            //Given
            updateSubTeamCommandHandler.Setup(cm => cm.Execute(It.Is<UpdateSubTeamCommand>(c =>
                c.HierarchyClassId == manager.HierarchyClassId &&
                c.HierarchyId == manager.HierarchyId &&
                c.HierarchyLevel == manager.HierarchyLevel &&
                c.HierarchyParentClassId == manager.HierarchyParentClassId &&
                !c.PeopleSoftChanged &&
                c.PeopleSoftNumber == manager.PeopleSoftNumber &&
                c.UpdatedHierarchyClass == null)))
                .Callback<UpdateSubTeamCommand>(c => 
                    { 
                        c.PeopleSoftChanged = true;
                        c.UpdatedHierarchyClass = new HierarchyClass
                        {
                            hierarchyClassID = manager.HierarchyClassId,
                            hierarchyID = manager.HierarchyId,
                            hierarchyClassName = "Test Hierarchy Class Name"
                        };
                    })
                .Verifiable();
            CreateManagerHandler();

            //When
            managerHandler.Execute(manager);

            //Then
            updateSubTeamCommandHandler.Verify();
            addHierarchyClassMessageCommandHandler.Verify(cm => cm.Execute(It.Is<AddHierarchyClassMessageCommand>(c =>
                c.ClassNameChange == true &&
                c.HierarchyClass.hierarchyClassName == "Test Hierarchy Class Name" &&
                c.HierarchyClass.hierarchyClassID == manager.HierarchyClassId &&
                c.HierarchyClass.hierarchyID == manager.HierarchyId)), Times.Once);
            associatedItemsCommandHandler.Verify(cm => cm.Execute(It.Is<AddProductMessagesBySubTeamCommand>(c =>
                c.NewSubTeam == "Test Hierarchy Class Name")), Times.Once);
        }

        [TestMethod]
        public void UpdateSubTeamManager_PeopleSoftNotChanged_ShouldNotCreateALocaleMessageAndMessagesForAssociatedProducts()
        {
            //Given
            updateSubTeamCommandHandler.Setup(cm => cm.Execute(It.IsAny<UpdateSubTeamCommand>()))
                .Callback<UpdateSubTeamCommand>(c => c.PeopleSoftChanged = false);
            CreateManagerHandler();

            //When
            managerHandler.Execute(manager);

            //Then
            updateSubTeamCommandHandler.Verify(cm => cm.Execute(It.IsAny<UpdateSubTeamCommand>()), Times.Once);
            addHierarchyClassMessageCommandHandler.Verify(cm => cm.Execute(It.IsAny<AddHierarchyClassMessageCommand>()), Times.Never);
            associatedItemsCommandHandler.Verify(cm => cm.Execute(It.IsAny<AddProductMessagesBySubTeamCommand>()), Times.Never);
        }

        [TestMethod]
        public void UpdateSubTeamManager_CommandHandlerThrowsException_ShouldThrowExceptionWithCustomMessage()
        {
            //Given
            updateSubTeamCommandHandler.Setup(cm => cm.Execute(It.IsAny<UpdateSubTeamCommand>()))
                .Throws(new Exception("There was an error updating subteam"));
            CreateManagerHandler();

            //When
            try
            {
                managerHandler.Execute(manager);
            }
            catch (CommandException e)
            {
                //Then
                Assert.IsTrue(e.Message.StartsWith("There was an error updating subteam"));
            }
        }


        private void CreateManagerHandler()
        {
            managerHandler = new UpdateSubTeamManagerHandler(context,
                updateSubTeamCommandHandler.Object,
                addHierarchyClassMessageCommandHandler.Object,
                associatedItemsCommandHandler.Object,
                updateHierarchyClassTraitHandler.Object,
                addSubTeamEventsCommandHandler.Object,
                getRegionalSettingsBySettingsKeyNameQuery.Object);
        }
    }
}
