using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.DataAccess.Commands;
using Icon.Framework;
using System.Data.Entity;
using Icon.Web.Tests.Common.Builders;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class DeletePluCategoryByIdCommandHandlerTests
    {
        private DeletePluCategoryByIdCommandHandler query;
        private DeletePluCategoryByIdCommand command;
        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            query = new DeletePluCategoryByIdCommandHandler(context);
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
        public void GetPluCategoryByIdQuery_PluCategoriesExist_ShouldReturnPluCategory()
        {
            //Given
            PLUCategory pluCategory = new TestPluCategoryBuilder();
            context.PLUCategory.Add(pluCategory);
            context.SaveChanges();

            command = new DeletePluCategoryByIdCommand(){ PluCategoryID = pluCategory.PluCategoryID };

            //When
            query.Execute(command);

            //Then
            PLUCategory actaulPluCategory = context.PLUCategory.FirstOrDefault(pc => pc.PluCategoryID == command.PluCategoryID);
            Assert.IsNull(actaulPluCategory);
        }
    }
}
