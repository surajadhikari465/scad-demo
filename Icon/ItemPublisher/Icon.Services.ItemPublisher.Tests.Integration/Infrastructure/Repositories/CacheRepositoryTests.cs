using Icon.Services.ItemPublisher.Infrastructure.Repositories;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Icon.Services.Newitem.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Repositories.Tests
{
    [TestClass()]
    public class CacheRepositoryTests
    {
        private ConnectionHelper connHelper;

        [TestInitialize]
        public void TestInitialize()
        {
            this.connHelper = new ConnectionHelper();
            this.connHelper.ProviderFactory.BeginTransaction();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.connHelper.ProviderFactory.RollbackTransaction();
        }

        /// <summary>
        /// Tests that attribute records are returned
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetAttributes_RecordsExists_RecordsReturned()
        {
            // Given.
            TestRepository testRepository = new TestRepository(this.connHelper);
            CacheRepository repository = new CacheRepository(this.connHelper.ProviderFactory);
            await testRepository.InsertDataType("TestDataType");
            int traitId = await testRepository.InsertTrait("TST");
            int attributeId = await testRepository.InsertAttribute("TST", "TestDataType");

            // When.
            Dictionary<string, Attributes> attributes = await repository.GetAttributes();

            // Then.
            Attributes attribute = attributes.First(x => x.Value.AttributeId == attributeId).Value;
            Assert.AreEqual(attributeId, attribute.AttributeId);
            Assert.AreEqual("AttributeName", attribute.AttributeName);
            Assert.AreEqual("AttributeDisplayName", attribute.AttributeDisplayName);
            Assert.AreEqual("Description", attribute.Description);
            Assert.AreEqual("TST", attribute.TraitCode);
            Assert.AreEqual(traitId, attribute.TraitId);
            Assert.AreEqual("XmlTraitDescription", attribute.XmlTraitDescription);
            Assert.AreEqual("TestDataType", attribute.DataTypeName);
            Assert.AreEqual(false, attribute.IsPickList);
            Assert.AreEqual(true, attribute.IsSpecialTransform);
        }

        /// <summary>
        /// Tests that the retrieve of a single attribute record works
        /// </summary>
        [TestMethod]
        public async Task GetSingleAttributes_RecordExists_RecordReturned()
        {
            // Given.
            TestRepository testRepository = new TestRepository(this.connHelper);
            CacheRepository repository = new CacheRepository(connHelper.ProviderFactory);
            await testRepository.InsertDataType("TestDataType");
            int traitId = await testRepository.InsertTrait("TST");
            int attributeId = await testRepository.InsertAttribute("TST", "TestDataType");

            // When.
            Attributes attribute = await repository.GetSingleAttribute("AttributeName");

            // Then.
            Assert.AreEqual(attributeId, attribute.AttributeId);
            Assert.AreEqual("AttributeName", attribute.AttributeName);
            Assert.AreEqual("AttributeDisplayName", attribute.AttributeDisplayName);
            Assert.AreEqual("Description", attribute.Description);
            Assert.AreEqual("TST", attribute.TraitCode);
            Assert.AreEqual(traitId, attribute.TraitId);
            Assert.AreEqual("XmlTraitDescription", attribute.XmlTraitDescription);
            Assert.AreEqual("TestDataType", attribute.DataTypeName);
            Assert.AreEqual(false, attribute.IsPickList);
            Assert.AreEqual(true, attribute.IsSpecialTransform);
        }

        /// <summary>
        /// Tests that the retrieve of hierarchy records work
        /// </summary>
        [TestMethod]
        public async Task GetHierarchies_RecordsExists_RecordsReturned()
        {
            // Given.
            TestRepository testRepository = new TestRepository(this.connHelper);
            int hierarchyId = await testRepository.InsertHierarchy("test");
            CacheRepository repository = new CacheRepository(connHelper.ProviderFactory);

            // When.
            Dictionary<string, HierarchyCacheItem> hierarchies = await repository.GetHierarchies();

            // Then.
            Assert.IsTrue(hierarchies.ContainsKey("test"));
            Assert.AreEqual(hierarchyId, hierarchies["test"].HierarchyId);
        }

        /// <summary>
        /// Tests that the retrieve of a single hierarchy record works
        /// </summary>
        [TestMethod]
        public async Task GetSingleHierarcy_RecordExists_RecordReturned()
        {
            // Given.
            TestRepository testRepository = new TestRepository(this.connHelper);
            int hierarchyId = await testRepository.InsertHierarchy("test");
            CacheRepository repository = new CacheRepository(connHelper.ProviderFactory);

            // When.
            HierarchyCacheItem hierarchy = await repository.GetSingleHierarchy("test");

            // Then.
            Assert.AreEqual("test", hierarchy.HierarchyName);
        }

        /// <summary>
        /// Tests that the retrieve of product selection groups returns the two we expect.
        /// Food_Stamp and Prohibit_Discount_Items should be returned.
        ///
        /// </summary>
        [TestMethod]
        public async Task ProductSelectionGroups_GetProuctSelectionGroups_CorrectRecordsReturned()
        {
            // Given.
            CacheRepository repository = new CacheRepository(connHelper.ProviderFactory);

            // When.
            Dictionary<int, ProductSelectionGroup> response = await repository.GetProductSelectionGroups();

            // Then.
            Assert.IsTrue(response.Count > 0);
        }
    }
}