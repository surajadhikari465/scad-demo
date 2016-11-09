using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Framework;
using RegionalEventController.DataAccess.Queries;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace RegionalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetTaxCodeToTaxIdMappingQueryHandlerTests
    {
        private IconContext context;
        private GetTaxCodeToTaxIdMappingQuery query;
        private GetTaxCodeToTaxIdMappingQueryHandler handler;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.query = new GetTaxCodeToTaxIdMappingQuery();
            this.handler = new GetTaxCodeToTaxIdMappingQueryHandler(this.context);
        }

        [TestMethod]
        public void GetTaxCodeToTaxIdMapping_QueryObject_ReturnsStringToIntDictionary()
        {
            // Given
            List<HierarchyClassTrait> taxes = this.context.HierarchyClassTrait
                .Where(hct => hct.traitID == Traits.TaxAbbreviation)
                .ToList();

            Dictionary<string, int> expected = new Dictionary<string, int>();
            foreach (var tax in taxes)
            {
                expected.Add(tax.traitValue.Split(' ')[0], tax.hierarchyClassID);
            }

            // When
            var actual = this.handler.Execute(this.query);

            // Then
            Assert.AreEqual(expected.Count, actual.Count, "The returned Dictionary count does not match the expected count.");

            foreach (var item in actual)
            {
                Assert.AreEqual(expected[item.Key], item.Value, "The actual dictionary value does not match the expected dictionary value.");
            }
        }
    }
}
