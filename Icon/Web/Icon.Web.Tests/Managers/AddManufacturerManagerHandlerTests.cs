using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass]
    public class AddManufacturerManagerHandlerTests
    {
        private ManufacturerManagerHandler managerHandler;
        private IconContext context;
        private Mock<ICommandHandler<AddManufacturerCommand>> mockAddManufacturerCommand;
        private Mock<ICommandHandler<AddManufacturerMessageCommand>> mockAddManufacturerMessageCommand;
        private Mock<ICommandHandler<UpdateManufacturerHierarchyClassTraitsCommand>> mockManufacturerHierarchyClassTraitsCommand;
        private Mock<ICommandHandler<UpdateManufacturerCommand>> mockUpdateManufacturerCommand;
        private HierarchyClass testManufacturer;
        private string testManufacturerName;
        private string testZipCode;
        private string testArCustomerId;
        private IMapper mapper;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mapper = AutoMapperWebConfiguration.Configure();
            mockAddManufacturerCommand = new Mock<ICommandHandler<AddManufacturerCommand>>();
            mockUpdateManufacturerCommand = new Mock<ICommandHandler<UpdateManufacturerCommand>>();
            mockAddManufacturerMessageCommand = new Mock<ICommandHandler<AddManufacturerMessageCommand>>();
            mockManufacturerHierarchyClassTraitsCommand = new Mock<ICommandHandler<UpdateManufacturerHierarchyClassTraitsCommand>>();

            testManufacturer = new HierarchyClass();
            testManufacturerName = "Test";
            testZipCode = "78704";
            testArCustomerId = "1234";

            managerHandler = new ManufacturerManagerHandler(
                context,
                mockAddManufacturerCommand.Object,
                mockManufacturerHierarchyClassTraitsCommand.Object,
                mockAddManufacturerMessageCommand.Object,
                mockUpdateManufacturerCommand.Object,
                mapper);
        }

        [TestMethod]
        public void AddManufacturer_SuccessfulExecution_AllCommandsShouldBeCalled()
        {
            // Given.
            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "TestManufacturer_!!",
                hierarchyClassID = 0
            };

            var manager = GetManufacturerManager(hierarchyClass);

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddManufacturerCommand.Verify(c => c.Execute(It.IsAny<AddManufacturerCommand>()), Times.Once);
            mockAddManufacturerMessageCommand.Verify(c => c.Execute(It.IsAny<AddManufacturerMessageCommand>()), Times.Once);
        }
        [TestMethod]
        public void UpdateManufacturer_SuccessfulExecution_AllCommandsShouldBeCalled()
        {
            // Given.
            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "TestManufacturer_!!",
                hierarchyClassID = 1
            };

            var manager = GetManufacturerManager(hierarchyClass);

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockUpdateManufacturerCommand.Verify(c => c.Execute(It.IsAny<UpdateManufacturerCommand>()), Times.Once);
            mockAddManufacturerMessageCommand.Verify(c => c.Execute(It.IsAny<AddManufacturerMessageCommand>()), Times.Once);
        }


        [TestMethod]
        public void AddManufacturer_AddManufacturerCommandThrowsDuplicateValueException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = "The manufacturer TestManufacturer already exists.";
            var duplicateValueException = new DuplicateValueException(exceptionMessage);

            mockAddManufacturerCommand.Setup(c => c.Execute(It.IsAny<AddManufacturerCommand>())).Throws(duplicateValueException);

            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "TestManufacturer",
                hierarchyClassID = 0
            };

            var manager = GetManufacturerManager(hierarchyClass);

            // When.
            CommandException caughtException = null;

            try
            {
                managerHandler.Execute(manager);
            }
            catch (CommandException ex)
            {
                caughtException = ex;
            }

            // Then.
            Assert.AreEqual(exceptionMessage, caughtException.Message);
            Assert.AreEqual(caughtException.InnerException.Message, duplicateValueException.Message);
        }

        [TestMethod]
        public void UpdateManufacturer_UpdateManufacturerCommandThrowsDuplicateValueException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = "The manufacturer TestManufacturer already exists.";
            var duplicateValueException = new DuplicateValueException(exceptionMessage);

            mockUpdateManufacturerCommand.Setup(c => c.Execute(It.IsAny<UpdateManufacturerCommand>())).Throws(duplicateValueException);

            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "TestManufacturer",
                hierarchyClassID = 1
            };

            var manager = GetManufacturerManager(hierarchyClass);

            // When.
            CommandException caughtException = null;

            try
            {
                managerHandler.Execute(manager);
            }
            catch (CommandException ex)
            {
                caughtException = ex;
            }

            // Then.
            Assert.AreEqual(exceptionMessage, caughtException.Message);
            Assert.AreEqual(caughtException.InnerException.Message, duplicateValueException.Message);
        }

        [TestMethod]
        public void AddManufacturer_AddManufacturerCommandThrowsUnexpectedException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = $"An error occurred when processing Manufacturer {testManufacturerName} (ID: 0).";
            var unexpectedException = new Exception(exceptionMessage);

            mockAddManufacturerCommand.Setup(c => c.Execute(It.IsAny<AddManufacturerCommand>())).Throws(unexpectedException);

            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = testManufacturerName,
                hierarchyClassID = testManufacturer.hierarchyClassID
            };

            var manager = GetManufacturerManager(hierarchyClass);

            // When.
            Exception caughtException = null;

            try
            {
                managerHandler.Execute(manager);
            }
            catch (CommandException ex)
            {
                caughtException = ex;
            }

            // Then.
            Assert.AreEqual(exceptionMessage, caughtException.Message);
            Assert.AreSame(caughtException.InnerException, unexpectedException);
        }

        [TestMethod]
        public void AddManufacturer_AddHierarchyClassMessageCommandThrowsException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = $"An error occurred when processing Manufacturer {testManufacturerName} (ID: 0).";
            var unexpectedException = new Exception(exceptionMessage);

            mockAddManufacturerMessageCommand.Setup(c => c.Execute(It.IsAny<AddManufacturerMessageCommand>())).Throws(unexpectedException);

            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = testManufacturerName,
                hierarchyClassID = testManufacturer.hierarchyClassID
            };

            var manager = GetManufacturerManager(hierarchyClass);

            // When.
            Exception caughtException = null;

            try
            {
                managerHandler.Execute(manager);
            }
            catch (CommandException ex)
            {
                caughtException = ex;
            }

            // Then.
            Assert.AreEqual(exceptionMessage, caughtException.Message);
            Assert.AreSame(caughtException.InnerException, unexpectedException);
        }

        [TestMethod]
        public void AddManufacturer_UserWriteAccessIsFull_AllCommandHandlersCalled()
        {
            // Given.
            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "TestManufacturer_!!",
                hierarchyClassID = 0
            };

            var manager = GetManufacturerManager(hierarchyClass);

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddManufacturerCommand.Verify(c => c.Execute(It.IsAny<AddManufacturerCommand>()), Times.Once);
            mockAddManufacturerMessageCommand.Verify(c => c.Execute(It.IsAny<AddManufacturerMessageCommand>()), Times.Once);
            mockManufacturerHierarchyClassTraitsCommand.Verify(c => c.Execute(It.IsAny<UpdateManufacturerHierarchyClassTraitsCommand>()), Times.Once);
        }

        ManufacturerManager GetManufacturerManager(HierarchyClass testManufacturer)
        {
            return new ManufacturerManager()
            {
                Manufacturer = testManufacturer,
                ZipCode = testZipCode,
                ArCustomerId = testArCustomerId
            };
        }
    }
}