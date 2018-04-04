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
    [TestClass] [Ignore]
    public class UpdateBrandManagerHandlerTests
    {
        private UpdateBrandManagerHandler managerHandler;
        private IconContext context;
        private Mock<ICommandHandler<UpdateBrandCommand>> mockUpdateBrandCommand;
        private Mock<ICommandHandler<UpdateBrandHierarchyClassTraitsCommand>> mockUpdateBrandHierarchyClassTraitsCommand;
        private Mock<ICommandHandler<AddBrandMessageCommand>> mockAddBrandMessageCommand;
        private HierarchyClass testBrand;
        private string testBrandName;
        private string testBrandAbbreviation;

        [TestInitialize]
        public void Initialize()
        {
            
            mockUpdateBrandCommand = new Mock<ICommandHandler<UpdateBrandCommand>>();
            mockUpdateBrandHierarchyClassTraitsCommand = new Mock<ICommandHandler<UpdateBrandHierarchyClassTraitsCommand>>();
            mockAddBrandMessageCommand = new Mock<ICommandHandler<AddBrandMessageCommand>>();

            testBrandName = "Test";
            testBrandAbbreviation = "ABBR";

            testBrand = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Brands).WithHierarchyClassName(testBrandName);

            AutoMapperWebConfiguration.Configure();

            context = new IconContext();
        }

        private void BuildManagerHandler()
        {
            managerHandler = new UpdateBrandManagerHandler(
                context,
                mockUpdateBrandCommand.Object,
                mockUpdateBrandHierarchyClassTraitsCommand.Object,
                mockAddBrandMessageCommand.Object);
        }

        [TestMethod]
        public void UpdateBrand_SuccessfulExecution_AllCommandsShouldBeCalled()
        {
            // Given.
            BuildManagerHandler();

            var manager = new UpdateBrandManager
            {
                Brand = testBrand,
                BrandAbbreviation = testBrandAbbreviation
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockUpdateBrandCommand.Verify(c => c.Execute(It.IsAny<UpdateBrandCommand>()), Times.Once);
            mockUpdateBrandHierarchyClassTraitsCommand.Verify(c => c.Execute(It.IsAny<UpdateBrandHierarchyClassTraitsCommand>()), Times.Once);
            mockAddBrandMessageCommand.Verify(c => c.Execute(It.IsAny<AddBrandMessageCommand>()), Times.Once);
        }

        [TestMethod]
        public void UpdateBrand_UpdateBrandCommandThrowsDuplicateValueException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = "Duplicate";
            var duplicateValueException = new DuplicateValueException(exceptionMessage);

            mockUpdateBrandCommand.Setup(c => c.Execute(It.IsAny<UpdateBrandCommand>())).Throws(duplicateValueException);

            BuildManagerHandler();

            var manager = new UpdateBrandManager
            {
                Brand = testBrand,
                BrandAbbreviation = testBrandAbbreviation
            };

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
            Assert.AreSame(caughtException.InnerException, duplicateValueException);
        }

        [TestMethod]
        public void UpdateBrand_UpdateBrandCommandThrowsUnexpectedException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = String.Format("An error occurred when updating Brand ID {0}.", testBrand.hierarchyClassID);
            var unexpectedException = new Exception(exceptionMessage);

            mockUpdateBrandCommand.Setup(c => c.Execute(It.IsAny<UpdateBrandCommand>())).Throws(unexpectedException);

            BuildManagerHandler();

            var manager = new UpdateBrandManager
            {
                Brand = testBrand,
                BrandAbbreviation = testBrandAbbreviation
            };

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
        public void UpdateBrand_UpdateBrandHierarchyClassTraitsCommandThrowsDuplicateValueException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = "Duplicate";
            var duplicateValueException = new DuplicateValueException(exceptionMessage);

            mockUpdateBrandHierarchyClassTraitsCommand.Setup(c => c.Execute(It.IsAny<UpdateBrandHierarchyClassTraitsCommand>())).Throws(duplicateValueException);

            BuildManagerHandler();

            var manager = new UpdateBrandManager
            {
                Brand = testBrand,
                BrandAbbreviation = testBrandAbbreviation
            };

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
            Assert.AreSame(caughtException.InnerException, duplicateValueException);
        }

        [TestMethod]
        public void UpdateBrand_UpdateBrandHierarchyClassTraitsCommandThrowsUnexpectedException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = String.Format("An error occurred when updating Brand ID {0}.", testBrand.hierarchyClassID);
            var unexpectedException = new Exception(exceptionMessage);

            mockUpdateBrandHierarchyClassTraitsCommand.Setup(c => c.Execute(It.IsAny<UpdateBrandHierarchyClassTraitsCommand>())).Throws(unexpectedException);

            BuildManagerHandler();

            var manager = new UpdateBrandManager
            {
                Brand = testBrand,
                BrandAbbreviation = testBrandAbbreviation
            };

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
        public void UpdateBrand_AddHierarchyClassMessageCommandThrowsUnexpectedException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = String.Format("An error occurred when updating Brand ID {0}.", testBrand.hierarchyClassID);
            var unexpectedException = new Exception(exceptionMessage);

            mockAddBrandMessageCommand.Setup(c => c.Execute(It.IsAny<AddBrandMessageCommand>())).Throws(unexpectedException);

            BuildManagerHandler();

            var manager = new UpdateBrandManager
            {
                Brand = testBrand,
                BrandAbbreviation = testBrandAbbreviation
            };

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
    }
}
