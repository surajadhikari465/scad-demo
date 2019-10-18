using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.Models;
using Mammoth.Esb.HierarchyClassListener.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Testing.Core;

namespace Mammoth.Esb.HierarchyClassListener.Tests.Queries
{
    [TestClass]
    public class GetItemsByBrandIdQueryHandlerTests
    {
        private SqlDbProvider dbProvider;
        private DapperSqlFactory dapperSqlFactory;
        private ObjectBuilderFactory objectBuilderFactory;
        private GetItemsByBrandIdQueryHandler getItemsByBrandQueryHandler;
        private HierarchyClass brand;
        private List<Item> expectedItems = new List<Item>();

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();

            getItemsByBrandQueryHandler = new GetItemsByBrandIdQueryHandler(dbProvider);

            var assembly = Assembly.GetExecutingAssembly();
            dapperSqlFactory = new DapperSqlFactory(assembly);
            objectBuilderFactory = new ObjectBuilderFactory(assembly);

            AddBrand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void GetItemsByBrandId_ItemsAssociatedToBrands_ReturnsListOfItems()
        {
            // Given
            AddItems(numberOfItems: 3, associateToAddedBrand: true);
            GetItemsByBrandIdParameter queryParameters = new GetItemsByBrandIdParameter();
            queryParameters.BrandIds = new List<int> { this.brand.HierarchyClassID };

            // When
            var actual = this.getItemsByBrandQueryHandler.Search(queryParameters).OrderBy(x => x.ItemID).ToList();

            // Then
            Assert.AreEqual(expectedItems.Count, actual.Count, "Correct Number of items were not returned by query");
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expectedItems[i].ItemID, actual[i].ItemID);
                Assert.AreEqual(expectedItems[i].ItemTypeID, actual[i].ItemTypeID);
                Assert.AreEqual(expectedItems[i].ScanCode, actual[i].ScanCode);
                Assert.AreEqual(expectedItems[i].HierarchyMerchandiseID, actual[i].HierarchyMerchandiseID);
                Assert.AreEqual(expectedItems[i].HierarchyNationalClassID, actual[i].HierarchyNationalClassID);
                Assert.AreEqual(expectedItems[i].BrandHCID, actual[i].BrandHCID);
                Assert.AreEqual(expectedItems[i].TaxClassHCID, actual[i].TaxClassHCID);
                Assert.AreEqual(expectedItems[i].PSNumber, actual[i].PSNumber);
                Assert.AreEqual(expectedItems[i].Desc_Product, actual[i].Desc_Product);
                Assert.AreEqual(expectedItems[i].Desc_POS, actual[i].Desc_POS);
                Assert.AreEqual(expectedItems[i].PackageUnit, actual[i].PackageUnit);
                Assert.AreEqual(expectedItems[i].RetailSize, actual[i].RetailSize);
                Assert.AreEqual(expectedItems[i].RetailUOM, actual[i].RetailUOM);
                Assert.AreEqual(expectedItems[i].FoodStampEligible, actual[i].FoodStampEligible);
                Assert.AreEqual(expectedItems[i].AddedDate.ToString(), actual[i].AddedDate.ToString());
                Assert.AreEqual(expectedItems[i].ModifiedDate.ToString(), actual[i].ModifiedDate.ToString());
            }
        }

        [TestMethod]
        public void GetItemsByBrandId_ItemsNotAssociatedToBrands_ReturnsNoItems()
        {
            // Given
            AddItems(numberOfItems: 3, associateToAddedBrand: false);
            GetItemsByBrandIdParameter queryParameters = new GetItemsByBrandIdParameter();
            queryParameters.BrandIds = new List<int> { this.brand.HierarchyClassID };

            // When
            var actual = this.getItemsByBrandQueryHandler.Search(queryParameters).ToList();

            // Then
            Assert.AreEqual(0, actual.Count, "Items were returned when they were not supposed to be.");
        }

        private void AddBrand()
        {
            int maxHierarchyClassId = this.dbProvider.Connection
                .Query<int>("SELECT MAX(HierarchyClassID) FROM HierarchyClass", transaction: this.dbProvider.Transaction)
                .FirstOrDefault();
            this.brand = this.objectBuilderFactory
                .Build<HierarchyClass>()
                .With(hc => hc.HierarchyClassID, maxHierarchyClassId + 1)
                .With(hc => hc.HierarchyClassName, "Mammoth BrandDelete Test")
                .With(hc => hc.HierarchyID, Hierarchies.Brands)
                .With(hc => hc.AddedDate, DateTime.UtcNow);
            string sql = dapperSqlFactory.BuildInsertSql<HierarchyClass>(false);
            this.dbProvider.Connection.Execute(sql, this.brand, this.dbProvider.Transaction);
        }

        private void AddItems(int numberOfItems, bool associateToAddedBrand)
        {
            int maxItemId = this.dbProvider.Connection
                .Query<int>("SELECT MAX(ItemID) FROM Items", transaction: this.dbProvider.Transaction)
                .FirstOrDefault();

            for (int i = 0; i < numberOfItems; i++)
            {
                Item item = this.objectBuilderFactory
                .Build<Item>()
                .With(x => x.BrandHCID, associateToAddedBrand ? this.brand.HierarchyClassID : maxItemId)
                .With(x => x.Desc_POS, $"HC Brand Delete Item {i}")
                .With(x => x.Desc_Product, $"HC Brand Delete Item {i}")
                .With(x => x.ItemID, maxItemId + i + 1)
                .With(x => x.ScanCode, $"333377333{i}")
                .With(x => x.AddedDate, DateTime.UtcNow);
                this.expectedItems.Add(item);
            }

            string sql = @"
                            INSERT INTO Items
                            (
                                [ItemID],
                                [ItemTypeID],
                                [ScanCode],
                                [HierarchyMerchandiseID],
                                [HierarchyNationalClassID],
                                [BrandHCID],
                                [TaxClassHCID],
                                [PSNumber],
                                [Desc_Product],
                                [Desc_POS],
                                [PackageUnit],
                                [RetailSize],
                                [RetailUOM],
                                [FoodStampEligible],
                                [AddedDate],
                                [ModifiedDate]
                            )
                            VALUES
                            (
                                @ItemID,
                                @ItemTypeID,
                                @ScanCode,
                                @HierarchyMerchandiseID,
                                @HierarchyNationalClassID,
                                @BrandHCID,
                                @TaxClassHCID,
                                @PSNumber,
                                @Desc_Product,
                                @Desc_POS,
                                @PackageUnit,
                                @RetailSize,
                                @RetailUOM,
                                @FoodStampEligible,
                                @AddedDate,
                                @ModifiedDate
                            )";
            this.dbProvider.Connection.Execute(sql, this.expectedItems, this.dbProvider.Transaction);
        }
    }
}
