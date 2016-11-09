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
        private AddBrandManagerHandler managerHandler;
        private IconContext context;
        private Mock<ICommandHandler<AddBrandCommand>> mockAddBrandCommand;
        private Mock<ICommandHandler<AddBrandMessageCommand>> mockAddBrandMessageCommand;
        private HierarchyClass testBrand;
        private string testBrandName;
        private string testBrandAbbreviation;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mockAddBrandCommand = new Mock<ICommandHandler<AddBrandCommand>>();
            mockAddBrandMessageCommand = new Mock<ICommandHandler<AddBrandMessageCommand>>();

            testBrandName = "Test";
            testBrandAbbreviation = "ABBR";

            testBrand = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Brands).WithHierarchyClassName(testBrandName);

            AutoMapperWebConfiguration.Configure();
        }

        private void BuildManagerHandler()
        {
            managerHandler = new AddBrandManagerHandler(context, mockAddBrandCommand.Object, mockAddBrandMessageCommand.Object);
        }

        [TestMethod]
        public void AddBrand_SuccessfulExecution_AllCommandsShouldBeCalled()
        {
            // Given.
            BuildManagerHandler();

            var manager = new AddBrandManager
            {
                Brand = testBrand,
                BrandAbbreviation = testBrandAbbreviation
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddBrandCommand.Verify(c => c.Execute(It.IsAny<AddBrandCommand>()), Times.Once);
            mockAddBrandMessageCommand.Verify(c => c.Execute(It.IsAny<AddBrandMessageCommand>()), Times.Once);
        }

        [TestMethod]
        public void AddBrand_AddBrandCommandThrowsDuplicateValueException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = "Duplicate";
            var duplicateValueException = new DuplicateValueException(exceptionMessage);

            mockAddBrandCommand.Setup(c => c.Execute(It.IsAny<AddBrandCommand>())).Throws(duplicateValueException);

            BuildManagerHandler();

            var manager = new AddBrandManager
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
        public void AddBrand_AddBrandCommandThrowsUnexpectedException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = String.Format("An error occurred when adding Brand {0}.", testBrandName);
            var unexpectedException = new Exception(exceptionMessage);

            mockAddBrandCommand.Setup(c => c.Execute(It.IsAny<AddBrandCommand>())).Throws(unexpectedException);

            BuildManagerHandler();

            var manager = new AddBrandManager
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
        public void AddBrand_AddHierarchyClassMessageCommandThrowsException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = String.Format("An error occurred when adding Brand {0}.", testBrandName);
            var unexpectedException = new Exception(exceptionMessage);

            mockAddBrandMessageCommand.Setup(c => c.Execute(It.IsAny<AddBrandMessageCommand>())).Throws(unexpectedException);

            BuildManagerHandler();

            var manager = new AddBrandManager
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
