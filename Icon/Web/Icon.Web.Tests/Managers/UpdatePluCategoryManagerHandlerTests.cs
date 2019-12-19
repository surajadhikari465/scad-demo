using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass] [Ignore]
    public class UpdatePluCategoryManagerHandlerTests
    {
        private UpdatePluCategoryManagerHandler managerHandler;
        private Mock<ICommandHandler<UpdatePluCategoryCommand>> mockUpdatePluCategoryCommandHandler;
        private UpdatePluCategoryManager manager;
        private IconContext context;
        private DbContextTransaction transaction;

        private string testBeginRange = "85046";
        private string testEndRange = "85047";
        private string testPluCategoryName = "TestPluCategoryName";

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mockUpdatePluCategoryCommandHandler = new Mock<ICommandHandler<UpdatePluCategoryCommand>>();

            manager = new UpdatePluCategoryManager();
            managerHandler = new UpdatePluCategoryManagerHandler(context, mockUpdatePluCategoryCommandHandler.Object);
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
        public void UpdatePluCategory_ValidPluCategory_ShouldCallUpdatePluCategoryCommandHandler()
        {
            //Given
            manager.BeginRange = testBeginRange;
            manager.EndRange = testEndRange;
            manager.PluCategoryName = testPluCategoryName;

            //When
            managerHandler.Execute(manager);

            //Then
            mockUpdatePluCategoryCommandHandler.Verify(m => m.Execute(It.IsAny<UpdatePluCategoryCommand>()), Times.Once);
        }

        [TestMethod, ExpectedException(typeof(CommandException), "PLU category range overlaps with another existing PLU range. Please enter new values")]
        public void UpdatePluCategory_DuplicateRange_ShouldThrowDuplicateRangeException()
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
        public void UpdatePluCategory_DuplicateCategoryName_ShouldThrowDuplicateCategoryException()
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
        public void UpdatePluCategory_CommandHandlerThrowsException_ShouldThrowCommandException()
        {
            //Given
            mockUpdatePluCategoryCommandHandler.Setup(m => m.Execute(It.IsAny<UpdatePluCategoryCommand>()))
                .Throws(new Exception("Test Exception"));

            manager.BeginRange = testBeginRange;
            manager.EndRange = testEndRange;
            manager.PluCategoryName = testPluCategoryName;

            //When
            managerHandler.Execute(manager);
        }
    }
}
