using Icon.RenewableContext;
using Icon.Esb.EwicAplListener.DataAccess.Commands;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;

namespace Icon.Esb.EwicAplListener.Tests.Integration.Commands
{
    [TestClass]
    public class AddAgencyCommandTests
    {
        private AddAgencyCommand command;
        private IRenewableContext<IconContext> globalContext;
        private IconContext context;
        private DbContextTransaction transaction;
        private Agency testAgency;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            command = new AddAgencyCommand(globalContext);

            testAgency = new Agency { AgencyId = "ZZ" };

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void AddAgency_ValidAgency_AgencyShouldBeSavedToDatabase()
        {
            // Given.
            var parameters = new AddAgencyParameters { Agency = testAgency };

            // When.
            command.Execute(parameters);

            // Then.
            var newAgency = context.Agency.SingleOrDefault(a => a.AgencyId == testAgency.AgencyId);

            Assert.IsNotNull(newAgency);
        }
    }
}
