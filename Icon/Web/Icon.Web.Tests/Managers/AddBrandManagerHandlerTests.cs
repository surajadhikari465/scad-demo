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
    public class AddBrandManagerHandlerTests
    {
        private BrandManagerHandler managerHandler;
        private IconContext context;
        private Mock<ICommandHandler<BrandCommand>> mockBrandCommand;
        private Mock<ICommandHandler<AddBrandMessageCommand>> mockAddBrandMessageCommand;
        private Mock<ICommandHandler<UpdateBrandHierarchyClassTraitsCommand>> mockBrandHierarchyClassTraitsCommand;
        private HierarchyClass testBrand;
        private string testBrandName;
        private string testBrandAbbreviation;
        private string testDesignation;
        private string testZipCode;
        private string testLocality;
        private IMapper mapper;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mapper = AutoMapperWebConfiguration.Configure();
            mockBrandCommand = new Mock<ICommandHandler<BrandCommand>>();
            mockAddBrandMessageCommand = new Mock<ICommandHandler<AddBrandMessageCommand>>();
            mockBrandHierarchyClassTraitsCommand = new Mock<ICommandHandler<UpdateBrandHierarchyClassTraitsCommand>>();

            testBrand = new HierarchyClass();
            testBrandName = "Test" + Guid.NewGuid().ToString();
            testBrandAbbreviation = "ABBR";
            testDesignation = "TestDesignation"; ;
            testZipCode = "78745";
            testLocality = "TestLocality";

            managerHandler = new BrandManagerHandler(
                context,
                mockBrandCommand.Object,
                mockBrandHierarchyClassTraitsCommand.Object,
                mockAddBrandMessageCommand.Object,
                mapper);
        }

        [TestMethod]
        public void AddBrand_SuccessfulExecution_AllCommandsShouldBeCalled()
        {
            // Given.
            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "TestBrand_!!",
                hierarchyClassID = 1
            };

            var manager = GetBrandManager(hierarchyClass, Enums.WriteAccess.Full);

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
            string exceptionMessage = "The brand TestBrand already exists.";
            var duplicateValueException = new DuplicateValueException(exceptionMessage);

            mockBrandCommand.Setup(c => c.Execute(It.IsAny<BrandCommand>())).Throws(duplicateValueException);

            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "TestBrand",
                hierarchyClassID = 1
            };

            var manager = GetBrandManager(hierarchyClass, Enums.WriteAccess.Full);

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
        public void AddBrand_AddBrandCommandThrowsUnexpectedException_CommandExceptionShouldBeThrownWithMessage()
        {
            // Given.
            string exceptionMessage = $"An error occurred when processing Brand {testBrandName} (ID: 0).";
            var unexpectedException = new Exception(exceptionMessage);

            mockBrandCommand.Setup(c => c.Execute(It.IsAny<BrandCommand>())).Throws(unexpectedException);

            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = testBrandName,
                hierarchyClassID = testBrand.hierarchyClassID
            };

            var manager = GetBrandManager(hierarchyClass, Enums.WriteAccess.Full);

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
            string exceptionMessage = $"An error occurred when processing Brand {testBrandName} (ID: 0).";
            var unexpectedException = new Exception(exceptionMessage);

            mockAddBrandMessageCommand.Setup(c => c.Execute(It.IsAny<AddBrandMessageCommand>())).Throws(unexpectedException);

            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = testBrandName,
                hierarchyClassID = testBrand.hierarchyClassID
            };

            var manager = GetBrandManager(hierarchyClass, Enums.WriteAccess.Full);

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
        public void AddBrand_UserWriteAccessIsFull_AllCommandHandlersCalled()
        {
            // Given.
            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "TestBrand_!!",
                hierarchyClassID = 1
            };

            var manager = GetBrandManager(hierarchyClass, Enums.WriteAccess.Full);

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockBrandCommand.Verify(c => c.Execute(It.IsAny<BrandCommand>()), Times.Once);
            mockAddBrandMessageCommand.Verify(c => c.Execute(It.IsAny<AddBrandMessageCommand>()), Times.Once);
            mockBrandHierarchyClassTraitsCommand.Verify(c => c.Execute(It.IsAny<UpdateBrandHierarchyClassTraitsCommand>()), Times.Once);
        }

        BrandManager GetBrandManager(HierarchyClass testBrand, Enums.WriteAccess userAccess)
        {
            return new BrandManager()
            {
                Brand = testBrand,
                BrandAbbreviation = testBrandAbbreviation,
                WriteAccess = userAccess,
                Designation = testDesignation,
                ZipCode = testZipCode,
                Locality = testLocality
            };
        }
    }
}