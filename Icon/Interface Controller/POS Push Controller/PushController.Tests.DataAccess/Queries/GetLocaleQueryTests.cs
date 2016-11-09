using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushController.DataAccess.Queries;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetLocaleQueryTests
    {
        private GetLocalesByBusinessUnitIdQueryHandler getLocaleQueryHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private List<Locale> testLocales;
        private List<int> testBusinessUnits;

        [TestInitialize]
        public void Initialize()
        {
            context = new GlobalIconContext(new IconContext());

            testLocales = new List<Locale>();
            testBusinessUnits = new List<int> { 77777, 88888, 99999 };

            foreach (var businessUnit in testBusinessUnits)
            {
                testLocales.Add(new TestLocaleBuilder().WithBusinessUnitId(businessUnit).WithLocaleName("TestLocale" + businessUnit));
            }

            getLocaleQueryHandler = new GetLocalesByBusinessUnitIdQueryHandler(context);

            transaction = context.Context.Database.BeginTransaction();

            context.Context.Locale.AddRange(testLocales);
            context.Context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void GetLocale_BusinessUnitExists_LocaleShouldBeReturned()
        {
            // Given.
            var query = new GetLocalesByBusinessUnitIdQuery
            {
                BusinessUnits = testBusinessUnits.Select(bu => bu.ToString()).ToList()
            };

            // When.
            var queryResults = getLocaleQueryHandler.Execute(query);

            // Then.
            Assert.AreEqual(testBusinessUnits.Count, queryResults.Count);
        }

        [TestMethod]
        public void GetLocale_BusinessUnitDoesNotExist_LocaleShouldNotBeReturned()
        {
            // Given.
            testBusinessUnits.Add(99988);

            var query = new GetLocalesByBusinessUnitIdQuery
            {
                BusinessUnits = testBusinessUnits.Select(bu => bu.ToString()).ToList()
            };

            // When.
            var queryResults = getLocaleQueryHandler.Execute(query);

            // Then.
            Assert.AreEqual(testBusinessUnits.Count - 1, queryResults.Count);
        }
    }
}
