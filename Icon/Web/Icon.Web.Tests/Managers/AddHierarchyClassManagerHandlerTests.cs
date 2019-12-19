using AutoMapper;
using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using Icon.Web.DataAccess.Queries;
using Icon.Common.DataAccess;

namespace Icon.Web.Tests.Unit.ManagerHandlers
{
    [TestClass]
    public class AddHierarchyClassManagerHandlerTests
    {
        private AddHierarchyClassManagerHandler managerHandler;
        private AddHierarchyClassManager manager;

        private IconContext context;
        private IMapper mapper;
        private Mock<ICommandHandler<AddHierarchyClassCommand>> addHierarchyClassCommandHandler;
        private Mock<ICommandHandler<AddHierarchyClassMessageCommand>> addHierarchyClassMessageCommandHandler;
        private Mock<ICommandHandler<AddSubTeamEventsCommand>> addSubTeamCommandHandler;
        private Mock<IQueryHandler<GetRegionalSettingsBySettingsKeyNameParameters, List<RegionalSettingsModel>>> getRegionalSettingsBySettingsKeyNameQuery;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mapper = AutoMapperWebConfiguration.Configure();
            addHierarchyClassCommandHandler = new Mock<ICommandHandler<AddHierarchyClassCommand>>();
            addHierarchyClassMessageCommandHandler = new Mock<ICommandHandler<AddHierarchyClassMessageCommand>>();
            addSubTeamCommandHandler = new Mock<ICommandHandler<AddSubTeamEventsCommand>>();
            getRegionalSettingsBySettingsKeyNameQuery = new Mock<IQueryHandler<GetRegionalSettingsBySettingsKeyNameParameters, List<RegionalSettingsModel>>>();

            manager = new AddHierarchyClassManager
            {
                NewHierarchyClass = new TestHierarchyClassBuilder(),
                NonMerchandiseTrait = "TestNonMerchandiseTrait",
                SubTeamHierarchyClassId = 5,
                TaxAbbreviation = "TestTaxAbbreviation",
                PosDeptNumber = "123",
                TeamName = "Team Name",
                TeamNumber = "123"
            };

            managerHandler = new AddHierarchyClassManagerHandler(
                context,
                addHierarchyClassCommandHandler.Object,
                addHierarchyClassMessageCommandHandler.Object,
                addSubTeamCommandHandler.Object,
                getRegionalSettingsBySettingsKeyNameQuery.Object,
                mapper);

        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }

        [TestMethod]
        public void AddHierarchyClassManager_ShouldCallCommandHandlers()
        {
            //Given
            AddHierarchyClassCommand command = null;
            AddHierarchyClassMessageCommand messageCommand = null;
            addHierarchyClassCommandHandler.Setup(cm => cm.Execute(It.IsAny<AddHierarchyClassCommand>()))
                .Callback<AddHierarchyClassCommand>(c => command = c);
            addHierarchyClassMessageCommandHandler.Setup(cm => cm.Execute(It.IsAny<AddHierarchyClassMessageCommand>()))
                .Callback<AddHierarchyClassMessageCommand>(c => messageCommand = c);

            //When
            managerHandler.Execute(manager);

            //Then 
            addHierarchyClassCommandHandler.Verify(cm => cm.Execute(It.IsAny<AddHierarchyClassCommand>()));
            addHierarchyClassMessageCommandHandler.Verify(cm => cm.Execute(It.IsAny<AddHierarchyClassMessageCommand>()));

            Assert.AreEqual(manager.NewHierarchyClass.hierarchyClassName, command.NewHierarchyClass.hierarchyClassName);
            Assert.AreEqual(manager.NonMerchandiseTrait, command.NonMerchandiseTrait);
            Assert.AreEqual(manager.SubTeamHierarchyClassId, command.SubTeamHierarchyClassId);
            Assert.AreEqual(manager.TaxAbbreviation, command.TaxAbbreviation);

            Assert.AreEqual(true, messageCommand.ClassNameChange);
            Assert.AreEqual(false, messageCommand.DeleteMessage);
            Assert.AreEqual(manager.NewHierarchyClass.hierarchyClassName, messageCommand.HierarchyClass.hierarchyClassName);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void AddHierarchyClassManager_AddHierarchyClassCommandHandlerThrowsException_ShouldThrowException()
        {
            //Given
            addHierarchyClassCommandHandler.Setup(cm => cm.Execute(It.IsAny<AddHierarchyClassCommand>()))
                .Throws(new Exception("Test Exception"));

            //When
            managerHandler.Execute(manager);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void AddHierarchyClassManager_AddHierarchyClassMessageCommandHandlerThrowsException_ShouldThrowException()
        {
            //Given
            addHierarchyClassMessageCommandHandler.Setup(cm => cm.Execute(It.IsAny<AddHierarchyClassMessageCommand>()))
                .Throws(new Exception("Test Exception"));

            //When
            managerHandler.Execute(manager);
        }

        [TestMethod]
        public void AddHierarchyClassManager_CommandHandlerThrowsDuplicateValueException_ShouldThrowCommandExceptionWithDuplicateValueExceptionAsInnerException()
        {
            //Given
            addHierarchyClassMessageCommandHandler.Setup(cm => cm.Execute(It.IsAny<AddHierarchyClassMessageCommand>()))
                .Throws(new DuplicateValueException("Test Exception"));

            //When
            try
            {
                managerHandler.Execute(manager);
            }
            catch (CommandException ex)
            {
                //Then
                DuplicateValueException innerException = ex.InnerException as DuplicateValueException;
                Assert.IsNotNull(innerException);
            }
        }
    }
}