namespace Mammoth.Price.Controller.DataAccess.Tests.Queries
{
    using Dapper;
    using Irma.Framework;
    using Irma.Testing.Builders;
    using Mammoth.Common.DataAccess;
    using Mammoth.Common.DataAccess.DbProviders;
    using Mammoth.Price.Controller.DataAccess.Models;
    using Mammoth.Price.Controller.DataAccess.Queries;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    [TestClass]
    public class GetPricesQueryTests
    {
        private GetPriceDataQuery query;
        private GetPriceDataParameters parameters;
        private SqlDbProvider dbProvider;
        private string region = "FL";

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_FL"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction(IsolationLevel.Snapshot);

            query = new GetPriceDataQuery(dbProvider);
            parameters = new GetPriceDataParameters { Instance = 454, Region = region };
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void GetPriceEventsQuery_OneItemFor365RegionAndOneItemForRmRegion_ShouldReturnRowForEachItemWithRespectiveRegionAbbreviation()
        {
            // Given
            var expectedTSBusinessUnitId = 724;
            var expectedBusinessUnitId = 723;

            var expectedPrice = 4.00m;
            var expectedRmCurrentRegularPrice = 5.00M;
            byte expectedRmCurrentRegularMultiple = 1;

            var expectedTsPrice = 7.00m;
            var expectedTsCurrentRegularPrice = 6.00M;
            byte expectedTsCurrentRegularMultiple = 2;

            byte expectedMultiple = 1;
            var expectedIdentifier = "7777777777777";
            
            var expectedRmStartDate = DateTime.Today.AddDays(2);
            var expectedTsStartDate = DateTime.Today.AddDays(3);

            var expectedPriceType = "REG";
            byte expectedPriceTypeId = dbProvider.Connection.Query<byte>(
                "SELECT PriceChgTypeID FROM PriceChgType WHERE PriceChgTypeDesc = @Desc",
                new { Desc = expectedPriceType },
                dbProvider.Transaction).First();
            var expectedResultUnitAbbreviation = "EA";
            var expectedRetailUnitId = dbProvider.Connection.Query<int>(
                "SELECT Unit_ID FROM ItemUnit WHERE Unit_Abbreviation = @Unit",
                new { Unit = expectedResultUnitAbbreviation },
                dbProvider.Transaction).First();

            var subTeamNo = dbProvider.Connection.Query<int>(
                "SELECT Top 1 SubTeam_No FROM SubTeam",
                null,
                dbProvider.Transaction).First();

            Item newItem = new TestIrmaDbItemBuilder()
                .WithRetail_Unit_ID(expectedRetailUnitId)
                .WithSubTeam_No(subTeamNo);
            var itemKey = this.AddItemToDatabase(newItem);

            ItemIdentifier newItemIdentifier = new TestItemIdentifierBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifier);

            this.AddItemIdentifierToDatabase(newItemIdentifier);

            Store newStore = new TestStoreBuilder()
               .WithStore_No(11441112)
               .WithBusinessUnitId(expectedBusinessUnitId)
               .WithStoreJurisdictionId(1);

            this.AddNewStore(newStore);

            Store newTsStore = new TestStoreBuilder()
                .WithBusinessUnitId(expectedTSBusinessUnitId)
                .WithStoreJurisdictionId(1)
                .WithStore_No(8394829)
                .WithStore_Name("Test TS Store");

            this.AddNewStore(newTsStore);

            StoreRegionMapping newStoreRegionMapping = new TestStoreRegionMappingBuilder()
                .WithStore_No(newStore.Store_No)
                .WithRegion_Code(this.region);

            this.AddNewStoreRegionMapping(newStoreRegionMapping);

            StoreRegionMapping newTsStoreRegionMapping = new TestStoreRegionMappingBuilder()
                .WithStore_No(newTsStore.Store_No)
                .WithRegion_Code("TS");

            this.AddNewStoreRegionMapping(newTsStoreRegionMapping);

            Price existingRmPrice = new TestPriceBuilder()
                .WithPrice1(expectedRmCurrentRegularPrice)
                .WithMultiple(expectedRmCurrentRegularMultiple)
                .WithPriceChgTypeId(expectedPriceTypeId)
                .WithStore_No(newStore.Store_No)
                .WithItem_Key(itemKey);

            this.AddPriceToDatabase(existingRmPrice);

            Price existingTsPrice = new TestPriceBuilder()
                .WithPrice1(expectedTsCurrentRegularPrice)
                .WithMultiple(expectedTsCurrentRegularMultiple)
                .WithPriceChgTypeId(expectedPriceTypeId)
                .WithStore_No(newTsStore.Store_No)
                .WithItem_Key(itemKey);

            this.AddPriceToDatabase(existingTsPrice);

            PriceBatchDetail newRmPriceBatchDetail = new TestPriceBatchDetailBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifier)
                .WithPrice(expectedPrice)
                .WithMultiple(expectedMultiple)
                .WithPriceChgTypeID(expectedPriceTypeId)
                .WithStore_No(newStore.Store_No)
                .WithStartDate(expectedRmStartDate);

            int rmPriceBatchDetailId = this.AddPriceBatchDetail(newRmPriceBatchDetail);

            PriceBatchDetail newTsPriceBatchDetail = new TestPriceBatchDetailBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifier)
                .WithPrice(expectedTsPrice)
                .WithMultiple(expectedMultiple)
                .WithPriceChgTypeID(expectedPriceTypeId)
                .WithStore_No(newTsStore.Store_No)
                .WithStartDate(expectedTsStartDate);

            int tsPriceBatchDetailId = this.AddPriceBatchDetail(newTsPriceBatchDetail);

            ValidatedScanCode newValidScanCode = new TestValidatedScanCodeBuilder()
                .WithScanCode(expectedIdentifier);

            this.AddValidatedScanCode(newValidScanCode);

            var itemPriceEvents = new List<TestItemsForPriceEvents>();
            itemPriceEvents.Add(new TestItemsForPriceEvents
            {
                Item_Key = itemKey,
                Identifier = expectedIdentifier,
                Store_No = newStore.Store_No,
                PriceBatchDetailID = rmPriceBatchDetailId
            });
            itemPriceEvents.Add(new TestItemsForPriceEvents
            {
                Item_Key = itemKey,
                Identifier = expectedIdentifier,
                Store_No = newTsStore.Store_No,
                PriceBatchDetailID = tsPriceBatchDetailId
            });

            this.InsertIntoEventQueue(itemPriceEvents);

            // When
            var results = this.query.Search(this.parameters);
            var actualRm = results.First(r => r.Region == this.region);
            var actualTs = results.First(r => r.Region == "TS");

            // Then
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(expectedRmCurrentRegularPrice, actualRm.CurrentRegularPrice);
            Assert.AreEqual(expectedRmCurrentRegularMultiple, actualRm.CurrentRegularMultiple);
            Assert.AreEqual(expectedPrice, actualRm.NewRegularPrice);
            Assert.AreEqual(expectedMultiple, actualRm.NewRegularMultiple);
            Assert.AreEqual(expectedIdentifier, actualRm.ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, actualRm.BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, actualRm.PriceUom);
            Assert.AreEqual(expectedRmStartDate, actualRm.NewStartDate);
            Assert.AreEqual(expectedPriceType, actualRm.NewPriceType);
            Assert.AreEqual(expectedPriceType, actualRm.CurrentPriceType);
            Assert.AreEqual(existingRmPrice.Sale_Multiple, actualRm.CurrentSaleMultiple);
            Assert.AreEqual(existingRmPrice.Sale_Price, actualRm.CurrentSalePrice);
            Assert.AreEqual(existingRmPrice.Sale_Start_Date, actualRm.CurrentSaleStartDate);
            Assert.AreEqual(existingRmPrice.Sale_End_Date, actualRm.CurrentSaleEndDate);
            Assert.IsNull(actualRm.NewSaleEndDate);
            Assert.IsNull(actualRm.NewSaleMultiple);
            Assert.IsNull(actualRm.NewSalePrice);
            Assert.IsNull(actualRm.CancelAllSales);
            Assert.IsNull(actualRm.ErrorMessage);
            Assert.AreEqual(this.region, actualRm.Region);

            Assert.AreEqual(expectedTsCurrentRegularPrice, actualTs.CurrentRegularPrice);
            Assert.AreEqual(expectedTsCurrentRegularMultiple, actualTs.CurrentRegularMultiple);
            Assert.AreEqual(expectedTsPrice, actualTs.NewRegularPrice);
            Assert.AreEqual(expectedMultiple, actualTs.NewRegularMultiple);
            Assert.AreEqual(expectedIdentifier, actualTs.ScanCode);
            Assert.AreEqual(expectedTSBusinessUnitId, actualTs.BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, actualTs.PriceUom);
            Assert.AreEqual(expectedTsStartDate, actualTs.NewStartDate);
            Assert.AreEqual(expectedPriceType, actualTs.NewPriceType);
            Assert.AreEqual(expectedPriceType, actualTs.CurrentPriceType);
            Assert.AreEqual(existingTsPrice.Sale_Multiple, actualTs.CurrentSaleMultiple);
            Assert.AreEqual(existingTsPrice.Sale_Price, actualTs.CurrentSalePrice);
            Assert.AreEqual(existingTsPrice.Sale_Start_Date, actualTs.CurrentSaleStartDate);
            Assert.AreEqual(existingTsPrice.Sale_End_Date, actualTs.CurrentSaleEndDate);
            Assert.IsNull(actualTs.NewSaleEndDate);
            Assert.IsNull(actualTs.NewSaleMultiple);
            Assert.IsNull(actualTs.NewSalePrice);
            Assert.IsNull(actualTs.CancelAllSales);
            Assert.IsNull(actualTs.ErrorMessage);
            Assert.AreEqual("TS", actualTs.Region);
        }

        [TestMethod]
        public void GetPriceEventsQuery_OneItemNotOnPromotionFor365Region_ShouldReturnDataWithTSRegionAbbreviation()
        {
            // Given
            var expectedPrice = 11.99M;
            byte expectedNewMultiple = 1;
            byte expectedCurrentMultiple = 2;
            var expectedIdentifier = "7777777777777";
            var expectedBusinessUnitId = 723;
            DateTime expectedStartDate = DateTime.Today.AddDays(2);
            var expectedPriceType = "REG";
            byte expectedPriceTypeId = dbProvider.Connection.Query<byte>(
                "SELECT PriceChgTypeID FROM PriceChgType WHERE PriceChgTypeDesc = @Desc",
                new { Desc = expectedPriceType },
                dbProvider.Transaction).First();
            var expectedResultUnitAbbreviation = "EA";
            var expectedRetailUnitId = dbProvider.Connection.Query<int>(
                "SELECT Unit_ID FROM ItemUnit WHERE Unit_Abbreviation = @Unit",
                new { Unit = expectedResultUnitAbbreviation },
                dbProvider.Transaction).First();

            var subTeamNo = dbProvider.Connection.Query<int>(
                "SELECT Top 1 SubTeam_No FROM SubTeam",
                null,
                dbProvider.Transaction).First();

            Item newItem = new TestIrmaDbItemBuilder()
                .WithRetail_Unit_ID(expectedRetailUnitId)
                .WithSubTeam_No(subTeamNo);
            var itemKey = this.AddItemToDatabase(newItem);

            ItemIdentifier newItemIdentifier = new TestItemIdentifierBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifier);

            this.AddItemIdentifierToDatabase(newItemIdentifier);

            Store newStore = new TestStoreBuilder()
               .WithBusinessUnitId(expectedBusinessUnitId)
               .WithStoreJurisdictionId(1);

            this.AddNewStore(newStore);

            StoreRegionMapping newStoreRegionMapping = new TestStoreRegionMappingBuilder()
                .WithStore_No(newStore.Store_No)
                .WithRegion_Code("TS");

            this.AddNewStoreRegionMapping(newStoreRegionMapping);

            Price existingPrice = new TestPriceBuilder()
                .WithPrice1(9.99M)
                .WithMultiple(expectedCurrentMultiple)
                .WithPriceChgTypeId(expectedPriceTypeId)
                .WithStore_No(newStore.Store_No)
                .WithItem_Key(itemKey);

            this.AddPriceToDatabase(existingPrice);

            ValidatedScanCode newValidScanCode = new TestValidatedScanCodeBuilder()
                .WithScanCode(expectedIdentifier);

            this.AddValidatedScanCode(newValidScanCode);

            PriceBatchDetail newPriceBatchDetail = new TestPriceBatchDetailBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifier)
                .WithPrice(expectedPrice)
                .WithMultiple(expectedNewMultiple)
                .WithPriceChgTypeID(expectedPriceTypeId)
                .WithStore_No(newStore.Store_No)
                .WithStartDate(expectedStartDate);

            int priceBatchDetailId = this.AddPriceBatchDetail(newPriceBatchDetail);

            var itemPriceEvents = new List<TestItemsForPriceEvents>();
            itemPriceEvents.Add(new TestItemsForPriceEvents
            {
                Item_Key = itemKey,
                Identifier = expectedIdentifier,
                Store_No = newStore.Store_No,
                PriceBatchDetailID = priceBatchDetailId
            });

            this.InsertIntoEventQueue(itemPriceEvents);

            // When
            var results = this.query.Search(this.parameters);
            var actual = results.First();

            // Then
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(existingPrice.Price1, actual.CurrentRegularPrice);
            Assert.AreEqual(existingPrice.Multiple, actual.CurrentRegularMultiple);
            Assert.AreEqual(expectedPrice, actual.NewRegularPrice);
            Assert.AreEqual(expectedNewMultiple, actual.NewRegularMultiple);
            Assert.AreEqual(expectedIdentifier, actual.ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, actual.BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, actual.PriceUom);
            Assert.AreEqual(expectedStartDate, actual.NewStartDate);
            Assert.AreEqual(expectedPriceType, actual.NewPriceType);
            Assert.AreEqual(expectedPriceType, actual.CurrentPriceType);
            Assert.AreEqual(existingPrice.Sale_Multiple, actual.CurrentSaleMultiple);
            Assert.AreEqual(existingPrice.Sale_Price, actual.CurrentSalePrice);
            Assert.AreEqual(existingPrice.Sale_Start_Date, actual.CurrentSaleStartDate);
            Assert.AreEqual(existingPrice.Sale_End_Date, actual.CurrentSaleEndDate);
            Assert.IsNull(actual.NewSaleEndDate);
            Assert.IsNull(actual.NewSaleMultiple);
            Assert.IsNull(actual.NewSalePrice);
            Assert.IsNull(actual.CancelAllSales);
            Assert.IsNull(actual.ErrorMessage);
            Assert.AreEqual("TS", actual.Region);
        }

        [TestMethod]
        public void GetPriceEventsQuery_OneItemHasPromotion_ShouldReturnOneRowWithPromotionData()
        {
            // Given
            var expectedPrice = 11.99M;
            byte expectedNewMultiple = 1;
            byte expectedCurrentMultiple = 1;
            var expectedIdentifier = "7777777777777";
            var expectedBusinessUnitId = 723;
            DateTime expectedStartDate = DateTime.Today.AddDays(2);
            DateTime expectedSaleEndDate = DateTime.Today.AddDays(16);
            decimal expectedSalePrice = 8.99M;
            byte expectedSaleMultiple = 2;

            var expectedPriceTypeSale = "SAL";
            byte expectedPriceTypeIdSale = dbProvider.Connection.Query<byte>(
                "SELECT PriceChgTypeID FROM PriceChgType WHERE PriceChgTypeDesc = @Desc",
                new { Desc = expectedPriceTypeSale },
                dbProvider.Transaction).First();

            string expectedPriceTypeReg = "REG";
            byte expectedPriceTypeIdReg = dbProvider.Connection.Query<byte>(
                "SELECT PriceChgTypeID FROM PriceChgType WHERE PriceChgTypeDesc = @Desc",
                new { Desc = expectedPriceTypeReg },
                dbProvider.Transaction).First();
            var expectedResultUnitAbbreviation = "EA";
            var expectedRetailUnitId = dbProvider.Connection.Query<int>(
                "SELECT Unit_ID FROM ItemUnit WHERE Unit_Abbreviation = @Unit",
                new { Unit = expectedResultUnitAbbreviation },
                dbProvider.Transaction).First();

            var subTeamNo = dbProvider.Connection.Query<int>(
                "SELECT Top 1 SubTeam_No FROM SubTeam",
                null,
                dbProvider.Transaction).First();

            Item newItem = new TestIrmaDbItemBuilder()
                .WithRetail_Unit_ID(expectedRetailUnitId)
                .WithSubTeam_No(subTeamNo);
            var itemKey = this.AddItemToDatabase(newItem);

            ItemIdentifier newItemIdentifier = new TestItemIdentifierBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifier);

            this.AddItemIdentifierToDatabase(newItemIdentifier);

            Store newStore = new TestStoreBuilder()
               .WithBusinessUnitId(expectedBusinessUnitId)
               .WithStoreJurisdictionId(1);

            this.AddNewStore(newStore);

            StoreRegionMapping newStoreRegionMapping = new TestStoreRegionMappingBuilder()
                .WithStore_No(newStore.Store_No)
                .WithRegion_Code(this.region);

            this.AddNewStoreRegionMapping(newStoreRegionMapping);

            Price existingPrice = new TestPriceBuilder()
                .WithPrice1(9.99M)
                .WithMultiple(expectedCurrentMultiple)
                .WithPriceChgTypeId(expectedPriceTypeIdReg)
                .WithStore_No(newStore.Store_No)
                .WithItem_Key(itemKey);

            this.AddPriceToDatabase(existingPrice);

            ValidatedScanCode newValidScanCode = new TestValidatedScanCodeBuilder()
                .WithScanCode(expectedIdentifier);

            this.AddValidatedScanCode(newValidScanCode);

            PriceBatchDetail newPriceBatchDetail = new TestPriceBatchDetailBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifier)
                .WithPrice(expectedPrice)
                .WithMultiple(expectedNewMultiple)
                .WithPriceChgTypeID(expectedPriceTypeIdSale)
                .WithStore_No(newStore.Store_No)
                .WithSale_Price(expectedSalePrice)
                .WithStartDate(expectedStartDate)
                .WithSale_End_Date(expectedSaleEndDate)
                .WithSale_Multiple(expectedSaleMultiple);

            int priceBatchDetailId = this.AddPriceBatchDetail(newPriceBatchDetail);

            var itemPriceEvents = new List<TestItemsForPriceEvents>();
            itemPriceEvents.Add(new TestItemsForPriceEvents
            {
                Item_Key = itemKey,
                Identifier = expectedIdentifier,
                Store_No = newStore.Store_No,
                PriceBatchDetailID = priceBatchDetailId
            });

            this.InsertIntoEventQueue(itemPriceEvents);

            // When
            var results = this.query.Search(this.parameters);
            var actual = results.First();

            // Then
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(existingPrice.Price1, actual.CurrentRegularPrice);
            Assert.AreEqual(existingPrice.Multiple, actual.CurrentRegularMultiple);
            Assert.AreEqual(expectedPrice, actual.NewRegularPrice);
            Assert.AreEqual(expectedNewMultiple, actual.NewRegularMultiple);
            Assert.AreEqual(expectedIdentifier, actual.ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, actual.BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, actual.PriceUom);
            Assert.AreEqual(expectedStartDate, actual.NewStartDate);
            Assert.AreEqual(expectedPriceTypeSale, actual.NewPriceType);
            Assert.AreEqual(expectedPriceTypeReg, actual.CurrentPriceType);
            Assert.AreEqual(existingPrice.Sale_Multiple, actual.CurrentSaleMultiple);
            Assert.AreEqual(existingPrice.Sale_Price, actual.CurrentSalePrice);
            Assert.AreEqual(existingPrice.Sale_Start_Date, actual.CurrentSaleStartDate);
            Assert.AreEqual(existingPrice.Sale_End_Date, actual.CurrentSaleEndDate);
            Assert.AreEqual(expectedSaleEndDate, actual.NewSaleEndDate);
            Assert.AreEqual(expectedSaleMultiple, actual.NewSaleMultiple);
            Assert.AreEqual(expectedSalePrice, actual.NewSalePrice);
            Assert.IsNull(actual.CancelAllSales);
            Assert.IsNull(actual.ErrorMessage);
            Assert.AreEqual(this.region, actual.Region);
        }

        [TestMethod]
        public void GetPriceEventsQuery_OneItemDoesNotHavePromotion_ShouldReturnOneRowWithoutPromotionData()
        {
            // Given
            var expectedPrice = 11.99M;
            byte expectedNewMultiple = 1;
            byte expectedCurrentMultiple = 1;
            var expectedIdentifier = "7777777777777";
            var expectedBusinessUnitId = 723;
            DateTime expectedStartDate = DateTime.Today.AddDays(2);

            
            var expectedPriceTypeSale = "SAL";
            byte expectedPriceTypeIdSale = dbProvider.Connection.Query<byte>(
                "SELECT PriceChgTypeID FROM PriceChgType WHERE PriceChgTypeDesc = @Desc",
                new { Desc = expectedPriceTypeSale },
                dbProvider.Transaction).First();

            var expectedPriceTypeReg = "REG";
            byte expectedPriceTypeIdReg = dbProvider.Connection.Query<byte>(
                "SELECT PriceChgTypeID FROM PriceChgType WHERE PriceChgTypeDesc = @Desc",
                new { Desc = expectedPriceTypeReg },
                dbProvider.Transaction).First();
            var expectedResultUnitAbbreviation = "EA";
            var expectedRetailUnitId = dbProvider.Connection.Query<int>(
                "SELECT Unit_ID FROM ItemUnit WHERE Unit_Abbreviation = @Unit",
                new { Unit = expectedResultUnitAbbreviation },
                dbProvider.Transaction).First();

            var subTeamNo = dbProvider.Connection.Query<int>(
                "SELECT Top 1 SubTeam_No FROM SubTeam",
                null,
                dbProvider.Transaction).First();

            Item newItem = new TestIrmaDbItemBuilder()
                .WithRetail_Unit_ID(expectedRetailUnitId)
                .WithSubTeam_No(subTeamNo);
            var itemKey = this.AddItemToDatabase(newItem);

            ItemIdentifier newItemIdentifier = new TestItemIdentifierBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifier);

            this.AddItemIdentifierToDatabase(newItemIdentifier);

            Store newStore = new TestStoreBuilder()
               .WithBusinessUnitId(expectedBusinessUnitId)
               .WithStoreJurisdictionId(1);

            this.AddNewStore(newStore);

            StoreRegionMapping newStoreRegionMapping = new TestStoreRegionMappingBuilder()
                .WithStore_No(newStore.Store_No)
                .WithRegion_Code(this.region);

            this.AddNewStoreRegionMapping(newStoreRegionMapping);

            Price existingPrice = new TestPriceBuilder()
                .WithPrice1(9.99M)
                .WithMultiple(expectedCurrentMultiple)
                .WithPriceChgTypeId(expectedPriceTypeIdReg)
                .WithStore_No(newStore.Store_No)
                .WithItem_Key(itemKey);

            this.AddPriceToDatabase(existingPrice);

            ValidatedScanCode newValidScanCode = new TestValidatedScanCodeBuilder()
                .WithScanCode(expectedIdentifier);

            this.AddValidatedScanCode(newValidScanCode);

            PriceBatchDetail newPriceBatchDetail = new TestPriceBatchDetailBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifier)
                .WithPrice(expectedPrice)
                .WithMultiple(expectedNewMultiple)
                .WithPriceChgTypeID(expectedPriceTypeIdReg)
                .WithStore_No(newStore.Store_No)
                .WithStartDate(expectedStartDate);

            int priceBatchDetailId = this.AddPriceBatchDetail(newPriceBatchDetail);

            var itemPriceEvents = new List<TestItemsForPriceEvents>();
            itemPriceEvents.Add(new TestItemsForPriceEvents
            {
                Item_Key = itemKey,
                Identifier = expectedIdentifier,
                Store_No = newStore.Store_No,
                PriceBatchDetailID = priceBatchDetailId
            });

            this.InsertIntoEventQueue(itemPriceEvents);

            // When
            var results = this.query.Search(this.parameters);
            var actual = results.First();

            // Then
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(existingPrice.Price1, actual.CurrentRegularPrice);
            Assert.AreEqual(existingPrice.Multiple, actual.CurrentRegularMultiple);
            Assert.AreEqual(expectedPrice, actual.NewRegularPrice);
            Assert.AreEqual(expectedNewMultiple, actual.NewRegularMultiple);
            Assert.AreEqual(expectedIdentifier, actual.ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, actual.BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, actual.PriceUom);
            Assert.AreEqual(expectedStartDate, actual.NewStartDate);
            Assert.AreEqual(expectedPriceTypeReg, actual.NewPriceType);
            Assert.AreEqual(expectedPriceTypeReg, actual.CurrentPriceType);
            Assert.AreEqual(existingPrice.Sale_Multiple, actual.CurrentSaleMultiple);
            Assert.AreEqual(existingPrice.Sale_Price, actual.CurrentSalePrice);
            Assert.AreEqual(existingPrice.Sale_Start_Date, actual.CurrentSaleStartDate);
            Assert.AreEqual(existingPrice.Sale_End_Date, actual.CurrentSaleEndDate);
            Assert.IsNull(actual.NewSaleEndDate);
            Assert.IsNull(actual.NewSaleMultiple);
            Assert.IsNull(actual.NewSalePrice);
            Assert.IsNull(actual.CancelAllSales);
            Assert.IsNull(actual.ErrorMessage);
            Assert.AreEqual(this.region, actual.Region);
        }

        [TestMethod]
        public void GetPriceEventsQuery_ItemDoesNotHaveEventReferenceId_ShouldReturnZeroRows()
        {
            // Given
            var expectedPrice = 11.99M;
            byte expectedNewMultiple = 1;
            byte expectedCurrentMultiple = 1;
            var expectedIdentifier = "7777777777777";
            var expectedBusinessUnitId = 723;
            DateTime expectedStartDate = DateTime.Today.AddDays(2);
            DateTime expectedSaleEndDate = DateTime.Today.AddDays(16);
            decimal expectedSalePrice = 8.99M;
            byte expectedSaleMultiple = 2;
            string expectedPriceType = "REG";
            byte expectedPriceTypeId = dbProvider.Connection.Query<byte>(
                "SELECT PriceChgTypeID FROM PriceChgType WHERE PriceChgTypeDesc = @Desc",
                new { Desc = expectedPriceType },
                dbProvider.Transaction).First();
            var expectedResultUnitAbbreviation = "EA";
            var expectedRetailUnitId = dbProvider.Connection.Query<int>(
                "SELECT Unit_ID FROM ItemUnit WHERE Unit_Abbreviation = @Unit",
                new { Unit = expectedResultUnitAbbreviation },
                dbProvider.Transaction).First();

            var subTeamNo = dbProvider.Connection.Query<int>(
                "SELECT Top 1 SubTeam_No FROM SubTeam",
                null,
                dbProvider.Transaction).First();

            Item newItem = new TestIrmaDbItemBuilder()
                .WithRetail_Unit_ID(expectedRetailUnitId)
                .WithSubTeam_No(subTeamNo);
            var itemKey = this.AddItemToDatabase(newItem);

            ItemIdentifier newItemIdentifier = new TestItemIdentifierBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifier);

            this.AddItemIdentifierToDatabase(newItemIdentifier);

            Store newStore = new TestStoreBuilder()
               .WithBusinessUnitId(expectedBusinessUnitId)
               .WithStoreJurisdictionId(1);

            this.AddNewStore(newStore);

            StoreRegionMapping newStoreRegionMapping = new TestStoreRegionMappingBuilder()
                .WithStore_No(newStore.Store_No)
                .WithRegion_Code(this.region);

            this.AddNewStoreRegionMapping(newStoreRegionMapping);

            Price existingPrice = new TestPriceBuilder()
                .WithPrice1(9.99M)
                .WithMultiple(expectedCurrentMultiple)
                .WithPriceChgTypeId(expectedPriceTypeId)
                .WithStore_No(newStore.Store_No)
                .WithItem_Key(itemKey);

            this.AddPriceToDatabase(existingPrice);

            ValidatedScanCode newValidScanCode = new TestValidatedScanCodeBuilder()
                .WithScanCode(expectedIdentifier);

            this.AddValidatedScanCode(newValidScanCode);

            PriceBatchDetail newPriceBatchDetail = new TestPriceBatchDetailBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifier)
                .WithPrice(expectedPrice)
                .WithMultiple(expectedNewMultiple)
                .WithPriceChgTypeID(expectedPriceTypeId)
                .WithStore_No(newStore.Store_No)
                .WithSale_Price(expectedSalePrice)
                .WithStartDate(expectedStartDate)
                .WithSale_End_Date(expectedSaleEndDate)
                .WithSale_Multiple(expectedSaleMultiple);

            int priceBatchDetailId = this.AddPriceBatchDetail(newPriceBatchDetail);

            var itemPriceEvents = new List<TestItemsForPriceEvents>();
            itemPriceEvents.Add(new TestItemsForPriceEvents
            {
                Item_Key = itemKey,
                Identifier = expectedIdentifier,
                Store_No = newStore.Store_No
            });

            this.InsertIntoEventQueue(itemPriceEvents);

            // When
            var results = this.query.Search(this.parameters);

            // Then
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetPriceEventsQuery_ItemHasAlternateIdentifiers_ShouldReturnOneRowForEachIdentifier()
        {
            // Given
            var expectedPrice = 11.99M;
            byte expectedNewMultiple = 1;
            byte expectedCurrentMultiple = 1;
            string expectedIdentifier = "7777777777771";
            string expectedAlternateIdentifier = "7777777777774";
            var expectedBusinessUnitId = 723;
            DateTime expectedStartDate = DateTime.Today.AddDays(2);
            DateTime expectedSaleEndDate = DateTime.Today.AddDays(16);
            decimal expectedSalePrice = 8.99M;
            byte expectedSaleMultiple = 2;
            string expectedPriceType = "REG";
            byte expectedPriceTypeId = dbProvider.Connection.Query<byte>(
                "SELECT PriceChgTypeID FROM PriceChgType WHERE PriceChgTypeDesc = @Desc",
                new { Desc = expectedPriceType },
                dbProvider.Transaction).First();
            var expectedResultUnitAbbreviation = "EA";
            var expectedRetailUnitId = dbProvider.Connection.Query<int>(
                "SELECT Unit_ID FROM ItemUnit WHERE Unit_Abbreviation = @Unit",
                new { Unit = expectedResultUnitAbbreviation },
                dbProvider.Transaction).First();

            var subTeamNo = dbProvider.Connection.Query<int>(
                "SELECT Top 1 SubTeam_No FROM SubTeam",
                null,
                dbProvider.Transaction).First();

            Item newItem = new TestIrmaDbItemBuilder()
                .WithRetail_Unit_ID(expectedRetailUnitId)
                .WithSubTeam_No(subTeamNo);
            var itemKey = this.AddItemToDatabase(newItem);

            ItemIdentifier newItemIdentifier = new TestItemIdentifierBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifier)
                .WithDefault_Identifier(1);

            this.AddItemIdentifierToDatabase(newItemIdentifier);

            ItemIdentifier newAlternateItemIdentifier = new TestItemIdentifierBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedAlternateIdentifier)
                .WithDefault_Identifier(0);

            this.AddItemIdentifierToDatabase(newAlternateItemIdentifier);

            Store newStore = new TestStoreBuilder()
               .WithBusinessUnitId(expectedBusinessUnitId)
               .WithStoreJurisdictionId(1);

            this.AddNewStore(newStore);

            StoreRegionMapping newStoreRegionMapping = new TestStoreRegionMappingBuilder()
                .WithStore_No(newStore.Store_No)
                .WithRegion_Code(this.region);

            this.AddNewStoreRegionMapping(newStoreRegionMapping);

            Price existingPrice = new TestPriceBuilder()
                .WithPrice1(9.99M)
                .WithMultiple(expectedCurrentMultiple)
                .WithPriceChgTypeId(expectedPriceTypeId)
                .WithStore_No(newStore.Store_No)
                .WithItem_Key(itemKey);

            this.AddPriceToDatabase(existingPrice);

            ValidatedScanCode newValidScanCode = new TestValidatedScanCodeBuilder()
                .WithScanCode(expectedIdentifier);

            this.AddValidatedScanCode(newValidScanCode);

            PriceBatchDetail newPriceBatchDetail = new TestPriceBatchDetailBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifier)
                .WithPrice(expectedPrice)
                .WithMultiple(expectedNewMultiple)
                .WithPriceChgTypeID(expectedPriceTypeId)
                .WithStore_No(newStore.Store_No)
                .WithSale_Price(expectedSalePrice)
                .WithStartDate(expectedStartDate)
                .WithSale_End_Date(expectedSaleEndDate)
                .WithSale_Multiple(expectedSaleMultiple);

            int priceBatchDetailId = this.AddPriceBatchDetail(newPriceBatchDetail);

            var itemPriceEvents = new List<TestItemsForPriceEvents>();
            itemPriceEvents.Add(new TestItemsForPriceEvents
            {
                Item_Key = itemKey,
                Identifier = expectedIdentifier,
                Store_No = newStore.Store_No,
                PriceBatchDetailID = priceBatchDetailId
            });

            this.InsertIntoEventQueue(itemPriceEvents);

            // When
            var results = this.query.Search(this.parameters).OrderBy(q => q.ScanCode).ToList();

            // Then
            Assert.AreEqual(2, results.Count);

            Assert.AreEqual(existingPrice.Price1, results[0].CurrentRegularPrice);
            Assert.AreEqual(existingPrice.Multiple, results[0].CurrentRegularMultiple);
            Assert.AreEqual(expectedPrice, results[0].NewRegularPrice);
            Assert.AreEqual(expectedNewMultiple, results[0].NewRegularMultiple);
            Assert.AreEqual(expectedIdentifier, results[0].ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, results[0].BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, results[0].PriceUom);
            Assert.AreEqual(expectedStartDate, results[0].NewStartDate);
            Assert.AreEqual(expectedPriceType, results[0].NewPriceType);
            Assert.AreEqual(expectedPriceType, results[0].CurrentPriceType);
            Assert.AreEqual(existingPrice.Sale_Multiple, results[0].CurrentSaleMultiple);
            Assert.AreEqual(existingPrice.Sale_Price, results[0].CurrentSalePrice);
            Assert.AreEqual(existingPrice.Sale_Start_Date, results[0].CurrentSaleStartDate);
            Assert.AreEqual(existingPrice.Sale_End_Date, results[0].CurrentSaleEndDate);
            Assert.AreEqual(expectedSaleEndDate, results[0].NewSaleEndDate);
            Assert.AreEqual(expectedSaleMultiple, results[0].NewSaleMultiple);
            Assert.AreEqual(expectedSalePrice, results[0].NewSalePrice);
            Assert.IsNull(results[0].CancelAllSales);
            Assert.IsNull(results[0].ErrorMessage);
            Assert.AreEqual(this.region, results[0].Region);

            Assert.AreEqual(existingPrice.Price1, results[1].CurrentRegularPrice);
            Assert.AreEqual(existingPrice.Multiple, results[1].CurrentRegularMultiple);
            Assert.AreEqual(expectedPrice, results[1].NewRegularPrice);
            Assert.AreEqual(expectedNewMultiple, results[1].NewRegularMultiple);
            Assert.AreEqual(expectedAlternateIdentifier, results[1].ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, results[1].BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, results[1].PriceUom);
            Assert.AreEqual(expectedStartDate, results[1].NewStartDate);
            Assert.AreEqual(expectedPriceType, results[1].NewPriceType);
            Assert.AreEqual(expectedPriceType, results[1].CurrentPriceType);
            Assert.AreEqual(existingPrice.Sale_Multiple, results[1].CurrentSaleMultiple);
            Assert.AreEqual(existingPrice.Sale_Price, results[1].CurrentSalePrice);
            Assert.AreEqual(existingPrice.Sale_Start_Date, results[1].CurrentSaleStartDate);
            Assert.AreEqual(existingPrice.Sale_End_Date, results[1].CurrentSaleEndDate);
            Assert.AreEqual(expectedSaleEndDate, results[1].NewSaleEndDate);
            Assert.AreEqual(expectedSaleMultiple, results[1].NewSaleMultiple);
            Assert.AreEqual(expectedSalePrice, results[1].NewSalePrice);
            Assert.IsNull(results[1].CancelAllSales);
            Assert.IsNull(results[1].ErrorMessage);
            Assert.AreEqual(this.region, results[1].Region);

        }

        [TestMethod]
        public void GetPriceEventsQuery_QueueHasItemsWithPromotionAndWithoutPromotion_ShouldReturnOneRowForEachItem()
        {
            // Given
            decimal expectedNewSalePrice = 8.99M;
            decimal expectedNewRegularPrice = 14.99M;

            decimal expectedCurrentRegularPrice = 12.99M;

            byte expectedNewMultiple = 1;
            byte expectedCurrentMultiple = 1;
            var expectedIdentifierReg = "7777777777775";
            var expectedIdentifierPromo = "7777777777777";
            var expectedBusinessUnitId = 723;
            DateTime expectedStartDate = DateTime.Today.AddDays(2);
            DateTime expectedSaleEndDate = DateTime.Today.AddDays(16);
            byte expectedSaleMultiple = 2;

            string expectedPriceTypeReg = "REG";
            byte expectedPriceTypeIdReg = dbProvider.Connection.Query<byte>(
                "SELECT PriceChgTypeID FROM PriceChgType WHERE PriceChgTypeDesc = @Desc",
                new { Desc = expectedPriceTypeReg },
                dbProvider.Transaction).First();

            string expectedPriceTypePromo = "SAL";
            byte expectedPriceTypeIdPromo = dbProvider.Connection.Query<byte>(
                "SELECT PriceChgTypeID FROM PriceChgType WHERE PriceChgTypeDesc = @Desc",
                new { Desc = expectedPriceTypePromo },
                dbProvider.Transaction).First();

            var expectedResultUnitAbbreviation = "EA";
            var expectedRetailUnitId = dbProvider.Connection.Query<int>(
                "SELECT Unit_ID FROM ItemUnit WHERE Unit_Abbreviation = @Unit",
                new { Unit = expectedResultUnitAbbreviation },
                dbProvider.Transaction).First();

            var subTeamNo = dbProvider.Connection.Query<int>(
                "SELECT Top 1 SubTeam_No FROM SubTeam",
                null,
                dbProvider.Transaction).First();

            Item newItem = new TestIrmaDbItemBuilder()
                .WithRetail_Unit_ID(expectedRetailUnitId)
                .WithSubTeam_No(subTeamNo);
            var itemKey = this.AddItemToDatabase(newItem);

            Item newItemReg = new TestIrmaDbItemBuilder()
                .WithRetail_Unit_ID(expectedRetailUnitId)
                .WithSubTeam_No(subTeamNo);
            var itemKeyReg = this.AddItemToDatabase(newItemReg);

            ItemIdentifier newItemIdentifier = new TestItemIdentifierBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifierPromo);

            this.AddItemIdentifierToDatabase(newItemIdentifier);

            ItemIdentifier newItemIdentifierReg = new TestItemIdentifierBuilder()
                .WithItem_Key(itemKeyReg)
                .WithIdentifier(expectedIdentifierReg);

            this.AddItemIdentifierToDatabase(newItemIdentifierReg);

            Store newStore = new TestStoreBuilder()
               .WithBusinessUnitId(expectedBusinessUnitId)
               .WithStoreJurisdictionId(1);

            this.AddNewStore(newStore);

            StoreRegionMapping newStoreRegionMapping = new TestStoreRegionMappingBuilder()
                .WithStore_No(newStore.Store_No)
                .WithRegion_Code(this.region);

            this.AddNewStoreRegionMapping(newStoreRegionMapping);

            Price existingPricePromo = new TestPriceBuilder()
                .WithPrice1(expectedCurrentRegularPrice)
                .WithMultiple(expectedCurrentMultiple)
                .WithPriceChgTypeId(expectedPriceTypeIdReg)
                .WithStore_No(newStore.Store_No)
                .WithItem_Key(itemKey);

            this.AddPriceToDatabase(existingPricePromo);

            Price existingPriceReg = new TestPriceBuilder()
                .WithPrice1(expectedCurrentRegularPrice)
                .WithMultiple(1)
                .WithPriceChgTypeId(expectedPriceTypeIdReg)
                .WithStore_No(newStore.Store_No)
                .WithItem_Key(itemKeyReg);

            this.AddPriceToDatabase(existingPriceReg);

            ValidatedScanCode newValidScanCodePromo = new TestValidatedScanCodeBuilder()
                .WithScanCode(expectedIdentifierPromo);

            this.AddValidatedScanCode(newValidScanCodePromo);

            ValidatedScanCode newValidScanCodeReg = new TestValidatedScanCodeBuilder()
                .WithScanCode(expectedIdentifierReg);

            this.AddValidatedScanCode(newValidScanCodeReg);

            PriceBatchDetail newPriceBatchDetailPromo = new TestPriceBatchDetailBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifierPromo)
                .WithPrice(expectedCurrentRegularPrice)
                .WithMultiple(expectedNewMultiple)
                .WithPriceChgTypeID(expectedPriceTypeIdPromo)
                .WithStore_No(newStore.Store_No)
                .WithSale_Price(expectedNewSalePrice)
                .WithStartDate(expectedStartDate)
                .WithSale_End_Date(expectedSaleEndDate)
                .WithSale_Multiple(expectedSaleMultiple);

            int priceBatchDetailIdPromo = this.AddPriceBatchDetail(newPriceBatchDetailPromo);

            PriceBatchDetail newPriceBatchDetailReg = new TestPriceBatchDetailBuilder()
                .WithItem_Key(itemKeyReg)
                .WithIdentifier(expectedIdentifierReg)
                .WithPrice(expectedNewRegularPrice)
                .WithMultiple(expectedNewMultiple)
                .WithPriceChgTypeID(expectedPriceTypeIdReg)
                .WithStore_No(newStore.Store_No)
                .WithStartDate(expectedStartDate);

            int priceBatchDetailIdReg = this.AddPriceBatchDetail(newPriceBatchDetailReg);

            var itemPriceEvents = new List<TestItemsForPriceEvents>();
            itemPriceEvents.Add(new TestItemsForPriceEvents
            {
                Item_Key = itemKey,
                Identifier = expectedIdentifierPromo,
                Store_No = newStore.Store_No,
                PriceBatchDetailID = priceBatchDetailIdPromo
            });
            itemPriceEvents.Add(new TestItemsForPriceEvents
            {
                Item_Key = itemKey,
                Identifier = expectedIdentifierReg,
                Store_No = newStore.Store_No,
                PriceBatchDetailID = priceBatchDetailIdReg
            });

            this.InsertIntoEventQueue(itemPriceEvents);

            // When
            var results = this.query.Search(this.parameters);
            var regular = results.First(r => r.NewPriceType == "REG");
            var promo = results.First(r => r.NewPriceType != "REG");

            // Then
            Assert.AreEqual(2, results.Count, "Did not return one row for each item.");

            Assert.AreEqual(expectedCurrentRegularPrice, regular.CurrentRegularPrice);
            Assert.AreEqual(expectedCurrentMultiple, regular.CurrentRegularMultiple);
            Assert.AreEqual(expectedNewRegularPrice, regular.NewRegularPrice);
            Assert.AreEqual(expectedNewMultiple, regular.NewRegularMultiple);
            Assert.AreEqual(expectedIdentifierReg, regular.ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, regular.BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, regular.PriceUom);
            Assert.AreEqual(expectedStartDate, regular.NewStartDate);
            Assert.AreEqual(expectedPriceTypeReg, regular.NewPriceType);
            Assert.AreEqual(expectedPriceTypeReg, regular.CurrentPriceType);
            Assert.AreEqual(existingPriceReg.Sale_Multiple, regular.CurrentSaleMultiple);
            Assert.AreEqual(existingPriceReg.Sale_Price, regular.CurrentSalePrice);
            Assert.AreEqual(existingPriceReg.Sale_Start_Date, regular.CurrentSaleStartDate);
            Assert.AreEqual(existingPriceReg.Sale_End_Date, regular.CurrentSaleEndDate);
            Assert.IsNull(regular.NewSaleEndDate);
            Assert.IsNull(regular.NewSaleMultiple);
            Assert.IsNull(regular.NewSalePrice);
            Assert.IsNull(regular.CancelAllSales);
            Assert.IsNull(regular.ErrorMessage);
            Assert.AreEqual(this.region, regular.Region);

            Assert.AreEqual(expectedCurrentRegularPrice, promo.CurrentRegularPrice);
            Assert.AreEqual(expectedCurrentMultiple, promo.CurrentRegularMultiple);
            Assert.AreEqual(expectedCurrentRegularPrice, promo.NewRegularPrice);
            Assert.AreEqual(expectedNewMultiple, promo.NewRegularMultiple);
            Assert.AreEqual(expectedIdentifierPromo, promo.ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, promo.BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, promo.PriceUom);
            Assert.AreEqual(expectedStartDate, promo.NewStartDate);
            Assert.AreEqual(expectedPriceTypePromo, promo.NewPriceType);
            Assert.AreEqual(expectedPriceTypeReg, promo.CurrentPriceType);
            Assert.AreEqual(existingPricePromo.Sale_Multiple, promo.CurrentSaleMultiple);
            Assert.AreEqual(existingPricePromo.Sale_Price, promo.CurrentSalePrice);
            Assert.AreEqual(existingPricePromo.Sale_Start_Date, promo.CurrentSaleStartDate);
            Assert.AreEqual(existingPricePromo.Sale_End_Date, promo.CurrentSaleEndDate);
            Assert.AreEqual(expectedSaleEndDate, promo.NewSaleEndDate);
            Assert.AreEqual(expectedSaleMultiple, promo.NewSaleMultiple);
            Assert.AreEqual(expectedNewSalePrice, promo.NewSalePrice);
            Assert.IsNull(promo.CancelAllSales);
            Assert.IsNull(promo.ErrorMessage);
            Assert.AreEqual(this.region, promo.Region);
        }

        private void InsertIntoEventQueue(List<TestItemsForPriceEvents> items)
        {
            foreach (var item in items)
            {
                dbProvider.Connection.Execute(
                    @"insert into mammoth.PriceChangeQueue(Item_Key, Store_No, Identifier, EventTypeID, EventReferenceID, InProcessBy)
                         values(@Item_Key, @Store_No, @Identifier, @EventTypeID, @PriceBatchDetailID, @Instance)",
                    new
                    {
                        Item_Key = item.Item_Key,
                        Store_No = item.Store_No,
                        Identifier = item.Identifier,
                        EventTypeID = IrmaEventTypes.Price,
                        PriceBatchDetailID = item.PriceBatchDetailID,
                        Instance = parameters.Instance
                    },
                    transaction: this.dbProvider.Transaction);
            }
        }

        private class TestItemsForPriceEvents
        {
            public int IdentifierCount { get; set; }
            public int? PriceBatchDetailID { get; set; }
            public string Identifier { get; set; }
            public int Item_Key { get; set; }
            public int Store_No { get; set; }
        }

        private int AddItemToDatabase(Item newItem)
        {
            var sql = @"INSERT INTO [dbo].[Item](
                [Item_Description]
               ,[Sign_Description]
               ,[Ingredients]
               ,[SubTeam_No]
               ,[Sales_Account]
               ,[Package_Desc1]
               ,[Package_Desc2]
               ,[Package_Unit_ID]
               ,[Min_Temperature]
               ,[Max_Temperature]
               ,[Units_Per_Pallet]
               ,[Average_Unit_Weight]
               ,[Tie]
               ,[High]
               ,[Yield]
               ,[Brand_ID]
               ,[Category_ID]
               ,[Origin_ID]
               ,[ShelfLife_Length]
               ,[ShelfLife_ID]
               ,[Retail_Unit_ID]
               ,[Vendor_Unit_ID]
               ,[Distribution_Unit_ID]
               ,[Cost_Unit_ID]
               ,[Freight_Unit_ID]
               ,[Deleted_Item]
               ,[WFM_Item]
               ,[Not_Available]
               ,[Pre_Order]
               ,[Remove_Item]
               ,[NoDistMarkup]
               ,[Organic]
               ,[Refrigerated]
               ,[Keep_Frozen]
               ,[Shipper_Item]
               ,[Full_Pallet_Only]
               ,[User_ID]
               ,[POS_Description]
               ,[Retail_Sale]
               ,[Food_Stamps]
               ,[Discountable]
               ,[Price_Required]
               ,[Quantity_Required]
               ,[ItemType_ID]
               ,[HFM_Item]
               ,[ScaleDesc1]
               ,[ScaleDesc2]
               ,[Not_AvailableNote]
               ,[CountryProc_ID]
               ,[Insert_Date]
               ,[Manufacturing_Unit_ID]
               ,[EXEDistributed]
               ,[ClassID]
               ,[User_ID_Date]
               ,[DistSubTeam_No]
               ,[CostedByWeight]
               ,[TaxClassID]
               ,[LabelType_ID]
               ,[ScaleDesc3]
               ,[ScaleDesc4]
               ,[ScaleTare]
               ,[ScaleUseBy]
               ,[ScaleForcedTare]
               ,[QtyProhibit]
               ,[GroupList]
               ,[ProdHierarchyLevel4_ID]
               ,[Case_Discount]
               ,[Coupon_Multiplier]
               ,[Misc_Transaction_Sale]
               ,[Misc_Transaction_Refund]
               ,[Recall_Flag]
               ,[Manager_ID]
               ,[Ice_Tare]
               ,[LockAuth]
               ,[PurchaseThresholdCouponAmount]
               ,[PurchaseThresholdCouponSubTeam]
               ,[Product_Code]
               ,[Unit_Price_Category]
               ,[StoreJurisdictionID]
               ,[CatchweightRequired]
               ,[COOL]
               ,[BIO]
               ,[LastModifiedUser_ID]
               ,[LastModifiedDate]
               ,[CatchWtReq]
               ,[SustainabilityRankingRequired]
               ,[SustainabilityRankingID]
               ,[Ingredient]
               ,[FSA_Eligible]
               ,[TaxClassModifiedDate]
               ,[UseLastReceivedCost]
               ,[GiftCard])
         VALUES(
               @Item_Description,
               @Sign_Description,
               @Ingredients,
               @SubTeam_No,
               @Sales_Account,
               @Package_Desc1,
               @Package_Desc2,
               @Package_Unit_ID,
               @Min_Temperature,
               @Max_Temperature,
               @Units_Per_Pallet,
               @Average_Unit_Weight,
               @Tie,
               @High,
               @Yield,
               @Brand_ID,
               @Category_ID,
               @Origin_ID,
               @ShelfLife_Length,
               @ShelfLife_ID,
               @Retail_Unit_ID,
               @Vendor_Unit_ID,
               @Distribution_Unit_ID,
               @Cost_Unit_ID,
               @Freight_Unit_ID,
               @Deleted_Item,
               @WFM_Item,
               @Not_Available,
               @Pre_Order,
               @Remove_Item,
               @NoDistMarkup,
               @Organic,
               @Refrigerated,
               @Keep_Frozen,
               @Shipper_Item,
               @Full_Pallet_Only,
               @User_ID,
               @POS_Description,
               @Retail_Sale,
               @Food_Stamps,
               @Discountable,
               @Price_Required,
               @Quantity_Required,
               @ItemType_ID,
               @HFM_Item,
               @ScaleDesc1,
               @ScaleDesc2,
               @Not_AvailableNote,
               @CountryProc_ID,
               @Insert_Date,
               @Manufacturing_Unit_ID,
               @EXEDistributed,
               @ClassID,
               @User_ID_Date,
               @DistSubTeam_No,
               @CostedByWeight,
               @TaxClassID,
               @LabelType_ID,
               @ScaleDesc3,
               @ScaleDesc4,
               @ScaleTare,
               @ScaleUseBy,
               @ScaleForcedTare,
               @QtyProhibit,
               @GroupList,
               @ProdHierarchyLevel4_ID,
               @Case_Discount,
               @Coupon_Multiplier,
               @Misc_Transaction_Sale,
               @Misc_Transaction_Refund,
               @Recall_Flag,
               @Manager_ID,
               @Ice_Tare,
               @LockAuth,
               @PurchaseThresholdCouponAmount,
               @PurchaseThresholdCouponSubTeam,
               @Product_Code,
               @Unit_Price_Category,
               @StoreJurisdictionID,
               @CatchweightRequired,
               @COOL,
               @BIO,
               @LastModifiedUser_ID,
               @LastModifiedDate,
               @CatchWtReq,
               @SustainabilityRankingRequired,
               @SustainabilityRankingID,
               @Ingredient,
               @FSA_Eligible,
               @TaxClassModifiedDate,
               @UseLastReceivedCost,
               @GiftCard)

            SELECT SCOPE_IDENTITY()";

            var itemKey = this.dbProvider.Connection.Query<int>(sql, newItem, dbProvider.Transaction).First();
            return itemKey;
        }

        private void AddItemIdentifierToDatabase(ItemIdentifier newItemIdentifier)
        {
            var sql = @"INSERT INTO [dbo].[ItemIdentifier](
                [Item_Key]
               ,[Identifier]
               ,[Default_Identifier]
               ,[Deleted_Identifier]
               ,[Add_Identifier]
               ,[Remove_Identifier]
               ,[National_Identifier]
               ,[CheckDigit]
               ,[IdentifierType]
               ,[NumPluDigitsSentToScale]
               ,[Scale_Identifier])
            VALUES(
               @Item_Key,
               @Identifier,
               @Default_Identifier,
               @Deleted_Identifier,
               @Add_Identifier,
               @Remove_Identifier,
               @National_Identifier,
               @CheckDigit,
               @IdentifierType,
               @NumPluDigitsSentToScale,
               @Scale_Identifier)";

            this.dbProvider.Connection.Execute(sql, newItemIdentifier, dbProvider.Transaction);
        }

        private void AddPriceToDatabase(Price newPrice)
        {
            var sql = @"INSERT INTO [dbo].[Price](
                [Item_Key]
                ,[Store_No]
                ,[Multiple]
                ,[Price]
                ,[MSRPPrice]
                ,[MSRPMultiple]
                ,[PricingMethod_ID]
                ,[Sale_Multiple]
                ,[Sale_Price]
                ,[Sale_Start_Date]
                ,[Sale_End_Date]
                ,[Sale_Max_Quantity]
                ,[Sale_Earned_Disc1]
                ,[Sale_Earned_Disc2]
                ,[Sale_Earned_Disc3]
                ,[Restricted_Hours]
                ,[AvgCostUpdated]
                ,[IBM_Discount]
                ,[POSPrice]
                ,[POSSale_Price]
                ,[NotAuthorizedForSale]
                ,[CompFlag]
                ,[PosTare]
                ,[LinkedItem]
                ,[GrillPrint]
                ,[AgeCode]
                ,[VisualVerify]
                ,[SrCitizenDiscount]
                ,[PriceChgTypeId]
                ,[ExceptionSubteam_No]
                ,[POSLinkCode]
                ,[KitchenRoute_ID]
                ,[Routing_Priority]
                ,[Consolidate_Price_To_Prev_Item]
                ,[Print_Condiment_On_Receipt]
                ,[Age_Restrict]
                ,[CompetitivePriceTypeID]
                ,[BandwidthPercentageHigh]
                ,[BandwidthPercentageLow]
                ,[MixMatch]
                ,[Discountable]
                ,[LastScannedUserId_DTS]
                ,[LastScannedUserId_NonDTS]
                ,[LastScannedDate_DTS]
                ,[LastScannedDate_NonDTS]
                ,[LocalItem]
                ,[ItemSurcharge]
                ,[ElectronicShelfTag])
            VALUES(
                 @Item_Key
                ,@Store_No
                ,@Multiple
                ,@Price1
                ,@MSRPPrice
                ,@MSRPMultiple
                ,@PricingMethod_ID
                ,@Sale_Multiple
                ,@Sale_Price
                ,@Sale_Start_Date
                ,@Sale_End_Date
                ,@Sale_Max_Quantity
                ,@Sale_Earned_Disc1
                ,@Sale_Earned_Disc2
                ,@Sale_Earned_Disc3
                ,@Restricted_Hours
                ,@AvgCostUpdated
                ,@IBM_Discount
                ,@POSPrice
                ,@POSSale_Price
                ,@NotAuthorizedForSale
                ,@CompFlag
                ,@PosTare
                ,@LinkedItem
                ,@GrillPrint
                ,@AgeCode
                ,@VisualVerify
                ,@SrCitizenDiscount
                ,@PriceChgTypeId
                ,@ExceptionSubteam_No
                ,@POSLinkCode
                ,@KitchenRoute_ID
                ,@Routing_Priority
                ,@Consolidate_Price_To_Prev_Item
                ,@Print_Condiment_On_Receipt
                ,@Age_Restrict
                ,@CompetitivePriceTypeID
                ,@BandwidthPercentageHigh
                ,@BandwidthPercentageLow
                ,@MixMatch
                ,@Discountable
                ,@LastScannedUserId_DTS
                ,@LastScannedUserId_NonDTS
                ,@LastScannedDate_DTS
                ,@LastScannedDate_NonDTS
                ,@LocalItem
                ,@ItemSurcharge
                ,@ElectronicShelfTag)";

            this.dbProvider.Connection.Execute(sql, newPrice, dbProvider.Transaction);
        }

        private void AddNewStore(Store newStore)
        {
            var sql = @"INSERT INTO [dbo].[Store]
           ([Store_No]
           ,[Store_Name]
           ,[Phone_Number]
           ,[Mega_Store]
           ,[Distribution_Center]
           ,[Manufacturer]
           ,[WFM_Store]
           ,[Internal]
           ,[TelnetUser]
           ,[TelnetPassword]
           ,[BatchID]
           ,[BatchRecords]
           ,[BusinessUnit_ID]
           ,[Zone_ID]
           ,[UNFI_Store]
           ,[LastRecvLogDate]
           ,[LastRecvLog_No]
           ,[RecvLogUser_ID]
           ,[EXEWarehouse]
           ,[Regional]
           ,[LastSalesUpdateDate]
           ,[StoreAbbr]
           ,[PLUMStoreNo]
           ,[TaxJurisdictionID]
           ,[POSSystemId]
           ,[PSI_Store_No]
           ,[StoreJurisdictionID]
           ,[UseAvgCostHistory]
           ,[GeoCode])
     VALUES
           (@Store_No,
           @Store_Name,
           @Phone_Number,
           @Mega_Store,
           @Distribution_Center,
           @Manufacturer,
           @WFM_Store,
           @Internal,
           @TelnetUser,
           @TelnetPassword,
           @BatchID,
           @BatchRecords,
           @BusinessUnit_ID,
           @Zone_ID,
           @UNFI_Store,
           @LastRecvLogDate,
           @LastRecvLog_No,
           @RecvLogUser_ID,
           @EXEWarehouse,
           @Regional,
           @LastSalesUpdateDate,
           @StoreAbbr,
           @PLUMStoreNo,
           @TaxJurisdictionID,
           @POSSystemId,
           @PSI_Store_No,
           @StoreJurisdictionID,
           @UseAvgCostHistory,
           @GeoCode)";

            this.dbProvider.Connection.Execute(sql, newStore, dbProvider.Transaction);
        }

        private void AddNewStoreRegionMapping(StoreRegionMapping newStoreRegionMapping)
        {
            var sql = @"INSERT INTO StoreRegionMapping (Store_No, Region_Code) VALUES (@Store_No, @Region_Code)";
            this.dbProvider.Connection.Execute(sql, newStoreRegionMapping, dbProvider.Transaction);
        }

        private void AddValidatedScanCode(ValidatedScanCode newValidatedScanCode)
        {
            var sql = @"INSERT INTO ValidatedScanCode (ScanCode, InsertDate) VALUES (@ScanCode, @InsertDate)";
            this.dbProvider.Connection.Execute(sql, newValidatedScanCode, dbProvider.Transaction);
        }

        private int AddPriceBatchDetail(PriceBatchDetail newPriceBatchDetail)
        {
            string sql = @"INSERT INTO[dbo].[PriceBatchDetail]
                        ([Item_Key]
	                    ,[Store_No]
	                    ,[PriceBatchHeaderID]
	                    ,[ItemChgTypeID]
	                    ,[PriceChgTypeID]
	                    ,[StartDate]
	                    ,[Multiple]
	                    ,[Price]
	                    ,[MSRPPrice]
	                    ,[MSRPMultiple]
	                    ,[PricingMethod_ID]
	                    ,[Sale_Multiple]
	                    ,[Sale_Price]
	                    ,[Sale_End_Date]
	                    ,[Sale_Max_Quantity]
	                    ,[Sale_Mix_Match]
	                    ,[Sale_Earned_Disc1]
	                    ,[Sale_Earned_Disc2]
	                    ,[Sale_Earned_Disc3]
	                    ,[Case_Price]
	                    ,[Sign_Description]
	                    ,[Ingredients]
	                    ,[Identifier]
	                    ,[Sold_By_Weight]
	                    ,[SubTeam_No]
	                    ,[Origin_Name]
	                    ,[Brand_Name]
	                    ,[Retail_Unit_Abbr]
	                    ,[Retail_Unit_Full]
	                    ,[Package_Unit]
	                    ,[Package_Desc1]
	                    ,[Package_Desc2]
	                    ,[Organic]
	                    ,[Vendor_Id]
	                    ,[ItemType_ID]
	                    ,[ScaleDesc1]
	                    ,[ScaleDesc2]
	                    ,[POS_Description]
	                    ,[Restricted_Hours]
	                    ,[Quantity_Required]
	                    ,[Price_Required]
	                    ,[Retail_Sale]
	                    ,[Discountable]
	                    ,[Food_Stamps]
	                    ,[IBM_Discount]
	                    ,[Hobart_Item]
	                    ,[PrintSign]
	                    ,[LineDrive]
	                    ,[POSPrice]
	                    ,[POSSale_Price]
	                    ,[Offer_ID]
	                    ,[AvgCostUpdated]
	                    ,[NotAuthorizedForSale]
	                    ,[Deleted_Item]
	                    ,[User_ID]
	                    ,[User_ID_Date]
	                    ,[LabelType_ID]
	                    ,[Insert_Date]
	                    ,[OfferChgTypeID]
	                    ,[QtyProhibit]
	                    ,[GroupList]
	                    ,[PosTare]
	                    ,[LinkedItem]
	                    ,[GrillPrint]
	                    ,[AgeCode]
	                    ,[VisualVerify]
	                    ,[SrCitizenDiscount]
	                    ,[AsOfDate]
	                    ,[AutoGenerated]
	                    ,[Expired]
	                    ,[POSLinkCode]
	                    ,[InsertApplication]
	                    ,[RetailUnit_WeightUnit]
	                    ,[TagTypeID]
	                    ,[TagTypeID2]
	                    ,[EndedEarly]
	                    ,[MixMatch]
	                    ,[PurchaseThresholdCouponAmount]
	                    ,[PurchaseThresholdCouponSubTeam]
	                    ,[ReAuthFlag]
	                    ,[Retail_Unit_ID]
	                    ,[Package_Unit_ID]
	                    ,[Item_Description]
	                    ,[Case_Discount]
	                    ,[Coupon_Multiplier]
	                    ,[Misc_Transaction_Sale]
	                    ,[Misc_Transaction_Refund]
	                    ,[Recall_Flag]
	                    ,[Ice_Tare]
	                    ,[Product_Code]
	                    ,[Unit_Price_Category]
	                    ,[KitchenRoute_ID]
	                    ,[Routing_Priority]
	                    ,[Consolidate_Price_To_Prev_Item]
	                    ,[Print_Condiment_On_Receipt]
	                    ,[Age_Restrict]
	                    ,[SLIMRequestID]
	                    ,[LocalItem]
	                    ,[FSA_Eligible]
	                    ,[ItemSurcharge]
	                    ,[CancelAllSales])
                    VALUES
                        (@Item_Key,
                        @Store_No,
                        @PriceBatchHeaderID,
                        @ItemChgTypeID,
                        @PriceChgTypeID,
                        @StartDate,
                        @Multiple,
                        @Price,
                        @MSRPPrice,
                        @MSRPMultiple,
                        @PricingMethod_ID,
                        @Sale_Multiple,
                        @Sale_Price,
                        @Sale_End_Date,
                        @Sale_Max_Quantity,
                        @Sale_Mix_Match,
                        @Sale_Earned_Disc1,
                        @Sale_Earned_Disc2,
                        @Sale_Earned_Disc3,
                        @Case_Price,
                        @Sign_Description,
                        @Ingredients,
                        @Identifier,
                        @Sold_By_Weight,
                        @SubTeam_No,
                        @Origin_Name,
                        @Brand_Name,
                        @Retail_Unit_Abbr,
                        @Retail_Unit_Full,
                        @Package_Unit,
                        @Package_Desc1,
                        @Package_Desc2,
                        @Organic,
                        @Vendor_Id,
                        @ItemType_ID,
                        @ScaleDesc1,
                        @ScaleDesc2,
                        @POS_Description,
                        @Restricted_Hours,
                        @Quantity_Required,
                        @Price_Required,
                        @Retail_Sale,
                        @Discountable,
                        @Food_Stamps,
                        @IBM_Discount,
                        @Hobart_Item,
                        @PrintSign,
                        @LineDrive,
                        @POSPrice,
                        @POSSale_Price,
                        @Offer_ID,
                        @AvgCostUpdated,
                        @NotAuthorizedForSale,
                        @Deleted_Item,
                        @User_ID,
                        @User_ID_Date,
                        @LabelType_ID,
                        @Insert_Date,
                        @OfferChgTypeID,
                        @QtyProhibit,
                        @GroupList,
                        @PosTare,
                        @LinkedItem,
                        @GrillPrint,
                        @AgeCode,
                        @VisualVerify,
                        @SrCitizenDiscount,
                        @AsOfDate,
                        @AutoGenerated,
                        @Expired,
                        @POSLinkCode,
                        @InsertApplication,
                        @RetailUnit_WeightUnit,
                        @TagTypeID,
                        @TagTypeID2,
                        @EndedEarly,
                        @MixMatch,
                        @PurchaseThresholdCouponAmount,
                        @PurchaseThresholdCouponSubTeam,
                        @ReAuthFlag,
                        @Retail_Unit_ID,
                        @Package_Unit_ID,
                        @Item_Description,
                        @Case_Discount,
                        @Coupon_Multiplier,
                        @Misc_Transaction_Sale,
                        @Misc_Transaction_Refund,
                        @Recall_Flag,
                        @Ice_Tare,
                        @Product_Code,
                        @Unit_Price_Category,
                        @KitchenRoute_ID,
                        @Routing_Priority,
                        @Consolidate_Price_To_Prev_Item,
                        @Print_Condiment_On_Receipt,
                        @Age_Restrict,
                        @SLIMRequestID,
                        @LocalItem,
                        @FSA_Eligible,
                        @ItemSurcharge,
                        @CancelAllSales)

                    SELECT SCOPE_IDENTITY();";

            int priceBatchDetailId = this.dbProvider.Connection.Query<int>(sql, 
                newPriceBatchDetail,
                this.dbProvider.Transaction).First();

            return priceBatchDetailId;
        }
    }
}
