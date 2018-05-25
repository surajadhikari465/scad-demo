using Dapper;
using Irma.Framework;
using Irma.Testing;
using Irma.Testing.Builders;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Price.Controller.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Price.Controller.DataAccess.Tests.Queries
{
    [TestClass]
    public class GetCancelAllSalesDataQueryTests
    {
        private GetCancelAllSalesDataQuery query;
        private GetCancelAllSalesDataParameters parameters;
        private SqlDbProvider dbProvider;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = IrmaTestDbProviderFactory.CreateAndOpen("FL");
            query = new GetCancelAllSalesDataQuery(dbProvider);
            parameters = new GetCancelAllSalesDataParameters
            {
                Instance = 454
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
        public void GetCancelAllSalesData_EventsExist_ShouldReturnEvents()
        {
            //Given
            var expectedIdentifier = "9988776631";
            var storeNo = 123421;
            var expectedBusinessUnitId = 999999;
            var subTeamNo = 1234567;
            var priceBatchDetailStartDate = DateTime.Today.AddDays(10);

            dbProvider.Connection.Execute(
                "insert into dbo.SubTeam(SubTeam_No) values(@SubTeamNo)", 
                new { SubTeamNo = subTeamNo },
                dbProvider.Transaction);

            // Insert New Item
            var itemKey = this.dbProvider.Insert(
                new IrmaQueryParams<Item, int>(
                    IrmaTestObjectFactory.BuildItem()
                        .With(x => x.Retail_Unit_ID, GetUnitId("EA"))
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

            this.dbProvider.Connection.Execute(
                $@"INSERT INTO dbo.ValidatedScanCode(ScanCode, InsertDate, InforItemId)
                  VALUES ({expectedIdentifier}, GETDATE(), -1)",
                transaction: dbProvider.Transaction);

            // Insert New Store
            this.dbProvider.Insert(IrmaTestObjectFactory.BuildStore()
                .With(x => x.Store_No, storeNo)
                .With(x => x.BusinessUnit_ID, expectedBusinessUnitId)
                .With(x => x.StoreJurisdictionID, 1)
                .ToObject());

            // Insert New Store Region Mapping
            this.dbProvider.Insert(IrmaTestObjectFactory.Build<StoreRegionMapping>()
                .With(x => x.Store_No, storeNo)
                .With(x => x.Region_Code, "FL")
                .ToObject());

            PriceBatchDetail newPriceBatchDetail = new TestPriceBatchDetailBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifier)
                .WithPrice(1)
                .WithMultiple(1)
                .WithPriceChgTypeID(1)
                .WithStore_No(storeNo)
                .WithStartDate(priceBatchDetailStartDate)
                .WithCancelAllSales(true);
            int priceBatchDetailId = this.AddPriceBatchDetail(newPriceBatchDetail);

            InsertIntoEventQueue(new List<TestItemsForPriceEvents>
            {
                new TestItemsForPriceEvents
                {
                    Identifier = expectedIdentifier,
                    Item_Key = itemKey,
                    Store_No = storeNo,
                    PriceBatchDetailID = priceBatchDetailId
                }
            });

            //When
            var events = query.Search(parameters);

            //Then
            Assert.AreEqual(1, events.Count);
            var cancelAllSalesEvent = events.First();
            AssertEventIsEqualToExpectedData(cancelAllSalesEvent, expectedIdentifier, expectedBusinessUnitId, priceBatchDetailStartDate);
        }

        [TestMethod]
        public void GetCancelAllSalesData_ItemHasAlternateIdentifiers_ShouldReturnAnEventForEachIdentifier()
        {
            //Given
            var expectedDefaultIdentifier = "9988776630";
            var expectedAlternateIdentifier1 = "9988776631";
            var expectedAlternateIdentifier2 = "9988776632";
            var expectedAlternateIdentifier3 = "9988776633";
            var expectedAlternateIdentifier4 = "9988776634";
            var storeNo = 123421;
            var expectedBusinessUnitId = 999999;
            var subTeamNo = 1234567;
            var priceBatchDetailStartDate = DateTime.Today.AddDays(10);

            dbProvider.Connection.Execute(
                "insert into dbo.SubTeam(SubTeam_No) values(@SubTeamNo)",
                new { SubTeamNo = subTeamNo },
                dbProvider.Transaction);

            // Insert New Item
            var itemKey = this.dbProvider.Insert(
                new IrmaQueryParams<Item, int>(
                    IrmaTestObjectFactory.BuildItem()
                        .With(x => x.Retail_Unit_ID, GetUnitId("EA"))
                        .With(x => x.SubTeam_No, subTeamNo)
                        .ToObject(),
                x => x.Item_Key));

            // Insert New Item Identifiers
            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, expectedDefaultIdentifier)
                        .With(x => x.Default_Identifier, (byte)1)
                        .ToObject(),
                    x => x.Identifier_ID));

            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, expectedAlternateIdentifier1)
                        .With(x => x.Default_Identifier, (byte)0)
                        .ToObject(),
                    x => x.Identifier_ID));

            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, expectedAlternateIdentifier2)
                        .With(x => x.Default_Identifier, (byte)0)
                        .ToObject(),
                    x => x.Identifier_ID));

            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, expectedAlternateIdentifier3)
                        .With(x => x.Default_Identifier, (byte)0)
                        .ToObject(),
                    x => x.Identifier_ID));

            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, expectedAlternateIdentifier4)
                        .With(x => x.Default_Identifier, (byte)0)
                        .ToObject(),
                    x => x.Identifier_ID));

            this.dbProvider.Connection.Execute(
                $@"INSERT INTO dbo.ValidatedScanCode(ScanCode, InsertDate, InforItemId)
                  VALUES ({expectedDefaultIdentifier}, GETDATE(), -1),
                         ({expectedAlternateIdentifier1}, GETDATE(), -2),
                         ({expectedAlternateIdentifier2}, GETDATE(), -3),
                         ({expectedAlternateIdentifier3}, GETDATE(), -4),
                         ({expectedAlternateIdentifier4}, GETDATE(), -5)",
                transaction: dbProvider.Transaction);

            // Insert New Store
            this.dbProvider.Insert(IrmaTestObjectFactory.BuildStore()
                .With(x => x.Store_No, storeNo)
                .With(x => x.BusinessUnit_ID, expectedBusinessUnitId)
                .With(x => x.StoreJurisdictionID, 1)
                .ToObject());

            // Insert New Store Region Mapping
            this.dbProvider.Insert(IrmaTestObjectFactory.Build<StoreRegionMapping>()
                .With(x => x.Store_No, storeNo)
                .With(x => x.Region_Code, "FL")
                .ToObject());

            PriceBatchDetail newPriceBatchDetail = new TestPriceBatchDetailBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedDefaultIdentifier)
                .WithPrice(1)
                .WithMultiple(1)
                .WithPriceChgTypeID(1)
                .WithStore_No(storeNo)
                .WithStartDate(priceBatchDetailStartDate)
                .WithCancelAllSales(true);
            int priceBatchDetailId = this.AddPriceBatchDetail(newPriceBatchDetail);

            InsertIntoEventQueue(new List<TestItemsForPriceEvents>
            {
                new TestItemsForPriceEvents
                {
                    Identifier = expectedDefaultIdentifier,
                    Item_Key = itemKey,
                    Store_No = storeNo,
                    PriceBatchDetailID = priceBatchDetailId
                }
            });

            //When
            var events = query.Search(parameters).OrderBy(e => e.ScanCode).ToList();

            //Then
            Assert.AreEqual(5, events.Count);
            AssertEventIsEqualToExpectedData(events[0], expectedDefaultIdentifier, expectedBusinessUnitId, priceBatchDetailStartDate);
            AssertEventIsEqualToExpectedData(events[1], expectedAlternateIdentifier1, expectedBusinessUnitId, priceBatchDetailStartDate);
            AssertEventIsEqualToExpectedData(events[2], expectedAlternateIdentifier2, expectedBusinessUnitId, priceBatchDetailStartDate);
            AssertEventIsEqualToExpectedData(events[3], expectedAlternateIdentifier3, expectedBusinessUnitId, priceBatchDetailStartDate);
            AssertEventIsEqualToExpectedData(events[4], expectedAlternateIdentifier4, expectedBusinessUnitId, priceBatchDetailStartDate);
        }

        private static void AssertEventIsEqualToExpectedData(Models.CancelAllSalesEventModel cancelAllSalesEvent, string expectedIdentifier, int expectedBusinessUnitId, DateTime priceBatchDetailStartDate)
        {
            Assert.AreEqual(expectedBusinessUnitId, cancelAllSalesEvent.BusinessUnitId);
            Assert.AreEqual(IrmaEventTypes.CancelAllSales, cancelAllSalesEvent.EventTypeId);
            Assert.AreEqual("FL", cancelAllSalesEvent.Region);
            Assert.AreEqual(expectedIdentifier, cancelAllSalesEvent.ScanCode);
            Assert.AreEqual(priceBatchDetailStartDate, cancelAllSalesEvent.EndDate);
        }

        private int GetUnitId(string value)
        {
            return dbProvider.GetLookupId<int>("Unit_ID", "ItemUnit", "Unit_Abbreviation", value);
        }

        private void InsertIntoEventQueue(List<TestItemsForPriceEvents> items)
        {
            foreach (var item in items)
            {
                dbProvider.Connection.Execute(
                    @"
                    insert into mammoth.PriceChangeQueue(Item_Key, Store_No, Identifier, EventTypeID, EventReferenceID, InProcessBy)
                    values(@Item_Key, @Store_No, @Identifier, @EventTypeID, @EventReferenceID, @Instance)",
                    new
                    {
                        Item_Key = item.Item_Key,
                        Store_No = item.Store_No,
                        Identifier = item.Identifier,
                        EventTypeID = IrmaEventTypes.CancelAllSales,
                        EventReferenceID = item.PriceBatchDetailID,
                        Instance = parameters.Instance
                    },
                    transaction: this.dbProvider.Transaction);
            }
        }

        private class TestItemsForPriceEvents
        {
            public int? PriceBatchDetailID { get; set; }
            public string Identifier { get; set; }
            public int Item_Key { get; set; }
            public int Store_No { get; set; }
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
	                    ,[ItemSurcharge])
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
                        @ItemSurcharge)

                    SELECT SCOPE_IDENTITY();";

            int priceBatchDetailId = this.dbProvider.Connection.Query<int>(sql,
                newPriceBatchDetail,
                this.dbProvider.Transaction).First();

            return priceBatchDetailId;
        }
    }
}
