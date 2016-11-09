using Icon.RenewableContext;
using Icon.Esb.EwicAplListener.DataAccess.Queries;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

namespace Icon.Esb.EwicAplListener.Tests.Integration.Queries
{
    [TestClass]
    public class AgencyExistsQueryTests
    {
        private AgencyExistsQuery query;
        private IRenewableContext<IconContext> globalContext;
        private IconContext context;
        private DbContextTransaction transaction;
        private string testAgencyId;
        
        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            query = new AgencyExistsQuery(globalContext);

            testAgencyId = "ZZ";

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            context.Dispose();
            globalContext = null;
        }

        private void StageTestAgency()
        {
            context.Agency.Add(new TestAgencyBuilder().WithAgencyId(testAgencyId));
            context.SaveChanges();
        }

        [TestMethod]
        public void AgencyExists_AgencyDoesNotExist_ResultShouldBeFalse()
        {
            // Given.
            var parameters = new AgencyExistsParameters { AgencyId = testAgencyId };

            // When.
            bool result = query.Search(parameters);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AgencyExists_AgencyDoesExist_ResultShouldBeTrue()
        {
            // Given.
            StageTestAgency();

            var parameters = new AgencyExistsParameters { AgencyId = testAgencyId };

            // When.
            bool result = query.Search(parameters);

            // Then.
            Assert.IsTrue(result);
        }
    }
}
