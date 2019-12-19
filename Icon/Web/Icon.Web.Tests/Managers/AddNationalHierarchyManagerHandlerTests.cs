using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass]
    public class AddNationalHierarchyManagerHandlerTests
    {
        private AddNationalHierarchyManagerHandler managerHandler;
        private AddNationalHierarchyManager manager;

        private IconContext context;
        private Mock<ICommandHandler<AddNationalHierarchyCommand>> addNationalHierarchyCommandHandler;
        private Mock<ICommandHandler<AddVimEventCommand>> addVimHierarchyEventCommandHandler;
        private Mock<ICommandHandler<AddHierarchyClassMessageCommand>> addHierarchyClassMessageCommandHandler;
        private Mock<IQueryHandler<GetNationalClassByClassCodeParameters, List<HierarchyClass>>> getNationalClassQuery;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            addNationalHierarchyCommandHandler = new Mock<ICommandHandler<AddNationalHierarchyCommand>>();
            addVimHierarchyEventCommandHandler = new Mock<ICommandHandler<AddVimEventCommand>>();
            addHierarchyClassMessageCommandHandler = new Mock<ICommandHandler<AddHierarchyClassMessageCommand>>();
            getNationalClassQuery = new Mock<IQueryHandler<GetNationalClassByClassCodeParameters, List<HierarchyClass>>>();

            manager = new AddNationalHierarchyManager
            {
                NationalClassCode = "12345",
                NationalHierarchy = new TestHierarchyClassBuilder(),
                UserName = "test"
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }

        [TestMethod]
        public void AddNationalHierarchyManager_ShouldCallCommandHandlers()
        {
            //Given
            AddNationalHierarchyCommand command = null;
            AddHierarchyClassMessageCommand messageCommand = null;
            AddVimEventCommand vimEventCommand = null;
            addNationalHierarchyCommandHandler.Setup(cm => cm.Execute(It.IsAny<AddNationalHierarchyCommand>()))
                .Callback<AddNationalHierarchyCommand>(c => command = c);
            addHierarchyClassMessageCommandHandler.Setup(cm => cm.Execute(It.IsAny<AddHierarchyClassMessageCommand>()))
                .Callback<AddHierarchyClassMessageCommand>(c => messageCommand = c);
            addVimHierarchyEventCommandHandler.Setup(cm => cm.Execute(It.IsAny<AddVimEventCommand>()))
                .Callback<AddVimEventCommand>(c => vimEventCommand = c);
            CreateManagerHandler();

            //When
            managerHandler.Execute(manager);

            //Then
            addNationalHierarchyCommandHandler.Verify(cm => cm.Execute(It.IsAny<AddNationalHierarchyCommand>()));
            addHierarchyClassMessageCommandHandler.Verify(cm => cm.Execute(It.IsAny<AddHierarchyClassMessageCommand>()));
            addVimHierarchyEventCommandHandler.Verify(cm => cm.Execute(It.IsAny<AddVimEventCommand>()));

            Assert.AreEqual(manager.NationalClassCode, command.NationalClassCode);
            Assert.AreEqual(manager.UserName, command.UserName);

            Assert.IsTrue(messageCommand.ClassNameChange);
            Assert.IsFalse(messageCommand.DeleteMessage);

            Assert.AreEqual("Unit Test Hierarchy Class Level 1", vimEventCommand.EventMessage);
        }

        [TestMethod]
        public void AddNationalHierarchyManager_CommandHandlerThrowsException_ShouldThrowException()
        {
            //Given
            addNationalHierarchyCommandHandler.Setup(cm => cm.Execute(It.IsAny<AddNationalHierarchyCommand>()))
                .Throws(new Exception("Test Exception"));
            CreateManagerHandler();

            //When
            try
            {
                managerHandler.Execute(manager);
            }
            catch (CommandException ex)
            {
                //Then
                Exception innerException = ex.InnerException as Exception;
                Assert.IsNotNull(innerException);
            }
        }

        [TestMethod]
        public void AddNationalHierarchyManager_CommandHandlerThrowsDuplicateValueException_ShouldThrowCommandExceptionWithDuplicateValueExceptionAsInnerException()
        {
            //Given
            addNationalHierarchyCommandHandler.Setup(cm => cm.Execute(It.IsAny<AddNationalHierarchyCommand>()))
                .Throws(new DuplicateValueException("Test Exception"));
            CreateManagerHandler();

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

        private void CreateManagerHandler()
        {
            managerHandler = new AddNationalHierarchyManagerHandler(
                context,
                getNationalClassQuery.Object,
                addNationalHierarchyCommandHandler.Object,
                addVimHierarchyEventCommandHandler.Object,
                addHierarchyClassMessageCommandHandler.Object,
                new AppSettings());
        }
    }
}
