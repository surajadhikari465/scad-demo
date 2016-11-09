using Dapper;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Esb.ProductListener.Commands;
using Mammoth.Esb.ProductListener.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Mammoth.Esb.ProductListener.Tests.Commands
{
    [TestClass]
    public class AddOrUpdateProductsCommandHandlerTests
    {
        private AddOrUpdateProductsCommandHandler commandHandler;
        private SqlDbProvider dbProvider;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();

            commandHandler = new AddOrUpdateProductsCommandHandler(dbProvider);
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        [Ignore] // Need to write test for not localdb
        public void AddOrUpdateProducts_ProductExist_ShouldUpdateProduct()
        {
            var merch = dbProvider.Connection.Query<dynamic>(
                "Select top 1 * from dbo.Hierarchy_Merchandise",
                null,
                dbProvider.Transaction).First();

            var nat = dbProvider.Connection.Query<dynamic>(
                "Select top 1 * from dbo.Hierarchy_NationalClass",
                null,
                dbProvider.Transaction).First();

            //Given
            var itemId = dbProvider.Connection.Query<int>(
                @"INSERT INTO dbo.Items(ItemID, ItemTypeID, ScanCode, HierarchyMerchandiseID, HierarchyNationalClassID, BrandHCID, TaxClassHCID, PSNumber, Desc_Product, Desc_POS, PackageUnit, RetailSize, RetailUOM, FoodStampEligible)
                        OUTPUT INSERTED.ItemID        
                  VALUES (20000000, 1, 1234, 1, 1, 1, 1, 1, 'Test Desc', 'Test POS Desc', '1', '1', 'EA', 0)",
                transaction: dbProvider.Transaction)
                .First();
            ProductModel product = new ProductModel
            {
                ItemID = itemId,
                ItemTypeID = 2,
                ScanCode = "12345",
                SubBrickID = merch.SubBrickHCID,
                NationalClassID = nat.ClassHCID,
                BrandHCID = 2,
                TaxClassHCID = 2,
                PSNumber = 2,
                Desc_Product = "Test Desc Updated",
                Desc_POS = "Test POS Desc Updated",
                PackageUnit = "2",
                RetailSize = "2",
                RetailUOM = "OZ",
                FoodStampEligible = true
            };

            //When
            commandHandler.Execute(new AddOrUpdateProductsCommand { Products = new List<ProductModel> { product } });

            //Then
            var item = dbProvider.Connection.Query<dynamic>(
                "SELECT * FROM dbo.Items WHERE ItemID = @ItemId",
                new { ItemId = itemId },
                dbProvider.Transaction)
                .Single();
            Assert.AreEqual(product.ItemTypeID, item.ItemTypeID);
            Assert.AreEqual(merch.HierarchyMerchandiseID, item.HierarchyMerchandiseID);
            Assert.AreEqual(nat.HierarchyNationalClassID, item.HierarchyNationalClassID);
            Assert.AreEqual(product.BrandHCID, item.BrandHCID);
            Assert.AreEqual(product.TaxClassHCID, item.TaxClassHCID);
            Assert.AreEqual(product.PSNumber, item.PSNumber);
            Assert.AreEqual(product.Desc_Product, item.Desc_Product);
            Assert.AreEqual(product.Desc_POS, item.Desc_POS);
            Assert.AreEqual(product.PackageUnit, item.PackageUnit);
            Assert.AreEqual(product.RetailSize, item.RetailSize);
            Assert.AreEqual(product.RetailUOM, item.RetailUOM);
            Assert.IsNotNull(item.AddedDate);
        }

        [TestMethod]
        [Ignore] // Need to write test for non local db
        public void AddOrUpdateProducts_ProductDoesntExist_ShouldAddProduct()
        {
            var merch = dbProvider.Connection.Query<dynamic>(
                "Select top 1 * from dbo.Hierarchy_Merchandise",
                null,
                dbProvider.Transaction).First();

            var nat = dbProvider.Connection.Query<dynamic>(
                "Select top 1 * from dbo.Hierarchy_NationalClass",
                null,
                dbProvider.Transaction).First();

            //Given
            ProductModel product = new ProductModel
            {
                ItemID = 9999999,
                ItemTypeID = 2,
                ScanCode = "12345",
                SubBrickID = merch.SubBrickHCID,
                NationalClassID = nat.ClassHCID,
                BrandHCID = 2,
                TaxClassHCID = 2,
                PSNumber = 2,
                Desc_Product = "Test Desc Updated",
                Desc_POS = "Test POS Desc Updated",
                PackageUnit = "2",
                RetailSize = "2",
                RetailUOM = "OZ",
                FoodStampEligible = true
            };

            //When
            commandHandler.Execute(new AddOrUpdateProductsCommand { Products = new List<ProductModel> { product } });

            //Then
            var item = dbProvider.Connection.Query<dynamic>(
                "SELECT * FROM dbo.Items WHERE ScanCode = @ScanCode",
                new { ScanCode = product.ScanCode },
                dbProvider.Transaction)
                .Single();
            Assert.AreEqual(product.ItemTypeID, item.ItemTypeID);
            Assert.AreEqual(product.ScanCode, item.ScanCode);
            Assert.AreEqual(merch.HierarchyMerchandiseID, item.HierarchyMerchandiseID);
            Assert.AreEqual(nat.HierarchyNationalClassID, item.HierarchyNationalClassID);
            Assert.AreEqual(product.BrandHCID, item.BrandHCID);
            Assert.AreEqual(product.TaxClassHCID, item.TaxClassHCID);
            Assert.AreEqual(product.PSNumber, item.PSNumber);
            Assert.AreEqual(product.Desc_Product, item.Desc_Product);
            Assert.AreEqual(product.Desc_POS, item.Desc_POS);
            Assert.AreEqual(product.PackageUnit, item.PackageUnit);
            Assert.AreEqual(product.RetailSize, item.RetailSize);
            Assert.AreEqual(product.RetailUOM, item.RetailUOM);
            Assert.IsNotNull(item.AddedDate);
        }
    }
}
