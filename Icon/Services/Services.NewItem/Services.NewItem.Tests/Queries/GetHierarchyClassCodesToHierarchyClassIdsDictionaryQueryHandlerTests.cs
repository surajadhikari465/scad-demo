using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.NewItem.Queries;
using Icon.Framework;
using System.Data.Entity;
using System.Collections.Generic;
using Icon.Common.Context;
using Moq;

namespace Services.NewItem.Tests.Queries
{
    [TestClass]
    public class GetHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandlerTests
    {
        private GetHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler queryHandler;
        private GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery query;
        private IconContext context;
        private DbContextTransaction transaction;
        private Mock<IRenewableContext<IconContext>> mockRenewableContext;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();

            mockRenewableContext = new Mock<IRenewableContext<IconContext>>();
            mockRenewableContext.SetupGet(m => m.Context).Returns(context);

            queryHandler = new GetHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler(mockRenewableContext.Object);
            query = new GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void GetHierarchyClassCodesToHierarchyClassIdsDictionary_TaxHierarchy_ShouldReturnTaxCodesToHierarchyClassIds()
        {
            //Given
            var hierarchyClass1 = new HierarchyClass
            {
                hierarchyID = Hierarchies.Tax,
                hierarchyClassName = "1111111 Test Tax",
                hierarchyLevel = 1
            };
            var hierarchyClass2 = new HierarchyClass
            {
                hierarchyID = Hierarchies.Tax,
                hierarchyClassName = "1111112 Test Tax",
                hierarchyLevel = 1
            };
            var hierarchyClass3 = new HierarchyClass
            {
                hierarchyID = Hierarchies.Tax,
                hierarchyClassName = "1111113 Test Tax",
                hierarchyLevel = 1
            };

            query.HierarchyId = Hierarchies.Tax;
            context.HierarchyClass.AddRange(
                new List<HierarchyClass> {
                    hierarchyClass1,
                    hierarchyClass2,
                    hierarchyClass3
                });
            context.SaveChanges();

            //When
            var results = queryHandler.Search(query);

            //Then
            Assert.AreEqual(hierarchyClass1.hierarchyClassID, results[hierarchyClass1.hierarchyClassName.Substring(0, 7)]);
            Assert.AreEqual(hierarchyClass2.hierarchyClassID, results[hierarchyClass2.hierarchyClassName.Substring(0, 7)]);
            Assert.AreEqual(hierarchyClass3.hierarchyClassID, results[hierarchyClass3.hierarchyClassName.Substring(0, 7)]);
        }

        [TestMethod]
        public void GetHierarchyClassCodesToHierarchyClassIdsDictionary_NationalHierarchy_ShouldReturnNationalCodesToHeirarchyClassIds()
        {
            //Given
            var hierarchyClass1 = new HierarchyClass
            {
                hierarchyID = Hierarchies.National,
                hierarchyClassName = "Test National 1",
                hierarchyLevel = HierarchyLevels.NationalClass,
                HierarchyClassTrait = new List<HierarchyClassTrait> { new HierarchyClassTrait { traitID = Traits.NationalClassCode, traitValue = "112345" } }
            };
            var hierarchyClass2 = new HierarchyClass
            {
                hierarchyID = Hierarchies.National,
                hierarchyClassName = "Test National 2",
                hierarchyLevel = HierarchyLevels.NationalClass,
                HierarchyClassTrait = new List<HierarchyClassTrait> { new HierarchyClassTrait { traitID = Traits.NationalClassCode, traitValue = "212345" } }
            };
            var hierarchyClass3 = new HierarchyClass
            {
                hierarchyID = Hierarchies.National,
                hierarchyClassName = "Test National 3",
                hierarchyLevel = HierarchyLevels.NationalClass,
                HierarchyClassTrait = new List<HierarchyClassTrait> { new HierarchyClassTrait { traitID = Traits.NationalClassCode, traitValue = "312345" } }
            };

            query.HierarchyId = Hierarchies.National;
            context.HierarchyClass.AddRange(
                new List<HierarchyClass> {
                    hierarchyClass1,
                    hierarchyClass2,
                    hierarchyClass3
                });
            context.SaveChanges();

            //When
            var results = queryHandler.Search(query);

            //Then
            Assert.AreEqual(hierarchyClass1.hierarchyClassID, results["112345"]);
            Assert.AreEqual(hierarchyClass2.hierarchyClassID, results["212345"]);
            Assert.AreEqual(hierarchyClass3.hierarchyClassID, results["312345"]);
        }
    }
}
