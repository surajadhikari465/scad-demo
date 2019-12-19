using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class AddTaxEventCommandHandlerTests
    {
        private AddTaxEventCommandHandler commandHandler;

        private IconContext context;
        private HierarchyClass taxClass;
        private HierarchyClassTrait taxAbbreviation;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.commandHandler = new AddTaxEventCommandHandler(context);
            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.transaction.Rollback();
            context.Dispose();
        }

        [TestMethod]
        public void EventGeneratorTax_NullTaxAbbreviation_NoEventCreated()
        {
            // Given
            AddTaxClass();
            AddTaxAbbreviation();

            // When
            commandHandler.Execute(new AddTaxEventCommand { TaxAbbreviation = null, HierarchyClassId = this.taxClass.hierarchyClassID });

            // Then
            var result = context.EventQueue.FirstOrDefault(e => e.EventReferenceId.Value == this.taxClass.hierarchyClassID);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void EventGeneratorTax_NotNullTaxAbbreviation_EventCreated()
        {
            // Given
            AddTaxClass();
            AddTaxAbbreviation();

            string newTaxAbbreviation = "1111111 updated tax abbrev";
            string expectedEventMessage = "1111111";
            string[] regions = ConfigurationManager.AppSettings["TaxUpdateEventConfiguredRegions"].Split(',');

            // When
            commandHandler.Execute(new AddTaxEventCommand { TaxAbbreviation = newTaxAbbreviation, HierarchyClassId = this.taxClass.hierarchyClassID });

            // Then
            var result = this.context.EventQueue.Where(e => e.EventReferenceId.Value == this.taxClass.hierarchyClassID);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(regions.Count(), result.Count());
            foreach (var region in regions)
            {
                Assert.AreEqual(result.First(r => r.RegionCode == region).EventMessage, expectedEventMessage);
                Assert.AreEqual(result.First(r => r.RegionCode == region).EventId, EventTypes.TaxNameUpdate);
            }
        }

        private void AddTaxClass()
        {
            this.taxClass = new HierarchyClass { hierarchyClassName = "1111111 event test tax class", hierarchyID = Hierarchies.Tax, hierarchyLevel = 1 };
            this.context.HierarchyClass.Add(taxClass);
            this.context.SaveChanges();
        }

        private void AddTaxAbbreviation()
        {
            this.taxAbbreviation = new HierarchyClassTrait { hierarchyClassID = this.taxClass.hierarchyClassID, traitID = Traits.TaxAbbreviation, traitValue = "1111111 event test tax abbrev" };
            this.context.HierarchyClassTrait.Add(taxAbbreviation);
            this.context.SaveChanges();
        }

    }
}
