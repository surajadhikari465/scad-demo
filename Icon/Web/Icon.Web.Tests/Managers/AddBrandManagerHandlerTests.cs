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
            
            var manager = GetBrandManager();

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

            var manager = GetBrandManager();

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

            var manager = GetBrandManager();

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

            var manager = GetBrandManager();

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

        BrandManager GetBrandManager()
        {
            return  new BrandManager() { Brand = testBrand, BrandAbbreviation = testBrandAbbreviation, Update = UpdateOptions.Brand | UpdateOptions.Traits };
        }
    }
}