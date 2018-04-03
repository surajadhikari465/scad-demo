namespace Mammoth.Price.Controller.DataAccess.Tests.Queries
{
    using Dapper;
    using Irma.Framework;
    using Irma.Testing;
    using Mammoth.Common.DataAccess;
    using Mammoth.Common.DataAccess.DbProviders;
    using Mammoth.Price.Controller.DataAccess.Queries;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class GetExistingPricesQueryTests
    {
        private const string RegularPriceType = "REG";
        private const string SalePriceType = "SAL";
        private const string ForEachAbbrev = "EA";

        private string region = "FL";
        private GetExistingPriceDataQuery query;
        private GetExistingPriceDataParameters parameters;
        private SqlDbProvider dbProvider;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = IrmaTestDbProviderFactory.CreateAndOpen(this.region);
            query = new GetExistingPriceDataQuery(dbProvider);
            parameters = new GetExistingPriceDataParameters
            {
                Instance = 454,
                Region = region
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void GetExisitingPriceEventsQuery_OneEventExistsNotOnPromotion_ShouldReturnOnePriceEvent()
        {
            // Given
            var expectedBusinessUnitId = 723;
            var expectedIdentifier = "7777777777777";
            var expectedMultiple = (byte)1;
            var expectedPrice = 4.00m;
            var expectedPriceType = RegularPriceType;
            var expectedPriceTypeId = this.GetPriceChgTypeId(RegularPriceType);
            var expectedResultUnitAbbreviation = ForEachAbbrev;
            var expectedRetailUnitId = this.GetUnitId(ForEachAbbrev);
            var storeNo = 73245732;
            var subTeamNo = this.GetFirstSubTeamNo();

            // Insert New Item
            var itemKey = this.dbProvider.Insert(
                new IrmaQueryParams<Item, int>(
                    IrmaTestObjectFactory.BuildItem()
                        .With(x => x.Retail_Unit_ID, expectedRetailUnitId)
                        .With(x => x.SubTeam_No, subTeamNo)
                        .ToObject(),
                x => x.Item_Key));

            // Insert New Item Identifier
            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, expectedIdentifier)
                        .ToObject(),
                    x => x.Identifier_ID));

            // Insert New Store
            this.dbProvider.Insert(IrmaTestObjectFactory.BuildStore()
                .With(x => x.Store_No, storeNo)
                .With(x => x.BusinessUnit_ID, expectedBusinessUnitId)
                .With(x => x.StoreJurisdictionID, 1)
                .ToObject());

            // Insert New Store Region Mapping
            this.dbProvider.Insert(IrmaTestObjectFactory.Build<StoreRegionMapping>()
                .With(x => x.Store_No, storeNo)
                .With(x => x.Region_Code, this.region)
                .ToObject());

            // Insert New Price
            this.dbProvider.Insert(new IrmaQueryParams<Price, int>(
                IrmaTestObjectFactory.Build<Price>()
                    .With(x => x.Price1, expectedPrice)
                    .With(x => x.Multiple, expectedMultiple)
                    .With(x => x.PriceChgTypeId, expectedPriceTypeId)
                    .With(x => x.Store_No, storeNo)
                    .With(x => x.Item_Key, itemKey)
                    .ToObject(),
                null,
                new Dictionary<string, string> { { "Price1", "Price" } }));

            // Insert New Validated Scan Code
            this.dbProvider.Insert(new IrmaQueryParams<ValidatedScanCode, int>(
                IrmaTestObjectFactory.BuildValidatedScanCode()
                    .With(x => x.ScanCode, expectedIdentifier)
                    .ToObject(),
                x => x.Id));

            this.InsertIntoEventQueue(new List<TestItemsForPriceEvents>
            {
                new TestItemsForPriceEvents
                {
                    Item_Key = itemKey,
                    Identifier = expectedIdentifier,
                    Store_No = storeNo,
                }
            });

            // When
            var actual = this.query.Search(this.parameters).Single();

            // Then
            Assert.AreEqual(expectedPrice, actual.CurrentRegularPrice);
            Assert.AreEqual(expectedPrice, actual.NewRegularPrice);
            Assert.AreEqual(expectedMultiple, actual.CurrentRegularMultiple);
            Assert.AreEqual(expectedMultiple, actual.NewRegularMultiple);
            Assert.AreEqual(expectedIdentifier, actual.ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, actual.BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, actual.PriceUom);
            Assert.AreEqual(DateTime.Today, actual.NewStartDate);
            Assert.AreEqual(expectedPriceType, actual.NewPriceType);
            Assert.IsNull(actual.NewSaleEndDate);
            Assert.IsNull(actual.NewSalePrice);
            Assert.IsNull(actual.NewSaleMultiple);
            Assert.IsNull(actual.CurrentSalePrice);
            Assert.IsNull(actual.CurrentSaleMultiple);
            Assert.IsNull(actual.CurrentSaleStartDate);
            Assert.IsNull(actual.CurrentSaleEndDate);
            Assert.IsNull(actual.CurrentPriceType);
            Assert.IsNull(actual.ErrorMessage);
            Assert.AreEqual(this.region, actual.Region);
        }

        [TestMethod]
        public void GetPriceEventsQuery_EventsExistForItemWithAlternateIdentifiers_ReturnsOnlyOneRowForIdentifierInEvent()
        {
            // Given
            var expectedPrice = 4.00m;
            var expectedMultiple = (byte)1;
            var expectedIdentifier = "7777777777777";
            var expectedBusinessUnitId = 723;
            var expectedPriceType = RegularPriceType;
            var expectedPriceTypeId = this.GetPriceChgTypeId(RegularPriceType);
            var expectedResultUnitAbbreviation = ForEachAbbrev;
            var expectedRetailUnitId = this.GetUnitId(ForEachAbbrev);
            var storeNo = 73245732;
            var subTeamNo = this.GetFirstSubTeamNo();

            // Insert New Item
            var itemKey = this.dbProvider.Insert(
                new IrmaQueryParams<Item, int>(
                    IrmaTestObjectFactory.BuildItem()
                        .With(x => x.Retail_Unit_ID, expectedRetailUnitId)
                        .With(x => x.SubTeam_No, subTeamNo)
                        .ToObject(),
                x => x.Item_Key));

            // Insert New Item Identifier
            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, expectedIdentifier)
                        .ToObject(),
                    x => x.Identifier_ID));

            // Insert New Item Identifier
            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, "7777777777778")
                        .With(x => x.Default_Identifier, (byte)0)
                        .ToObject(),
                    x => x.Identifier_ID));

            // Insert New Store
            this.dbProvider.Insert(IrmaTestObjectFactory.BuildStore()
                .With(x => x.Store_No, storeNo)
                .With(x => x.BusinessUnit_ID, expectedBusinessUnitId)
                .With(x => x.StoreJurisdictionID, 1)
                .ToObject());

            // Insert New Store Region Mapping
            this.dbProvider.Insert(IrmaTestObjectFactory.Build<StoreRegionMapping>()
                .With(x => x.Store_No, storeNo)
                .With(x => x.Region_Code, this.region)
                .ToObject());

            // Insert New Price
            this.dbProvider.Insert(new IrmaQueryParams<Price, int>(
                IrmaTestObjectFactory.Build<Price>()
                    .With(x => x.Price1, expectedPrice)
                    .With(x => x.Multiple, expectedMultiple)
                    .With(x => x.PriceChgTypeId, expectedPriceTypeId)
                    .With(x => x.Store_No, storeNo)
                    .With(x => x.Item_Key, itemKey)
                    .ToObject(),
                null,
                new Dictionary<string, string> { { "Price1", "Price" } }));

            // Insert New Validated Scan Code
            this.dbProvider.Insert(new IrmaQueryParams<ValidatedScanCode, int>(
                IrmaTestObjectFactory.BuildValidatedScanCode()
                    .With(x => x.ScanCode, expectedIdentifier)
                    .ToObject(),
                x => x.Id));

            this.InsertIntoEventQueue(new List<TestItemsForPriceEvents>
            {
                new TestItemsForPriceEvents
                {
                    Item_Key = itemKey,
                    Identifier = expectedIdentifier,
                    Store_No = storeNo,
                }
            });

            // When
            var actual = this.query.Search(this.parameters).Single();

            // Then
            Assert.AreEqual(expectedPrice, actual.CurrentRegularPrice);
            Assert.AreEqual(expectedMultiple, actual.CurrentRegularMultiple);
            Assert.AreEqual(expectedPrice, actual.NewRegularPrice);
            Assert.AreEqual(expectedMultiple, actual.NewRegularMultiple);
            Assert.AreEqual(expectedIdentifier, actual.ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, actual.BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, actual.PriceUom);
            Assert.AreEqual(DateTime.Today, actual.NewStartDate);
            Assert.AreEqual(expectedPriceType, actual.NewPriceType);
            Assert.IsNull(actual.NewSaleEndDate);
            Assert.IsNull(actual.NewSalePrice);
            Assert.IsNull(actual.NewSaleMultiple);
            Assert.IsNull(actual.CurrentSalePrice);
            Assert.IsNull(actual.CurrentSaleMultiple);
            Assert.IsNull(actual.CurrentSaleStartDate);
            Assert.IsNull(actual.CurrentSaleEndDate);
            Assert.IsNull(actual.CurrentPriceType);
            Assert.IsNull(actual.ErrorMessage);
            Assert.AreEqual(this.region, actual.Region);
        }

        [TestMethod]
        public void GetPriceEventsQuery_EventIsForAnAlternateIdentifier_ReturnsRowForIdentifierWithExistingPrices()
        {
            // Given
            var expectedBusinessUnitId = 723;
            var expectedIdentifier = "7777777777777";
            var expectedIdentifier2 = "7777777777778";
            var expectedMultiple = (byte)1;
            var expectedPrice = 4.00m;
            var expectedPriceType = RegularPriceType;
            var expectedPriceTypeId = this.GetPriceChgTypeId(RegularPriceType);
            var expectedResultUnitAbbreviation = ForEachAbbrev;
            var expectedRetailUnitId = this.GetUnitId(ForEachAbbrev);
            var storeNo = 73245732;
            var subTeamNo = this.GetFirstSubTeamNo();

            // Insert New Item
            var itemKey = this.dbProvider.Insert(
                new IrmaQueryParams<Item, int>(
                    IrmaTestObjectFactory.BuildItem()
                        .With(x => x.Retail_Unit_ID, expectedRetailUnitId)
                        .With(x => x.SubTeam_No, subTeamNo)
                        .ToObject(),
                x => x.Item_Key));

            // Insert New Item Identifier
            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, expectedIdentifier)
                        .ToObject(),
                    x => x.Identifier_ID));

            // Insert New Item Identifier
            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, "7777777777778")
                        .With(x => x.Default_Identifier, (byte)0)
                        .ToObject(),
                    x => x.Identifier_ID));

            // Insert New Store
            this.dbProvider.Insert(IrmaTestObjectFactory.BuildStore()
                .With(x => x.Store_No, storeNo)
                .With(x => x.BusinessUnit_ID, expectedBusinessUnitId)
                .With(x => x.StoreJurisdictionID, 1)
                .ToObject());

            // Insert New Store Region Mapping
            this.dbProvider.Insert(IrmaTestObjectFactory.Build<StoreRegionMapping>()
                .With(x => x.Store_No, storeNo)
                .With(x => x.Region_Code, this.region)
                .ToObject());

            // Insert New Price
            this.dbProvider.Insert(new IrmaQueryParams<Price, int>(
                IrmaTestObjectFactory.Build<Price>()
                    .With(x => x.Price1, expectedPrice)
                    .With(x => x.Multiple, expectedMultiple)
                    .With(x => x.PriceChgTypeId, expectedPriceTypeId)
                    .With(x => x.Store_No, storeNo)
                    .With(x => x.Item_Key, itemKey)
                    .ToObject(),
                null,
                new Dictionary<string, string> { { "Price1", "Price" } }));

            // Insert New Validated Scan Code
            this.dbProvider.Insert(new IrmaQueryParams<ValidatedScanCode, int>(
                IrmaTestObjectFactory.BuildValidatedScanCode()
                    .With(x => x.ScanCode, expectedIdentifier)
                    .ToObject(),
                x => x.Id));

            // Insert New Validated Scan Code
            this.dbProvider.Insert(new IrmaQueryParams<ValidatedScanCode, int>(
                IrmaTestObjectFactory.BuildValidatedScanCode()
                    .With(x => x.ScanCode, expectedIdentifier2)
                    .ToObject(),
                x => x.Id));

            this.InsertIntoEventQueue(new List<TestItemsForPriceEvents>
            {
                new TestItemsForPriceEvents
                {
                    Item_Key = itemKey,
                    Identifier = expectedIdentifier2,
                    Store_No = storeNo,
                }
            });

            // When
            var actual = this.query.Search(this.parameters).Single();

            // Then
            Assert.AreEqual(expectedPrice, actual.CurrentRegularPrice);
            Assert.AreEqual(expectedPrice, actual.NewRegularPrice);
            Assert.AreEqual(expectedMultiple, actual.CurrentRegularMultiple);
            Assert.AreEqual(expectedMultiple, actual.NewRegularMultiple);
            Assert.AreEqual(expectedIdentifier2, actual.ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, actual.BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, actual.PriceUom);
            Assert.AreEqual(DateTime.Today, actual.NewStartDate);
            Assert.AreEqual(expectedPriceType, actual.NewPriceType);
            Assert.IsNull(actual.NewSaleEndDate);
            Assert.IsNull(actual.NewSalePrice);
            Assert.IsNull(actual.NewSaleMultiple);
            Assert.IsNull(actual.CurrentSalePrice);
            Assert.IsNull(actual.CurrentSaleMultiple);
            Assert.IsNull(actual.CurrentSaleStartDate);
            Assert.IsNull(actual.CurrentSaleEndDate);
            Assert.IsNull(actual.CurrentPriceType);
            Assert.IsNull(actual.ErrorMessage);
            Assert.AreEqual(this.region, actual.Region);
        }

        [TestMethod]
        public void GetExistingPriceEventsQuery_EventRowsHavePromotion_ReturnOneRowForRegularPriceAndOneRowForPromoPrice()
        {
            // Given
            var expectedPrice = 4.00m;
            var expectedMultiple = (byte)1;
            var expectedIdentifier = "7777777777777";
            var expectedBusinessUnitId = 723;
            var expectedPriceType = SalePriceType;
            var expectedPriceTypeId = this.GetPriceChgTypeId(SalePriceType);
            var expectedResultUnitAbbreviation = ForEachAbbrev;
            var expectedRetailUnitId = this.GetUnitId(ForEachAbbrev);
            var expectedSalePrice = 1.00m;
            var expectedSaleMultiple = (byte)2;
            var expectedSaleStartDate = DateTime.Today.AddDays(-7);
            var expectedSaleEndDate = DateTime.Today.AddDays(5);
            var storeNo = 73245732;
            var subTeamNo = this.GetFirstSubTeamNo();

            // Insert New Item
            var itemKey = this.dbProvider.Insert(
                new IrmaQueryParams<Item, int>(
                    IrmaTestObjectFactory.BuildItem()
                        .With(x => x.Retail_Unit_ID, expectedRetailUnitId)
                        .With(x => x.SubTeam_No, subTeamNo)
                        .ToObject(),
                x => x.Item_Key));

            // Insert New Item Identifier
            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, expectedIdentifier)
                        .ToObject(),
                    x => x.Identifier_ID));

            // Insert New Item Identifier
            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, "7777777777778")
                        .With(x => x.Default_Identifier, (byte)0)
                        .ToObject(),
                    x => x.Identifier_ID));

            // Insert New Store
            this.dbProvider.Insert(IrmaTestObjectFactory.BuildStore()
                .With(x => x.Store_No, storeNo)
                .With(x => x.BusinessUnit_ID, expectedBusinessUnitId)
                .With(x => x.StoreJurisdictionID, 1)
                .ToObject());

            // Insert New Store Region Mapping
            this.dbProvider.Insert(IrmaTestObjectFactory.Build<StoreRegionMapping>()
                .With(x => x.Store_No, storeNo)
                .With(x => x.Region_Code, this.region)
                .ToObject());

            // Insert New Price
            this.dbProvider.Insert(new IrmaQueryParams<Price, int>(
                IrmaTestObjectFactory.Build<Price>()
                    .With(x => x.Price1, expectedPrice)
                    .With(x => x.Multiple, expectedMultiple)
                    .With(x => x.PriceChgTypeId, expectedPriceTypeId)
                    .With(x => x.Store_No, storeNo)
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Sale_Price, expectedSalePrice)
                    .With(x => x.Sale_Multiple, expectedSaleMultiple)
                    .With(x => x.Sale_Start_Date, expectedSaleStartDate)
                    .With(x => x.Sale_End_Date, expectedSaleEndDate)
                    .ToObject(),
                null,
                new Dictionary<string, string> { { "Price1", "Price" } }));


            // Insert New Validated Scan Code
            this.dbProvider.Insert(new IrmaQueryParams<ValidatedScanCode, int>(
                IrmaTestObjectFactory.BuildValidatedScanCode()
                    .With(x => x.ScanCode, expectedIdentifier)
                    .ToObject(),
                x => x.Id));

            this.InsertIntoEventQueue(new List<TestItemsForPriceEvents>
            {
                new TestItemsForPriceEvents
                {
                    Item_Key = itemKey,
                    Identifier = expectedIdentifier,
                    Store_No = storeNo,
                }
            });

            // When
            var results = this.query.Search(this.parameters);

            // Then
            Assert.AreEqual(2, results.Count);

            Assert.AreEqual(expectedPrice, results.First().CurrentRegularPrice);
            Assert.AreEqual(expectedPrice, results.First().NewRegularPrice);
            Assert.AreEqual(expectedMultiple, results.First().CurrentRegularMultiple);
            Assert.AreEqual(expectedMultiple, results.First().NewRegularMultiple);
            Assert.AreEqual(expectedIdentifier, results.First().ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, results.First().BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, results.First().PriceUom);
            Assert.AreEqual(expectedSaleStartDate, results.First().NewStartDate);
            Assert.AreEqual(RegularPriceType, results.First().NewPriceType);
            Assert.AreEqual(null, results.First().NewSaleEndDate);
            Assert.AreEqual(null, results.First().NewSalePrice);
            Assert.AreEqual(null, results.First().NewSaleMultiple);
            Assert.IsNull(results.First().CurrentSalePrice);
            Assert.IsNull(results.First().CurrentSaleMultiple);
            Assert.IsNull(results.First().CurrentSaleStartDate);
            Assert.IsNull(results.First().CurrentSaleEndDate);
            Assert.IsNull(results.First().CurrentPriceType);
            Assert.IsNull(results.First().ErrorMessage);
            Assert.AreEqual(this.region, results.First().Region);

            Assert.AreEqual(expectedPrice, results[1].CurrentRegularPrice);
            Assert.AreEqual(expectedMultiple, results[1].CurrentRegularMultiple);
            Assert.AreEqual(expectedPrice, results[1].NewRegularPrice);
            Assert.AreEqual(expectedMultiple, results[1].NewRegularMultiple);
            Assert.AreEqual(expectedIdentifier, results[1].ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, results[1].BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, results[1].PriceUom);
            Assert.AreEqual(expectedSaleStartDate, results[1].NewStartDate);
            Assert.AreEqual(expectedPriceType, results[1].NewPriceType);
            Assert.AreEqual(expectedSaleEndDate, results[1].NewSaleEndDate);
            Assert.AreEqual(expectedSalePrice, results[1].NewSalePrice);
            Assert.AreEqual(expectedSaleMultiple, results[1].NewSaleMultiple);
            Assert.IsNull(results[1].CurrentSalePrice);
            Assert.IsNull(results[1].CurrentSaleMultiple);
            Assert.IsNull(results[1].CurrentSaleStartDate);
            Assert.IsNull(results[1].CurrentSaleEndDate);
            Assert.IsNull(results[1].CurrentPriceType);
            Assert.IsNull(results[1].ErrorMessage);
            Assert.AreEqual(this.region, results[1].Region);
        }

        [TestMethod]
        public void GetExistingPriceEventsQuery_EventRowsHaveBothPromotionAndOnlyRegularPrices_ReturnOneRowForEachRegularPriceAndTwoRowsForEachPromo()
        {
            // Given
            var expectedMultiple = (byte)1;
            var expectedPrice = 4.00m;
            var expectedIdentifier = "7777777777777";
            var expectedSecondPrice = 8.00m;
            var expectedSecondIdentifier = "7777777777778";
            var expectedBusinessUnitId = 723;
            var expectedPriceType = RegularPriceType;
            var expectedPriceTypeId = this.GetPriceChgTypeId(RegularPriceType);
            var expectedSalePriceType = SalePriceType;
            var expectedSalePriceTypeId = this.GetPriceChgTypeId(SalePriceType);
            var expectedResultUnitAbbreviation = ForEachAbbrev;
            var expectedRetailUnitId = this.GetUnitId(ForEachAbbrev);
            var expectedSalePrice = 1.00m;
            var expectedSaleMultiple = (byte)2;
            var expectedSaleStartDate = DateTime.Today.AddDays(-7);
            var expectedSaleEndDate = DateTime.Today.AddDays(5);
            var storeNo = 73245732;
            var subTeamNo = this.GetFirstSubTeamNo();

            // Insert New Item
            var itemKey = this.dbProvider.Insert(
                new IrmaQueryParams<Item, int>(
                    IrmaTestObjectFactory.BuildItem()
                        .With(x => x.Retail_Unit_ID, expectedRetailUnitId)
                        .With(x => x.SubTeam_No, subTeamNo)
                        .ToObject(),
                x => x.Item_Key));

            // Insert New Item Identifier
            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, expectedIdentifier)
                        .ToObject(),
                    x => x.Identifier_ID));

            // Insert Second Item
            var secondItemKey = this.dbProvider.Insert(
                new IrmaQueryParams<Item, int>(
                    IrmaTestObjectFactory.BuildItem()
                        .With(x => x.Retail_Unit_ID, expectedRetailUnitId)
                        .With(x => x.SubTeam_No, subTeamNo)
                        .ToObject(),
                x => x.Item_Key));

            // Insert Second Item Identifier
            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, secondItemKey)
                        .With(x => x.Identifier, expectedSecondIdentifier)
                        .ToObject(),
                    x => x.Identifier_ID));

            // Insert New Store
            this.dbProvider.Insert(IrmaTestObjectFactory.BuildStore()
                .With(x => x.Store_No, storeNo)
                .With(x => x.BusinessUnit_ID, expectedBusinessUnitId)
                .With(x => x.StoreJurisdictionID, 1)
                .ToObject());

            // Insert New Store Region Mapping
            this.dbProvider.Insert(IrmaTestObjectFactory.Build<StoreRegionMapping>()
                .With(x => x.Store_No, storeNo)
                .With(x => x.Region_Code, this.region)
                .ToObject());

            // Insert New Price
            this.dbProvider.Insert(new IrmaQueryParams<Price, int>(
                IrmaTestObjectFactory.Build<Price>()
                    .With(x => x.Price1, expectedPrice)
                    .With(x => x.Multiple, expectedMultiple)
                    .With(x => x.PriceChgTypeId, expectedPriceTypeId)
                    .With(x => x.Store_No, storeNo)
                    .With(x => x.Item_Key, itemKey)
                    .ToObject(),
                null,
                new Dictionary<string, string> { { "Price1", "Price" } }));

            // Insert Second Price
            this.dbProvider.Insert(new IrmaQueryParams<Price, int>(
                IrmaTestObjectFactory.Build<Price>()
                    .With(x => x.Price1, expectedSecondPrice)
                    .With(x => x.Multiple, expectedMultiple)
                    .With(x => x.PriceChgTypeId, expectedSalePriceTypeId)
                    .With(x => x.Store_No, storeNo)
                    .With(x => x.Item_Key, secondItemKey)
                    .With(x => x.Sale_Price, expectedSalePrice)
                    .With(x => x.Sale_Multiple, expectedSaleMultiple)
                    .With(x => x.Sale_Start_Date, expectedSaleStartDate)
                    .With(x => x.Sale_End_Date, expectedSaleEndDate)
                    .ToObject(),
                null,
                new Dictionary<string, string> { { "Price1", "Price" } }));

            // Insert New Validated Scan Code
            this.dbProvider.Insert(new IrmaQueryParams<ValidatedScanCode, int>(
                IrmaTestObjectFactory.BuildValidatedScanCode()
                    .With(x => x.ScanCode, expectedIdentifier)
                    .ToObject(),
                x => x.Id));

            // Insert Second Validated Scan Code
            this.dbProvider.Insert(new IrmaQueryParams<ValidatedScanCode, int>(
                IrmaTestObjectFactory.BuildValidatedScanCode()
                    .With(x => x.ScanCode, expectedSecondIdentifier)
                    .ToObject(),
                x => x.Id));

            this.InsertIntoEventQueue(new List<TestItemsForPriceEvents>
                {
                    new TestItemsForPriceEvents
                    {
                        Item_Key = itemKey,
                        Identifier = expectedIdentifier,
                        Store_No = storeNo,
                    },
                    new TestItemsForPriceEvents
                    {
                        Item_Key = secondItemKey,
                        Identifier = expectedSecondIdentifier,
                        Store_No = storeNo,
                    }
                });

            // When
            var results = this.query.Search(this.parameters);

            // Then
            Assert.AreEqual(3, results.Count);

            Assert.AreEqual(expectedPrice, results.First().CurrentRegularPrice);
            Assert.AreEqual(expectedPrice, results.First().NewRegularPrice);
            Assert.AreEqual(expectedMultiple, results.First().CurrentRegularMultiple);
            Assert.AreEqual(expectedMultiple, results.First().NewRegularMultiple);
            Assert.AreEqual(expectedIdentifier, results.First().ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, results.First().BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, results.First().PriceUom);
            Assert.AreEqual(DateTime.Today, results.First().NewStartDate);
            Assert.AreEqual(expectedPriceType, results.First().NewPriceType);
            Assert.AreEqual(null, results.First().NewSaleEndDate);
            Assert.AreEqual(null, results.First().NewSalePrice);
            Assert.AreEqual(null, results.First().NewSaleMultiple);
            Assert.IsNull(results.First().CurrentSalePrice);
            Assert.IsNull(results.First().CurrentSaleMultiple);
            Assert.IsNull(results.First().CurrentSaleStartDate);
            Assert.IsNull(results.First().CurrentSaleEndDate);
            Assert.IsNull(results.First().CurrentPriceType);
            Assert.IsNull(results.First().ErrorMessage);
            Assert.AreEqual(this.region, results.First().Region);

            Assert.AreEqual(expectedSecondPrice, results[1].CurrentRegularPrice);
            Assert.AreEqual(expectedSecondPrice, results[1].NewRegularPrice);
            Assert.AreEqual(expectedMultiple, results[1].CurrentRegularMultiple);
            Assert.AreEqual(expectedMultiple, results[1].NewRegularMultiple);
            Assert.AreEqual(expectedSecondIdentifier, results[1].ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, results[1].BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, results[1].PriceUom);
            Assert.AreEqual(expectedSaleStartDate, results[1].NewStartDate);
            Assert.AreEqual(expectedPriceType, results[1].NewPriceType);
            Assert.AreEqual(null, results[1].NewSaleEndDate);
            Assert.AreEqual(null, results[1].NewSalePrice);
            Assert.AreEqual(null, results[1].NewSaleMultiple);
            Assert.IsNull(results[1].CurrentSalePrice);
            Assert.IsNull(results[1].CurrentSaleMultiple);
            Assert.IsNull(results[1].CurrentSaleStartDate);
            Assert.IsNull(results[1].CurrentSaleEndDate);
            Assert.IsNull(results[1].CurrentPriceType);
            Assert.IsNull(results[1].ErrorMessage);
            Assert.AreEqual(this.region, results[1].Region);

            Assert.AreEqual(expectedSecondPrice, results[2].CurrentRegularPrice);
            Assert.AreEqual(expectedSecondPrice, results[2].NewRegularPrice);
            Assert.AreEqual(expectedMultiple, results[2].CurrentRegularMultiple);
            Assert.AreEqual(expectedMultiple, results[2].NewRegularMultiple);
            Assert.AreEqual(expectedSecondIdentifier, results[2].ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, results[2].BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, results[2].PriceUom);
            Assert.AreEqual(expectedSaleStartDate, results[2].NewStartDate);
            Assert.AreEqual(expectedSalePriceType, results[2].NewPriceType);
            Assert.AreEqual(expectedSaleEndDate, results[2].NewSaleEndDate);
            Assert.AreEqual(expectedSalePrice, results[2].NewSalePrice);
            Assert.AreEqual(expectedSaleMultiple, results[2].NewSaleMultiple);
            Assert.IsNull(results[2].CurrentSalePrice);
            Assert.IsNull(results[2].CurrentSaleMultiple);
            Assert.IsNull(results[2].CurrentSaleStartDate);
            Assert.IsNull(results[2].CurrentSaleEndDate);
            Assert.IsNull(results[2].CurrentPriceType);
            Assert.IsNull(results[2].ErrorMessage);
            Assert.AreEqual(this.region, results[2].Region);
        }

        [TestMethod]
        public void GetExistingPriceDataQuery_OneItemNotOnPromotionFor365Region_ShouldReturnDataWithTSRegionAbbreviation()
        {
            // Given
            var expectedBusinessUnitId = 723;
            var expectedIdentifier = "7777777777777";
            var expectedMultiple = (byte)1;
            var expectedPrice = 4.00m;
            var expectedPriceType = RegularPriceType;
            var expectedPriceTypeId = this.GetPriceChgTypeId(RegularPriceType);
            var expectedResultUnitAbbreviation = ForEachAbbrev;
            var expectedRetailUnitId = this.GetUnitId(ForEachAbbrev);
            var storeNo = 73245732;
            var subTeamNo = this.GetFirstSubTeamNo();

            // Insert New Item
            var itemKey = this.dbProvider.Insert(
                new IrmaQueryParams<Item, int>(
                    IrmaTestObjectFactory.BuildItem()
                        .With(x => x.Retail_Unit_ID, expectedRetailUnitId)
                        .With(x => x.SubTeam_No, subTeamNo)
                        .ToObject(),
                x => x.Item_Key));

            // Insert New Item Identifier
            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, expectedIdentifier)
                        .ToObject(),
                    x => x.Identifier_ID));

            // Insert New Store
            this.dbProvider.Insert(IrmaTestObjectFactory.BuildStore()
                .With(x => x.Store_No, storeNo)
                .With(x => x.BusinessUnit_ID, expectedBusinessUnitId)
                .With(x => x.StoreJurisdictionID, 1)
                .ToObject());

            // Insert New Store Region Mapping
            this.dbProvider.Insert(IrmaTestObjectFactory.Build<StoreRegionMapping>()
                .With(x => x.Store_No, storeNo)
                .With(x => x.Region_Code, "TS")
                .ToObject());

            // Insert New Price
            this.dbProvider.Insert(new IrmaQueryParams<Price, int>(
                IrmaTestObjectFactory.Build<Price>()
                    .With(x => x.Price1, expectedPrice)
                    .With(x => x.Multiple, expectedMultiple)
                    .With(x => x.PriceChgTypeId, expectedPriceTypeId)
                    .With(x => x.Store_No, storeNo)
                    .With(x => x.Item_Key, itemKey)
                    .ToObject(),
                null,
                new Dictionary<string, string> { { "Price1", "Price" } }));

            // Insert New Validated Scan Code
            this.dbProvider.Insert(new IrmaQueryParams<ValidatedScanCode, int>(
                IrmaTestObjectFactory.BuildValidatedScanCode()
                    .With(x => x.ScanCode, expectedIdentifier)
                    .ToObject(),
                x => x.Id));

            this.InsertIntoEventQueue(new List<TestItemsForPriceEvents>
            {
                new TestItemsForPriceEvents
                {
                    Item_Key = itemKey,
                    Identifier = expectedIdentifier,
                    Store_No = storeNo,
                }
            });

            // When
            var results = this.query.Search(this.parameters);

            // Then
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(expectedPrice, results.First().CurrentRegularPrice);
            Assert.AreEqual(expectedPrice, results.First().NewRegularPrice);
            Assert.AreEqual(expectedMultiple, results.First().CurrentRegularMultiple);
            Assert.AreEqual(expectedMultiple, results.First().NewRegularMultiple);
            Assert.AreEqual(expectedIdentifier, results.First().ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, results.First().BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, results.First().PriceUom);
            Assert.AreEqual(DateTime.Today, results.First().NewStartDate);
            Assert.AreEqual(expectedPriceType, results.First().NewPriceType);
            Assert.IsNull(results.First().NewSaleEndDate);
            Assert.IsNull(results.First().NewSalePrice);
            Assert.IsNull(results.First().NewSaleMultiple);
            Assert.IsNull(results.First().CurrentSalePrice);
            Assert.IsNull(results.First().CurrentSaleMultiple);
            Assert.IsNull(results.First().CurrentSaleStartDate);
            Assert.IsNull(results.First().CurrentSaleEndDate);
            Assert.IsNull(results.First().CurrentPriceType);
            Assert.IsNull(results.First().ErrorMessage);
            Assert.AreEqual("TS", results.First().Region);
        }

        [TestMethod]
        public void GetExistingPriceDataQuery_OneItemFor365RegionAndOneItemForRmRegion_ShouldReturnRowForEachItemWithRespectiveRegionAbbreviation()
        {
            // Given
            var expectedTSBusinessUnitId = 724;
            var expectedTsPrice = 7.00m;
            var expectedBusinessUnitId = 723;
            var expectedIdentifier = "7777777777777";
            var expectedMultiple = (byte)1;
            var expectedPrice = 4.00m;
            var expectedPriceType = RegularPriceType;
            var expectedPriceTypeId = this.GetPriceChgTypeId(RegularPriceType);
            var expectedResultUnitAbbreviation = ForEachAbbrev;
            var expectedRetailUnitId = this.GetUnitId(ForEachAbbrev);
            var storeNo = 73245732;
            var tsStoreNo = 8394829;
            var subTeamNo = this.GetFirstSubTeamNo();

            // Given
            // Insert New Item
            var itemKey = this.dbProvider.Insert(
                new IrmaQueryParams<Item, int>(
                    IrmaTestObjectFactory.BuildItem()
                        .With(x => x.Retail_Unit_ID, expectedRetailUnitId)
                        .With(x => x.SubTeam_No, subTeamNo)
                        .ToObject(),
                x => x.Item_Key));

            // Insert New Item Identifier
            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, expectedIdentifier)
                        .ToObject(),
                    x => x.Identifier_ID));

            // Insert New Store
            this.dbProvider.Insert(IrmaTestObjectFactory.BuildStore()
                .With(x => x.Store_No, storeNo)
                .With(x => x.BusinessUnit_ID, expectedBusinessUnitId)
                .With(x => x.StoreJurisdictionID, 1)
                .ToObject());

            // Insert New Store Region Mapping
            this.dbProvider.Insert(IrmaTestObjectFactory.Build<StoreRegionMapping>()
                .With(x => x.Store_No, storeNo)
                .With(x => x.Region_Code, this.region)
                .ToObject());

            // Insert New TS Store
            this.dbProvider.Insert(IrmaTestObjectFactory.BuildStore()
                .With(x => x.Store_No, tsStoreNo)
                .With(x => x.Store_Name, "Test TS Store")
                .With(x => x.BusinessUnit_ID, expectedTSBusinessUnitId)
                .With(x => x.StoreJurisdictionID, 1)
                .ToObject());

            // Insert New TS Store Region Mapping
            this.dbProvider.Insert(IrmaTestObjectFactory.Build<StoreRegionMapping>()
                .With(x => x.Store_No, tsStoreNo)
                .With(x => x.Region_Code, "TS")
                .ToObject());

            // Insert New Price
            this.dbProvider.Insert(new IrmaQueryParams<Price, int>(
                IrmaTestObjectFactory.Build<Price>()
                    .With(x => x.Price1, expectedPrice)
                    .With(x => x.Multiple, expectedMultiple)
                    .With(x => x.PriceChgTypeId, expectedPriceTypeId)
                    .With(x => x.Store_No, storeNo)
                    .With(x => x.Item_Key, itemKey)
                    .ToObject(),
                null,
                new Dictionary<string, string> { { "Price1", "Price" } }));

            // Insert TS New Price
            this.dbProvider.Insert(new IrmaQueryParams<Price, int>(
                IrmaTestObjectFactory.Build<Price>()
                    .With(x => x.Price1, expectedTsPrice)
                    .With(x => x.Multiple, expectedMultiple)
                    .With(x => x.PriceChgTypeId, expectedPriceTypeId)
                    .With(x => x.Store_No, tsStoreNo)
                    .With(x => x.Item_Key, itemKey)
                    .ToObject(),
                null,
                new Dictionary<string, string> { { "Price1", "Price" } }));

            // Insert New Validated Scan Code
            this.dbProvider.Insert(new IrmaQueryParams<ValidatedScanCode, int>(
                IrmaTestObjectFactory.BuildValidatedScanCode()
                    .With(x => x.ScanCode, expectedIdentifier)
                    .ToObject(),
                x => x.Id));

            this.InsertIntoEventQueue(new List<TestItemsForPriceEvents>
                {
                    new TestItemsForPriceEvents
                    {
                        Item_Key = itemKey,
                        Identifier = expectedIdentifier,
                        Store_No = storeNo,
                    },
                    new TestItemsForPriceEvents
                    {
                        Item_Key = itemKey,
                        Identifier = expectedIdentifier,
                        Store_No = tsStoreNo,
                    }
                });

            // When
            var results = this.query.Search(this.parameters).OrderBy(x => x.Region).ToList();

            // Then
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(expectedPrice, results.First().CurrentRegularPrice);
            Assert.AreEqual(expectedPrice, results.First().NewRegularPrice);
            Assert.AreEqual(expectedMultiple, results.First().CurrentRegularMultiple);
            Assert.AreEqual(expectedMultiple, results.First().NewRegularMultiple);
            Assert.AreEqual(expectedIdentifier, results.First().ScanCode);
            Assert.AreEqual(expectedBusinessUnitId, results.First().BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, results.First().PriceUom);
            Assert.AreEqual(DateTime.Today, results.First().NewStartDate);
            Assert.AreEqual(expectedPriceType, results.First().NewPriceType);
            Assert.IsNull(results.First().NewSaleEndDate);
            Assert.IsNull(results.First().NewSalePrice);
            Assert.IsNull(results.First().NewSaleMultiple);
            Assert.IsNull(results.First().CurrentSalePrice);
            Assert.IsNull(results.First().CurrentSaleMultiple);
            Assert.IsNull(results.First().CurrentSaleStartDate);
            Assert.IsNull(results.First().CurrentSaleEndDate);
            Assert.IsNull(results.First().CurrentPriceType);
            Assert.IsNull(results.First().ErrorMessage);
            Assert.AreEqual(this.region, results.First().Region);

            Assert.AreEqual(expectedTsPrice, results[1].CurrentRegularPrice);
            Assert.AreEqual(expectedTsPrice, results[1].NewRegularPrice);
            Assert.AreEqual(expectedMultiple, results[1].CurrentRegularMultiple);
            Assert.AreEqual(expectedMultiple, results[1].NewRegularMultiple);
            Assert.AreEqual(expectedIdentifier, results[1].ScanCode);
            Assert.AreEqual(expectedTSBusinessUnitId, results[1].BusinessUnitId);
            Assert.AreEqual(expectedResultUnitAbbreviation, results[1].PriceUom);
            Assert.AreEqual(DateTime.Today, results[1].NewStartDate);
            Assert.AreEqual(expectedPriceType, results[1].NewPriceType);
            Assert.IsNull(results[1].NewSaleEndDate);
            Assert.IsNull(results[1].NewSalePrice);
            Assert.IsNull(results[1].NewSaleMultiple);
            Assert.IsNull(results[1].CurrentSalePrice);
            Assert.IsNull(results[1].CurrentSaleMultiple);
            Assert.IsNull(results[1].CurrentSaleStartDate);
            Assert.IsNull(results[1].CurrentSaleEndDate);
            Assert.IsNull(results[1].CurrentPriceType);
            Assert.IsNull(results[1].ErrorMessage);
            Assert.AreEqual("TS", results[1].Region);
        }

        #region Private Methods

        private void InsertIntoEventQueue(List<TestItemsForPriceEvents> items)
        {
            foreach (var item in items)
            {
                dbProvider.Connection.Execute(
                    @"insert into mammoth.PriceChangeQueue(Item_Key, Store_No, Identifier, EventTypeID, EventReferenceID, InProcessBy)
                         values(@Item_Key, @Store_No, @Identifier, @EventTypeID, @EventReferenceID, @Instance)",
                    new
                    {
                        Item_Key = item.Item_Key,
                        Store_No = item.Store_No,
                        Identifier = item.Identifier,
                        EventTypeID = IrmaEventTypes.Price,
                        EventReferenceID = item.PriceBatchDetailID,
                        Instance = parameters.Instance
                    },
                    transaction: this.dbProvider.Transaction);
            }
        }

        private byte GetPriceChgTypeId(string value)
        {
            return dbProvider.GetLookupId<byte>("PriceChgTypeID", "PriceChgType", "PriceChgTypeDesc", value);
        }
        private int GetUnitId(string value)
        {
            return dbProvider.GetLookupId<int>("Unit_ID", "ItemUnit", "Unit_Abbreviation", value);
        }

        private int GetFirstSubTeamNo()
        {
            var subTeamNo = dbProvider.Connection.Query<int>(
               "SELECT Top 1 SubTeam_No FROM SubTeam",
               null,
               dbProvider.Transaction).First();

            return subTeamNo;
        }

        #endregion

        private class TestItemsForPriceEvents
        {
            public int? PriceBatchDetailID { get; set; }
            public string Identifier { get; set; }
            public int Item_Key { get; set; }
            public int Store_No { get; set; }
        }
    }
}
