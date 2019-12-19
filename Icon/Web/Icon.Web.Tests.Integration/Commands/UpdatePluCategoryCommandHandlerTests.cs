using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class UpdatePluCategoryCommandHandlerTests
    {
        private UpdatePluCategoryCommandHandler commandHandler;
        private UpdatePluCategoryCommand command;
        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            commandHandler = new UpdatePluCategoryCommandHandler(context);
            command = new UpdatePluCategoryCommand();
            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void UpdatePluCategory_PluCategoryExists_ShouldUpdatePlu()
        {
            //Given
            PLUCategory testPluCategory = new PLUCategory
            {
                BeginRange = "1",
                EndRange = "1",
                PluCategoryName = "Test Plu Category Name"
            };
            context.PLUCategory.Add(testPluCategory);
            context.SaveChanges();

            command.PluCategory = new PLUCategory
            {
                PluCategoryID = testPluCategory.PluCategoryID,
                BeginRange = "5",
                EndRange = "6",
                PluCategoryName = "Test Plu Category Name Updated"
            };

            //When
            commandHandler.Execute(command);

            //Then
            var actualPluCategory = context.PLUCategory.FirstOrDefault(pluc => pluc.PluCategoryID == testPluCategory.PluCategoryID);
            Assert.IsNotNull(actualPluCategory);
            Assert.AreEqual(command.PluCategory.BeginRange, actualPluCategory.BeginRange);
            Assert.AreEqual(command.PluCategory.EndRange, actualPluCategory.EndRange);
            Assert.AreEqual(command.PluCategory.PluCategoryName, actualPluCategory.PluCategoryName);
        }
    }
}
