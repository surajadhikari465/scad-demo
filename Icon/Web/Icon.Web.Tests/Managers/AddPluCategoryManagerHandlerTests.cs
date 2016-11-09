using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass]
    public class AddPluCategoryManagerHandlerTests
    {
        private AddPluCategoryManagerHandler managerHandler;
        private Mock<ICommandHandler<AddPluCategoryCommand>> mockAddPluCategoryCommandHandler;
        private AddPluCategoryManager manager;
        private IconContext context;
        private DbContextTransaction transaction;

        private string testBeginRange = "500000";
        private string testEndRange = "500001";
        private string testPluCategoryName = "TestPluCategoryName";

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mockAddPluCategoryCommandHandler = new Mock<ICommandHandler<AddPluCategoryCommand>>();

            manager = new AddPluCategoryManager();
            managerHandler = new AddPluCategoryManagerHandler(context, mockAddPluCategoryCommandHandler.Object);
            transaction = context.Database.BeginTransaction();

            var endRanges = this.context.PLUCategory.Select(c => c.EndRange).ToList();
            var maxEndRange = endRanges.Select(long.Parse).Max() + 1;
            this.testBeginRange = maxEndRange.ToString();
            this.testEndRange = (maxEndRange + 1).ToString();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }
        
        [TestMethod]
        public void AddPluCategory_ValidPluCategory_ShouldCallAddPluCategoryCommandHandler()
        {
            //Given
            manager.BeginRange = this.testBeginRange;
            manager.EndRange = this.testEndRange;
            manager.PluCategoryName = testPluCategoryName;

            //When
            managerHandler.Execute(manager);

            //Then
            mockAddPluCategoryCommandHandler.Verify(m => m.Execute(It.IsAny<AddPluCategoryCommand>()), Times.Once);
        }

        [TestMethod, ExpectedException(typeof(CommandException), "PLU category range overlaps with another existing PLU range. Please enter new values")]
        public void AddPluCategory_DuplicateRange_ShouldThrowDuplicateRangeException()
        {
            //Given
            context.PLUCategory.Add(new PLUCategory
            {
                BeginRange = testBeginRange,
                EndRange = testEndRange,
                PluCategoryName = testPluCategoryName
            });
            context.SaveChanges();

            manager.BeginRange = testBeginRange;
            manager.EndRange = testEndRange;
            manager.PluCategoryName = "DifferentTestName";

            //When
            managerHandler.Execute(manager);
        }

        [TestMethod, ExpectedException(typeof(CommandException), "Another PLU category with specified name exists. Please enter new PLU category name.")]
        public void AddPluCategory_DuplicateCategoryName_ShouldThrowDuplicateCategoryException()
        {
            //Given
            context.PLUCategory.Add(new PLUCategory
            {
                BeginRange = testBeginRange,
                EndRange = testEndRange,
                PluCategoryName = testPluCategoryName
            });
            context.SaveChanges();

            manager.BeginRange = "48";
            manager.EndRange = "49";
            manager.PluCategoryName = testPluCategoryName;

            //When
            managerHandler.Execute(manager);
        }

        [TestMethod, ExpectedException(typeof(CommandException), "An error occurred when adding PLU Category TestPluCategoryName.")]
        public void AddPluCategory_CommandHandlerThrowsException_ShouldThrowCommandException()
        {
            //Given
            mockAddPluCategoryCommandHandler.Setup(m => m.Execute(It.IsAny<AddPluCategoryCommand>()))
                .Throws(new Exception("Test Exception"));

            manager.BeginRange = testBeginRange;
            manager.EndRange = testEndRange;
            manager.PluCategoryName = testPluCategoryName;

            //When
            managerHandler.Execute(manager);
        }
    }
}
