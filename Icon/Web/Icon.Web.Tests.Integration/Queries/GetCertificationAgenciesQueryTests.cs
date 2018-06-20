using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    [Ignore]
    public class GetCertificationAgenciesQueryTests
    {
        private GetCertificationAgenciesQuery query;
        private GetCertificationAgenciesParameters parameters;
        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();
            query = new GetCertificationAgenciesQuery(context);
            parameters = new GetCertificationAgenciesParameters();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void GetCertificationAgenciesQuery_AgenciesExist_ShouldReturnAgencies()
        {
            //Given
            context.HierarchyClass.Add(new TestHierarchyClassBuilder()
                .WithHierarchyClassName("Test Agency")
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithGlutenFreeTrait("1"));
            context.SaveChanges();

            //When
            var results = query.Search(parameters);

            //Then
            var agency = results.SingleOrDefault(a => a.HierarchyClassName == "Test Agency");
            Assert.IsNotNull(agency);
            Assert.AreEqual("1", agency.GlutenFree);
            Assert.AreEqual("0", agency.Kosher);
            Assert.AreEqual("0", agency.NonGMO);
            Assert.AreEqual("0", agency.Organic);
            Assert.AreEqual("0", agency.Vegan);
        }
    }
}