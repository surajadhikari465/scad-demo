using Icon.Common.DataAccess;
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

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass]
    public class AddBrandManagerHandlerTests
    {
        private BrandManagerHandler managerHandler;
        private IconContext context;
        private Mock<ICommandHandler<BrandCommand>> mockBrandCommand;
        private Mock<ICommandHandler<BrandCommandHandler>> mockBrandCommandhandler;
        private Mock<ICommandHandler<UpdateHierarchyClassTraitCommand>> mockBrandTraitsCommand;
        private Mock<ICommandHandler<UpdateHierarchyClassTraitCommandHandler>> mockBrandTraitsCommandHandler;
        private Mock<ICommandHandler<AddBrandMessageCommand>> mockAddBrandMessageCommand;
        private Mock<ICommandHandler<UpdateBrandHierarchyClassTraitsCommand>> mockBrandHierarchyClassTraitsCommand;
        private HierarchyClass testBrand;
        private string testBrandName;
        private string testBrandAbbreviation;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mockBrandCommand = new Mock<ICommandHandler<BrandCommand>>();
            mockAddBrandMessageCommand = new Mock<ICommandHandler<AddBrandMessageCommand>>();
            mockBrandHierarchyClassTraitsCommand = new Mock<ICommandHandler<UpdateBrandHierarchyClassTraitsCommand>>();

            testBrandName = "Test";
            testBrandAbbreviation = "ABBR";
            testBrand = new HierarchyClass(){ hierarchyClassName = testBrandName, hierarchyID = Hierarchies.Brands, hierarchyLevel = HierarchyLevels.Parent, hierarchyParentClassID = null };

            AutoMapperWebConfiguration.Configure();
        }

        private void BuildManagerHandler()
        {
            managerHandler = new BrandManagerHandler(context, mockBrandCommand.Object, mockBrandHierarchyClassTraitsCommand.Object, mockAddBrandMessageCommand.Object);
        }

        [TestMethod]
        public void AddBrand_SuccessfulExecution_AllCommandsShouldBeCalled()
        {
            // Given.
            BuildManagerHandler();

            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "TestBrand",
                hierarchyClassID = 1
            };

            var manager = GetBrandManager(hierarchyClass, String.Empty, Enums.WriteAccess.Full);

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockBrandCommand.Verify(c => c.Execute(It.IsAny<BrandCommand>()), Times.Once);
            mockAddBrandMessageCommand.Verify(c => c.Execute(It.IsAny<AddBrandMessageCommand>()), Times.Once);
        }

        [TestMethod]
        public void AddBrand_AddBrandCommandThrowsDuplicateValueException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = "Duplicate";
            var duplicateValueException = new DuplicateValueException(exceptionMessage);

            mockBrandCommand.Setup(c => c.Execute(It.IsAny<BrandCommand>())).Throws(duplicateValueException);

            BuildManagerHandler();

            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "TestBrand",
                hierarchyClassID = 1
            };

            var manager = GetBrandManager(hierarchyClass, String.Empty, Enums.WriteAccess.Full);

            // When.
            CommandException caughtException = null;

            try
            {
                managerHandler.Execute(manager);
            }
            catch(CommandException ex)
            {
                caughtException = ex;
            }

            // Then.
            Assert.AreEqual(exceptionMessage, caughtException.Message);
            Assert.AreSame(caughtException.InnerException, duplicateValueException);
        }

        [TestMethod]
        public void AddBrand_AddBrandCommandThrowsUnexpectedException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = $"An error occurred when processing Brand <{testBrandName}> (ID: 0).";
            var unexpectedException = new Exception(exceptionMessage);

            mockBrandCommand.Setup(c => c.Execute(It.IsAny<BrandCommand>())).Throws(unexpectedException);

            BuildManagerHandler();

            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = testBrandName,
                hierarchyClassID = testBrand.hierarchyClassID
            };

            var manager = GetBrandManager(hierarchyClass, String.Empty, Enums.WriteAccess.Full);

            // When.
            Exception caughtException = null;

            try
            {
                managerHandler.Execute(manager);
            }
            catch(CommandException ex)
            {
                caughtException = ex;
            }

            // Then.
            Assert.AreEqual(exceptionMessage, caughtException.Message);
            Assert.AreSame(caughtException.InnerException, unexpectedException);
        }

        [TestMethod]
        public void AddBrand_AddHierarchyClassMessageCommandThrowsException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = $"An error occurred when processing Brand <{testBrandName}> (ID: 0).";
            var unexpectedException = new Exception(exceptionMessage);

            mockAddBrandMessageCommand.Setup(c => c.Execute(It.IsAny<AddBrandMessageCommand>())).Throws(unexpectedException);

            BuildManagerHandler();

            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = testBrandName,
                hierarchyClassID = testBrand.hierarchyClassID
            };

            var manager = GetBrandManager(hierarchyClass, String.Empty, Enums.WriteAccess.Full);

            // When.
            Exception caughtException = null;

            try
            {
                managerHandler.Execute(manager);
            }
            catch(CommandException ex)
            {
                caughtException = ex;
            }

            // Then.
            Assert.AreEqual(exceptionMessage, caughtException.Message);
            Assert.AreSame(caughtException.InnerException, unexpectedException);
        }

        [TestMethod]
        public void AddBrand_UserWriteAccessIsFull_AllCommandHandlersCalled()
        {
            // Given.
            BuildManagerHandler();

            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "TestBrand",
                hierarchyClassID = 1
            };

            var manager = GetBrandManager(hierarchyClass, String.Empty, Enums.WriteAccess.Full);

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockBrandCommand.Verify(c => c.Execute(It.IsAny<BrandCommand>()), Times.Once);
            mockAddBrandMessageCommand.Verify(c => c.Execute(It.IsAny<AddBrandMessageCommand>()), Times.Once);
            mockBrandHierarchyClassTraitsCommand.Verify(c => c.Execute(It.IsAny<UpdateBrandHierarchyClassTraitsCommand>()), Times.Once);
        }

        [TestMethod]
        public void AddBrand_UserWriteAccessIsTraits_AllCommandHandlersCalled()
        {
            // Given.
            BuildManagerHandler();

            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "TestBrand",
                hierarchyClassID = 1
            };

            var manager = GetBrandManager(hierarchyClass, String.Empty, Enums.WriteAccess.Traits);

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockBrandCommand.Verify(c => c.Execute(It.IsAny<BrandCommand>()), Times.Never);
            mockAddBrandMessageCommand.Verify(c => c.Execute(It.IsAny<AddBrandMessageCommand>()), Times.Never);
            mockBrandHierarchyClassTraitsCommand.Verify(c => c.Execute(It.IsAny<UpdateBrandHierarchyClassTraitsCommand>()), Times.Once);
        }

        [TestMethod]
        public void AddBrand_UserWriteAccessIsNone_ZeroCommandHandlersCalled()
        {
            // Given.
            BuildManagerHandler();

            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "TestBrand",
                hierarchyClassID = 1
            };

            var manager = GetBrandManager(hierarchyClass, String.Empty, Enums.WriteAccess.None);

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockBrandCommand.Verify(c => c.Execute(It.IsAny<BrandCommand>()), Times.Never);
            mockAddBrandMessageCommand.Verify(c => c.Execute(It.IsAny<AddBrandMessageCommand>()), Times.Never);
            mockBrandHierarchyClassTraitsCommand.Verify(c => c.Execute(It.IsAny<UpdateBrandHierarchyClassTraitsCommand>()), Times.Never);
        }

        BrandManager GetBrandManager(HierarchyClass testBrand, string brandAbbreviation, Enums.WriteAccess userAccess)
        {
            return  new BrandManager()
            {
                Brand = testBrand,
                BrandAbbreviation = testBrandAbbreviation,
                WriteAccess = userAccess
            };
        }
    }
}