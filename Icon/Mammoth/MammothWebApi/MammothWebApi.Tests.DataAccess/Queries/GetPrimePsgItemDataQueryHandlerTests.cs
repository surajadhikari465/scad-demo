using Dapper;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Testing.Core;
using MammothWebApi.Common;
using Moq;

namespace MammothWebApi.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetPrimePsgItemDataQueryHandlerTests
    {
        private IDbProvider db;
        private GetPrimePsgItemDataByScanCodeQuery query;
        private GetPrimePsgItemDataByScanCodeQueryHandler queryHandler;
        private string scanCode1 = "8004005002003";
        private string scanCode2 = "8004005002004";
        private int expectedItem1ID = 900000001;
        private int expectedItem2ID = 900000002;
        private int expectedStore1BusID = 70001;
        private int expectedStore2BusID = 80001;
        private Mock<IPrimeAffinityPsgSettings> settings;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            db = new SqlDbProvider();
            db.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();

            this.settings = new Mock<IPrimeAffinityPsgSettings>();
            this.query = new GetPrimePsgItemDataByScanCodeQuery();
            this.queryHandler = new GetPrimePsgItemDataByScanCodeQueryHandler(this.db, this.settings.Object);
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            this.queryHandler = null;
            if (this.db.Transaction != null)
            {
                this.db.Transaction.Rollback();
                this.db.Transaction.Dispose();
            }
            this.db.Connection.Dispose();
        }

        [TestMethod]
        public void GetPrimePsgItemData_ScanCodesExistInDatabase_ReturnsItemData()
        {
            // Given
            var objectFactory = new ObjectBuilderFactory(this.GetType().Assembly);
            var dapperFactory = new DapperSqlFactory(this.GetType().Assembly);
            string expectedItemTypeCode = "TST";
            int expectedPSNumber = 1000;
            string expectedStoreName1 = "My Test Store Psg 1";
            string expectedStoreName2 = "My Test Store Psg 2";
            string expectedRegion1 = "FL";
            string expectedRegion2 = "FL";

            string insertItemTypeSql = @"INSERT INTO dbo.ItemTypes (itemTypeCode, itemTypeDesc, AddedDate) VALUES (@itemTypeCode, @itemTypeDesc, @AddedDate) SELECT SCOPE_IDENTITY()";
            string insertItemSql = @"INSERT INTO dbo.Items (ItemID, ItemTypeID, ScanCode, Desc_Product, PSNumber, AddedDate) VALUES (@ItemID, @ItemTypeID, @ScanCode, @Desc_Product, @PSNumber, @AddedDate) SELECT SCOPE_IDENTITY()";

            this.db.Connection.Execute(
                dapperFactory.BuildInsertSql<Locales>(false),
                objectFactory.Build<Locales>()
                    .With(l => l.BusinessUnitID, expectedStore1BusID)
                    .With(l => l.Region, "FL")
                    .With(l => l.StoreName, expectedStoreName1)
                    .With(l => l.StoreAbbrev, "MTS1").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                dapperFactory.BuildInsertSql<Locales>(false),
                objectFactory.Build<Locales>()
                    .With(l => l.BusinessUnitID, expectedStore2BusID)
                    .With(l => l.Region, "FL")
                    .With(l => l.StoreName, expectedStoreName2)
                    .With(l => l.StoreAbbrev, "MTS2").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertItemTypeSql,
                objectFactory.Build<ItemType>()
                    .With(t => t.ItemTypeCode, expectedItemTypeCode)
                    .With(t => t.ItemTypeDesc, "Test Type")
                    .With(t => t.AddedDate, DateTime.Now)
                    .CreatedObject,
                this.db.Transaction);

            int itemTypeId = this.db.Connection.Query<int>("SELECT itemTypeID FROM ItemTypes WHERE itemTypeCode = @code",
                    new { code = expectedItemTypeCode }, this.db.Transaction)
                .First();

            this.db.Connection.Execute(
                insertItemSql,
                objectFactory.Build<Item>()
                    .With(i => i.ItemID, expectedItem1ID)
                    .With(i => i.ItemTypeID, itemTypeId)
                    .With(i => i.ScanCode, scanCode1)
                    .With(i => i.Desc_Product, "My Test Product 1")
                    .With(i => i.PSNumber, expectedPSNumber).CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertItemSql,
                objectFactory.Build<Item>()
                    .With(i => i.ItemID, expectedItem2ID)
                    .With(i => i.ItemTypeID, itemTypeId)
                    .With(i => i.ScanCode, scanCode2)
                    .With(i => i.Desc_Product, "My Test Product 2")
                    .With(i => i.PSNumber, expectedPSNumber).CreatedObject,
                this.db.Transaction);

            this.query = new GetPrimePsgItemDataByScanCodeQuery
            {
                StoreScanCodes = new List<StoreScanCode>
                {
                    new StoreScanCode { ScanCode = scanCode1, BusinessUnitID = expectedStore1BusID },
                    new StoreScanCode { ScanCode = scanCode2, BusinessUnitID = expectedStore2BusID }
                }
            };

            // When
            var primePsgItemData = this.queryHandler.Search(this.query);

            // Then
            var itemData1 = primePsgItemData.Single(d => d.ItemId == expectedItem1ID);
            Assert.AreEqual(expectedItemTypeCode, itemData1.ItemTypeCode);
            Assert.AreEqual(scanCode1, itemData1.ScanCode);
            Assert.AreEqual(expectedPSNumber, itemData1.PsSubTeamNumber);
            Assert.AreEqual(expectedStore1BusID, itemData1.BusinessUnitId);
            Assert.AreEqual(expectedStoreName1, itemData1.StoreName);
            Assert.AreEqual(expectedRegion1, itemData1.Region);

            var itemData2 = primePsgItemData.Single(d => d.ItemId == expectedItem2ID);
            Assert.AreEqual(expectedItemTypeCode, itemData2.ItemTypeCode);
            Assert.AreEqual(scanCode2, itemData2.ScanCode);
            Assert.AreEqual(expectedPSNumber, itemData2.PsSubTeamNumber);
            Assert.AreEqual(expectedStore2BusID, itemData2.BusinessUnitId);
            Assert.AreEqual(expectedStoreName2, itemData2.StoreName);
            Assert.AreEqual(expectedRegion2, itemData2.Region);
        }

        [TestMethod]
        public void GetPrimePsgItemData_1ScanCodeIsAssociatedToAnExcludedSubTeam_DoesNotReturnExcludeSubTeamItemData()
        {
            // Given
            List<int> excludedSubTeams = new List<int> { 2200 };
            settings.SetupGet(m => m.ExcludedPsNumbers)
                .Returns(excludedSubTeams);

            var objectFactory = new ObjectBuilderFactory(this.GetType().Assembly);
            var dapperFactory = new DapperSqlFactory(this.GetType().Assembly);
            string expectedItemTypeCode = "TST";
            int expectedPSNumber = 1000;
            string expectedStoreName1 = "My Test Store Psg 1";
            string expectedStoreName2 = "My Test Store Psg 2";
            string expectedRegion1 = "FL";

            string insertItemTypeSql = @"INSERT INTO dbo.ItemTypes (itemTypeCode, itemTypeDesc, AddedDate) VALUES (@itemTypeCode, @itemTypeDesc, @AddedDate) SELECT SCOPE_IDENTITY()";
            string insertItemSql = @"INSERT INTO dbo.Items (ItemID, ItemTypeID, ScanCode, Desc_Product, PSNumber, AddedDate) VALUES (@ItemID, @ItemTypeID, @ScanCode, @Desc_Product, @PSNumber, @AddedDate) SELECT SCOPE_IDENTITY()";

            this.db.Connection.Execute(
                dapperFactory.BuildInsertSql<Locales>(false),
                objectFactory.Build<Locales>()
                    .With(l => l.BusinessUnitID, expectedStore1BusID)
                    .With(l => l.Region, "FL")
                    .With(l => l.StoreName, expectedStoreName1)
                    .With(l => l.StoreAbbrev, "MTS1").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                dapperFactory.BuildInsertSql<Locales>(false),
                objectFactory.Build<Locales>()
                    .With(l => l.BusinessUnitID, expectedStore2BusID)
                    .With(l => l.Region, "FL")
                    .With(l => l.StoreName, expectedStoreName2)
                    .With(l => l.StoreAbbrev, "MTS2").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertItemTypeSql,
                objectFactory.Build<ItemType>()
                    .With(t => t.ItemTypeCode, expectedItemTypeCode)
                    .With(t => t.ItemTypeDesc, "Test Type")
                    .With(t => t.AddedDate, DateTime.Now)
                    .CreatedObject,
                this.db.Transaction);

            int itemTypeId = this.db.Connection.Query<int>("SELECT itemTypeID FROM ItemTypes WHERE itemTypeCode = @code",
                    new { code = expectedItemTypeCode }, this.db.Transaction)
                .First();

            this.db.Connection.Execute(
                insertItemSql,
                objectFactory.Build<Item>()
                    .With(i => i.ItemID, expectedItem1ID)
                    .With(i => i.ItemTypeID, itemTypeId)
                    .With(i => i.ScanCode, scanCode1)
                    .With(i => i.Desc_Product, "My Test Product 1")
                    .With(i => i.PSNumber, expectedPSNumber).CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertItemSql,
                objectFactory.Build<Item>()
                    .With(i => i.ItemID, expectedItem2ID)
                    .With(i => i.ItemTypeID, itemTypeId)
                    .With(i => i.ScanCode, scanCode2)
                    .With(i => i.Desc_Product, "My Test Product 2")
                    .With(i => i.PSNumber, excludedSubTeams[0]).CreatedObject,
                this.db.Transaction);

            this.query = new GetPrimePsgItemDataByScanCodeQuery
            {
                StoreScanCodes = new List<StoreScanCode>
                {
                    new StoreScanCode { ScanCode = scanCode1, BusinessUnitID = expectedStore1BusID },
                    new StoreScanCode { ScanCode = scanCode2, BusinessUnitID = expectedStore2BusID }
                }
            };

            // When
            var primePsgItemData = this.queryHandler.Search(this.query);

            // Then
            Assert.AreEqual(1, primePsgItemData.Count());
            var itemData1 = primePsgItemData.Single(d => d.ItemId == expectedItem1ID);
            Assert.AreEqual(expectedItemTypeCode, itemData1.ItemTypeCode);
            Assert.AreEqual(scanCode1, itemData1.ScanCode);
            Assert.AreEqual(expectedPSNumber, itemData1.PsSubTeamNumber);
            Assert.AreEqual(expectedStore1BusID, itemData1.BusinessUnitId);
            Assert.AreEqual(expectedStoreName1, itemData1.StoreName);
            Assert.AreEqual(expectedRegion1, itemData1.Region);
        }
    }
}
