using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Transactions;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Queries
{
    [TestClass]
    public class GetTaxIdFromTaxCodeQueryHandlerTests
    {
        private GetTaxIdFromTaxCodeQueryHandler queryHandler;
        private GetTaxIdFromTaxCodeParameters query;
        private IconDbContextFactory contextFactory;
        private IconContext context;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            context = new IconContext();

            contextFactory = new IconDbContextFactory();
            queryHandler = new GetTaxIdFromTaxCodeQueryHandler(contextFactory);
            query = new GetTaxIdFromTaxCodeParameters();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetTaxIdFromTaxCode_TaxClassesExist_ShouldReturnDictionaryOfTaxIdsAndTaxCodes()
        {
            //Given
            var taxClasses = new List<Framework.HierarchyClass>
            {
                new Framework.HierarchyClass { hierarchyID = Hierarchies.Tax, hierarchyClassName = "1234567 Test Tax 1", hierarchyLevel = 1 },
                new Framework.HierarchyClass { hierarchyID = Hierarchies.Tax, hierarchyClassName = "2234567 Test Tax 2", hierarchyLevel = 1 },
                new Framework.HierarchyClass { hierarchyID = Hierarchies.Tax, hierarchyClassName = "3234567 Test Tax 3", hierarchyLevel = 1 }
            };
            context.HierarchyClass.AddRange(taxClasses);
            context.SaveChanges();

            //When
            var result = queryHandler.Search(null);

            //Then
            Assert.AreEqual(taxClasses[0].hierarchyClassID, result[taxClasses[0].hierarchyClassName.Substring(0, 7)]);
            Assert.AreEqual(taxClasses[1].hierarchyClassID, result[taxClasses[1].hierarchyClassName.Substring(0, 7)]);
            Assert.AreEqual(taxClasses[2].hierarchyClassID, result[taxClasses[2].hierarchyClassName.Substring(0, 7)]);
        }
    }
}
