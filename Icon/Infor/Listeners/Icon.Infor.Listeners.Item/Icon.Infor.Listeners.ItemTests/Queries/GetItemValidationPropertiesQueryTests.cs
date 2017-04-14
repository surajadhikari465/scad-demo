using Icon.Common.Context;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Commands;
using Icon.Infor.Listeners.Item.Models;
using Icon.Infor.Listeners.Item.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Tests.Commands
{
    [TestClass]
    public class GetItemValidationPropertiesQueryTests
    {
        private GetItemValidationPropertiesQuery queryHandler;
        private GetItemValidationPropertiesParameters parameters;
        private Mock<IRenewableContext<IconContext>> mockGlobalContext;
        private IconContext context;
        private DbContextTransaction transaction;
        private ItemModel testItem;
        private HierarchyClass testBrand;
        private HierarchyClass testSubTeam;
        private HierarchyClass testSubBrick;
        private HierarchyClass testNationalClass;
        private HierarchyClass testTax;
        private DateTime testDatabaseModifiedDate;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();

            mockGlobalContext = new Mock<IRenewableContext<IconContext>>();
            mockGlobalContext.SetupGet(m => m.Context).Returns(context);

            queryHandler = new GetItemValidationPropertiesQuery(mockGlobalContext.Object);
            parameters = new GetItemValidationPropertiesParameters();

            testItem = new ItemModel { ItemId = 999999999 };
            parameters.Items = new List<ItemModel> { testItem };
            SetupTestItems();
        }

        private void SetupTestItems()
        {
            testBrand = context.HierarchyClass.Add(new HierarchyClass { hierarchyClassName = "Test Brand", hierarchyID = Hierarchies.Brands, hierarchyLevel = HierarchyLevels.Brand });
            testSubTeam = context.HierarchyClass.Add(new HierarchyClass { hierarchyClassName = "Test Sub Team (1234)", hierarchyID = Hierarchies.Financial, hierarchyLevel = HierarchyLevels.Financial });
            testSubBrick = context.HierarchyClass.Add(new HierarchyClass { hierarchyClassName = "Test Sub Brick", hierarchyID = Hierarchies.Merchandise, hierarchyLevel = HierarchyLevels.SubBrick });
            testNationalClass = context.HierarchyClass.Add(new HierarchyClass { hierarchyClassName = "Test National Class", hierarchyID = Hierarchies.National, hierarchyLevel = HierarchyLevels.NationalClass });
            testTax = context.HierarchyClass.Add(new HierarchyClass { hierarchyClassName = "0123456 Test Tax", hierarchyID = Hierarchies.Tax, hierarchyLevel = HierarchyLevels.Tax });
            context.SaveChanges();

            testItem.BrandsHierarchyClassId = testBrand.hierarchyClassID.ToString();
            testItem.FinancialHierarchyClassId = "1234";
            testItem.MerchandiseHierarchyClassId = testSubBrick.hierarchyClassID.ToString();
            testItem.NationalHierarchyClassId = testNationalClass.hierarchyClassID.ToString();
            testItem.TaxHierarchyClassId = "0123456";
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void GetItemValidationProperties_AllHierarchyClassesExist_AllPropertiesReturned()
        {
            //When
            var result = queryHandler.Search(parameters).Single();

            //Then
            Assert.AreEqual(testItem.ItemId, result.ItemId);
            Assert.IsNotNull(result.BrandId);
            Assert.IsNull(result.ModifiedDate);
            Assert.IsNotNull(result.NationalClassId);
            Assert.IsNotNull(result.SubBrickId);
            Assert.IsNotNull(result.SubTeamId);
            Assert.IsNotNull(result.TaxClassId);
        }

        [TestMethod]
        public void GetItemValidationProperties_BrandDoesNotExist_BrandError()
        {
            //Given
            context.HierarchyClass.Remove(testBrand);
            context.SaveChanges();

            //When
            var result = queryHandler.Search(parameters).Single();

            //Then
            Assert.AreEqual(testItem.ItemId, result.ItemId);
            Assert.IsNull(result.BrandId);
            Assert.IsNull(result.ModifiedDate);
            Assert.IsNotNull(result.NationalClassId);
            Assert.IsNotNull(result.SubBrickId);
            Assert.IsNotNull(result.SubTeamId);
            Assert.IsNotNull(result.TaxClassId);
        }

        [TestMethod]
        public void GetItemValidationProperties_SubTeamDoesNotExist_SubTeamIsNull()
        {
            //Given
            context.HierarchyClass.Remove(testSubTeam);
            context.SaveChanges();

            //When
            var result = queryHandler.Search(parameters).Single();

            //Then
            Assert.AreEqual(testItem.ItemId, result.ItemId);
            Assert.IsNotNull(result.BrandId);
            Assert.IsNull(result.ModifiedDate);
            Assert.IsNotNull(result.NationalClassId);
            Assert.IsNotNull(result.SubBrickId);
            Assert.IsNull(result.SubTeamId);
            Assert.IsNotNull(result.TaxClassId);
        }

        [TestMethod]
        public void GetItemValidationProperties_SubBrickDoesNotExist_SubBrickIsNull()
        {
            //Given
            context.HierarchyClass.Remove(testSubBrick);
            context.SaveChanges();

            //When
            var result = queryHandler.Search(parameters).Single();

            //Then
            Assert.AreEqual(testItem.ItemId, result.ItemId);
            Assert.IsNotNull(result.BrandId);
            Assert.IsNull(result.ModifiedDate);
            Assert.IsNotNull(result.NationalClassId);
            Assert.IsNull(result.SubBrickId);
            Assert.IsNotNull(result.SubTeamId);
            Assert.IsNotNull(result.TaxClassId);
        }

        [TestMethod]
        public void GetItemValidationProperties_TaxDoesNotExist_TaxIsNull()
        {
            //Given
            context.HierarchyClass.Remove(testTax);
            context.SaveChanges();

            //When
            var result = queryHandler.Search(parameters).Single();

            //Then
            Assert.AreEqual(testItem.ItemId, result.ItemId);
            Assert.IsNotNull(result.BrandId);
            Assert.IsNull(result.ModifiedDate);
            Assert.IsNotNull(result.NationalClassId);
            Assert.IsNotNull(result.SubBrickId);
            Assert.IsNotNull(result.SubTeamId);
            Assert.IsNull(result.TaxClassId);
        }

        [TestMethod]
        public void GetItemValidationProperties_NationalClassDoesNotExists_NationalClassIsNull()
        {
            //Given
            context.HierarchyClass.Remove(testNationalClass);
            context.SaveChanges();

            //When
            var result = queryHandler.Search(parameters).Single();

            //Then
            Assert.AreEqual(testItem.ItemId, result.ItemId);
            Assert.IsNotNull(result.BrandId);
            Assert.IsNull(result.ModifiedDate);
            Assert.IsNull(result.NationalClassId);
            Assert.IsNotNull(result.SubBrickId);
            Assert.IsNotNull(result.SubTeamId);
            Assert.IsNotNull(result.TaxClassId);
        }

        [TestMethod]
        public void GetItemValidationProperties_ModifiedDateDoesNotExistForItem_ModifiedDateIsNull()
        {
            //Given
            context.Database.ExecuteSqlCommand($@"
                SET IDENTITY_INSERT dbo.Item ON

                INSERT INTO Item (itemID, itemTypeID)
                VALUES ({testItem.ItemId}, {ItemTypes.RetailSale})

                SET IDENTITY_INSERT dbo.Item OFF");

            //When
            var result = queryHandler.Search(parameters).Single();

            //Then
            Assert.AreEqual(testItem.ItemId, result.ItemId);
            Assert.IsNotNull(result.BrandId);
            Assert.IsNull(result.ModifiedDate);
            Assert.IsNotNull(result.NationalClassId);
            Assert.IsNotNull(result.SubBrickId);
            Assert.IsNotNull(result.SubTeamId);
            Assert.IsNotNull(result.TaxClassId);
        }

        [TestMethod]
        public void GetItemValidationProperties_ModifiedDateDoesExist_ModifiedDateIsDatabaseValue()
        {
            //Given
            DateTime modifiedDate = DateTime.Now;
            string datetimeFormat = "yyyy-MM-dd HH:mm:ss.fffffff";

            context.Database.ExecuteSqlCommand($@"
                SET IDENTITY_INSERT dbo.Item ON

                INSERT INTO Item (itemID, itemTypeID)
                VALUES ({testItem.ItemId}, {ItemTypes.RetailSale})

                SET IDENTITY_INSERT dbo.Item OFF

                INSERT INTO ItemTrait (itemID, localeID, traitID, traitValue, uomID)
                VALUES ({testItem.ItemId}, {Locales.WholeFoods}, {Traits.ModifiedDate}, '{modifiedDate.ToString(datetimeFormat)}', NULL)");

            //When
            var result = queryHandler.Search(parameters).Single();

            //Then
            Assert.AreEqual(testItem.ItemId, result.ItemId);
            Assert.IsNotNull(result.BrandId);
            Assert.AreEqual(modifiedDate.ToString(datetimeFormat), result.ModifiedDate);
            Assert.IsNotNull(result.NationalClassId);
            Assert.IsNotNull(result.SubBrickId);
            Assert.IsNotNull(result.SubTeamId);
            Assert.IsNotNull(result.TaxClassId);
        }
    }
}
