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
    public class GetNationalClassCodeToIdMappingQueryHandlerTests
    {
        private IconContext context;
        private GetNationalClassCodeToClassIdMappingQuery query;
        private GetNationalClassCodeToClassIdMappingQueryHandler handler;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.query = new GetNationalClassCodeToClassIdMappingQuery();
            this.handler = new GetNationalClassCodeToClassIdMappingQueryHandler(this.context);
        }

        [TestMethod]
        public void GetNationalClassCodeToIdMapping_QueryObject_ReturnsDictionary()
        {
            // Given
            List<HierarchyClassTrait> nationalClassTraitList = this.context.HierarchyClassTrait
                .Where(hct => hct.traitID == Traits.NationalClassCode)
                .ToList();

            Dictionary<int, int> expected = new Dictionary<int, int>();
            foreach (var nationalClassTrait in nationalClassTraitList)
            {
                expected.Add(Int32.Parse(nationalClassTrait.traitValue), nationalClassTrait.hierarchyClassID);
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
