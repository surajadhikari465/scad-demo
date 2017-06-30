using GlobalEventController.DataAccess.Queries;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GlobalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetItemIdentifiersQueryHandlerTests
    {
        private GetItemIdentifiersQueryHandler queryHandler;
        private IrmaDbContextFactory contextFactory;
        private IrmaContext context;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext();
            this.contextFactory = new IrmaDbContextFactory();
            this.queryHandler = new GetItemIdentifiersQueryHandler(contextFactory);
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.context.Dispose();
        }

        [TestMethod]
        public void GetItemIdentifiersQuery_ValidDefaultIdentifier_ReturnsExpectedIRMAItemIdentifierObject()
        {
            string expected = "9948245026"; // identifer 9948245026 is "365 OG PNUT BUTTER" in all regions except UK.
            // Given
            GetItemIdentifiersQuery query = new GetItemIdentifiersQuery { Predicate = ii => ii.Identifier == expected && ii.Deleted_Identifier == 0 };
            // When
            List<ItemIdentifier> actualList = queryHandler.Handle(query);

            // Then
            Assert.IsTrue(actualList.Count > 0, string.Format("Target identifier [{0}] not returned by query.", expected));
            Assert.IsTrue(actualList.Count == 1, string.Format("We expect a single matching identifier in IRMA, but [{0}] were returned by query.", actualList.Count));
            // Get item.
            ItemIdentifier actual = actualList[0];
            // We got correct identifier.
            Assert.AreEqual(actual.Identifier, expected, string.Format("We expected to get identifier [{0}], but got [{1}].", expected, actual.Identifier));

            // Has valid item key.
            Assert.IsTrue(actual.Item_Key > 0,
                string.Format("We got an identifier obj [{0}], but its item_key is not a positive integer: [{1}].", actual.Identifier, actual.Item_Key)
            );
        }

        [TestMethod]
        public void GetItemIdentifiersQuery_AlphaCharIdentifier_NoIdentifierObjectsReturned()
        {
            string expected = "irma";
            // Given
            GetItemIdentifiersQuery query = new GetItemIdentifiersQuery { Predicate = ii => ii.Identifier == expected && ii.Deleted_Identifier == 0 };

            // When
            List<ItemIdentifier> expectedList = queryHandler.Handle(query);

            // Then
            Assert.AreEqual(
                expectedList.Count,
                0,
                string.Format("We expected no identifier objects to be returned for invalid identifier [{0}], but our list contains [{1}] objects.", expected, expectedList.Count)
            );
        }
    }
}
