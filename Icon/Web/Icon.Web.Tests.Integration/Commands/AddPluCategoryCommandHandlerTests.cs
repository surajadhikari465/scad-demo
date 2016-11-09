using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.DataAccess.Commands;
using Icon.Framework;
using System.Data.Entity;
using Icon.Web.DataAccess.Infrastructure;
using System.Linq;

using Icon.Web.Tests.Common.Builders;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class AddPluCategoryCommandHandlerTests
    {
        private AddPluCategoryCommandHandler commandHandler;
        private AddPluCategoryCommand command;
        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            commandHandler = new AddPluCategoryCommandHandler(context);
            command = new AddPluCategoryCommand();
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
        public void AddPluCategory_ValidPluCategory_ShouldAddPluCategory()
        {
            //Given
            command.PluCategory = new TestPluCategoryBuilder();

            //When
            commandHandler.Execute(command);

            //Then
            Assert.IsNotNull(context.PLUCategory.SingleOrDefault(pluc => pluc.PluCategoryName == command.PluCategory.PluCategoryName));
        }
    }
}
