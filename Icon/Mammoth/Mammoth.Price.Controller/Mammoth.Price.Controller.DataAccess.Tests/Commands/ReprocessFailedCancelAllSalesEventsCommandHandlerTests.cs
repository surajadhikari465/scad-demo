using Dapper;
using Irma.Framework;
using Irma.Testing;
using Irma.Testing.Builders;
using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Logging;
using Mammoth.Price.Controller.Common;
using Mammoth.Price.Controller.DataAccess.Commands;
using Mammoth.Price.Controller.DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using static Mammoth.Common.Constants;

namespace Mammoth.Price.Controller.DataAccess.Tests.Commands
{
    [TestClass]
    public class ReprocessFailedCancelAllSalesEventsCommandHandlerTests
    {
        private ReprocessFailedCancelAllSalesEventsCommandHandler commandHandler;
        private TransactionScope transaction;
        private PriceControllerApplicationSettings settings;
        private Mock<ILogger> logger;
        private ReprocessFailedCancelAllSalesEventsCommand data;
        private IDbProvider dbProvider;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.Snapshot
            });
            dbProvider = IrmaTestDbProviderFactory.CreateAndOpen("MA"); ;
            settings = new PriceControllerApplicationSettings();
            logger = new Mock<ILogger>();
            commandHandler = new ReprocessFailedCancelAllSalesEventsCommandHandler(
                dbProvider,
                settings,
                logger.Object);
            settings.ReprocessCount = 3;
            data = new ReprocessFailedCancelAllSalesEventsCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void ReprocessFailedCancelAllSalesEvents_FailedCancelAllSalesEventsHaveSmallerReprocessCountThanSettings_InsertsEventsBackIntoTheQueue()
        {
            //Given
            var expectedIdentifier = "9988776631";
            var storeNo = 123421;
            var expectedBusinessUnitId = 999999;
            var subTeamNo = 1234567;
            var priceBatchDetailStartDate = DateTime.Today.AddDays(10);
            var expectedEventCreatedDate = DateTime.Today;
            int itemKey, priceBatchDetailId;
            InsertTestSubTeam(subTeamNo);
            InsertTestStore(storeNo, expectedBusinessUnitId);
            InsertTestData(expectedIdentifier, storeNo, subTeamNo, priceBatchDetailStartDate, out itemKey, out priceBatchDetailId);

            data.CancelAllSales = new List<CancelAllSalesEventModel>
            {
                new CancelAllSalesEventModel
                {
                    QueueId = 1,
                    ErrorMessage = "Test"
                }
            };
            data.Events = new List<EventQueueModel>
            {
                new EventQueueModel
                {
                    EventReferenceId = priceBatchDetailId,
                    EventTypeId = EventTypes.CancelAllSales,
                    Identifier = expectedIdentifier,
                    InProcessBy = null,
                    InsertDate = expectedEventCreatedDate,
                    ItemKey = itemKey,
                    ProcessFailedDate = null,
                    QueueId = 1,
                    ReprocessCount = null,
                    StoreNo = storeNo
                }
            };

            //When
            commandHandler.Execute(data);

            //Then
            var actualEvent = dbProvider.Connection.QuerySingle<EventQueueModel>(
                @"
                SELECT QueueID as QueueId,
                    Item_Key as ItemKey,
                    Store_No as StoreNo,
                    Identifier as Identifier,
                    EventTypeID as EventTypeId,
                    EventReferenceID as EventReferenceId,
                    InsertDate,
                    ProcessFailedDate,
                    InProcessBy,
                    ReprocessCount
                FROM mammoth.PriceChangeQueue WHERE Item_Key = @ItemKey AND Store_No = @StoreNo",
                new { ItemKey = itemKey, StoreNo = storeNo });

            var expectedEvent = data.Events.First();
            Assert.AreEqual(expectedEvent.EventReferenceId, actualEvent.EventReferenceId);
            Assert.AreEqual(expectedEvent.EventTypeId, actualEvent.EventTypeId);
            Assert.AreEqual(expectedEvent.Identifier, actualEvent.Identifier);
            Assert.AreEqual(null, actualEvent.InProcessBy);
            Assert.AreEqual(expectedEvent.InsertDate, actualEvent.InsertDate);
            Assert.AreEqual(expectedEvent.ItemKey, actualEvent.ItemKey);
            Assert.AreEqual(null, actualEvent.ProcessFailedDate);
            Assert.AreEqual(1, actualEvent.ReprocessCount);
            Assert.AreEqual(expectedEvent.StoreNo, actualEvent.StoreNo);
        }



        [TestMethod]
        public void ReprocessFailedCancelAllSalesEvents_MultipleFailedCancelAllSalesEventsHaveSmallerReprocessCountThanSettings_InsertsEventsBackIntoTheQueue()
        {
            //Given
            var expectedIdentifier = "9988776631";
            var expectedIdentifier2 = "9988776632";
            var storeNo = 123421;
            var expectedBusinessUnitId = 999999;
            var subTeamNo = 1234567;
            var priceBatchDetailStartDate = DateTime.Today.AddDays(10);
            var expectedEventCreatedDate = DateTime.Today;
            var expectedEventCreatedDate2 = DateTime.Today.AddMinutes(10);
            int itemKey1 = 0;
            int itemKey2 = 0;
            int priceBatchDetailId1 = 0;
            int priceBatchDetailId2 = 0;
            InsertTestSubTeam(subTeamNo);
            InsertTestStore(storeNo, expectedBusinessUnitId);
            InsertTestData(expectedIdentifier, storeNo, subTeamNo, priceBatchDetailStartDate, out itemKey1, out priceBatchDetailId1);
            InsertTestData(expectedIdentifier2, storeNo, subTeamNo, priceBatchDetailStartDate, out itemKey2, out priceBatchDetailId2);

            data.CancelAllSales = new List<CancelAllSalesEventModel>
            {
                new CancelAllSalesEventModel
                {
                    QueueId = 1,
                    ErrorMessage = "Test"
                },
                new CancelAllSalesEventModel
                {
                    QueueId = 2,
                    ErrorMessage = "Test"
                }
            };
            data.Events = new List<EventQueueModel>
            {
                new EventQueueModel
                {
                    EventReferenceId = priceBatchDetailId1,
                    EventTypeId = EventTypes.CancelAllSales,
                    Identifier = expectedIdentifier,
                    InProcessBy = null,
                    InsertDate = expectedEventCreatedDate,
                    ItemKey = itemKey1,
                    ProcessFailedDate = null,
                    QueueId = 1,
                    ReprocessCount = null,
                    StoreNo = storeNo
                },
                new EventQueueModel
                {
                    EventReferenceId = priceBatchDetailId2,
                    EventTypeId = EventTypes.CancelAllSales,
                    Identifier = expectedIdentifier2,
                    InProcessBy = null,
                    InsertDate = expectedEventCreatedDate2,
                    ItemKey = itemKey2,
                    ProcessFailedDate = null,
                    QueueId = 2,
                    ReprocessCount = 2,
                    StoreNo = storeNo
                }
            };

            //When
            commandHandler.Execute(data);

            //Then
            var actualEvents = dbProvider.Connection.Query<EventQueueModel>(
                @"
                SELECT QueueID as QueueId,
                    Item_Key as ItemKey,
                    Store_No as StoreNo,
                    Identifier as Identifier,
                    EventTypeID as EventTypeId,
                    EventReferenceID as EventReferenceId,
                    InsertDate,
                    ProcessFailedDate,
                    InProcessBy,
                    ReprocessCount
                FROM mammoth.PriceChangeQueue 
                WHERE (Item_Key = @ItemKey1
                        AND Store_No = @StoreNo)
                    OR (Item_Key = @ItemKey2
                        AND Store_No = @StoreNo)",
                new { ItemKey1 = itemKey1, ItemKey2 = itemKey2, StoreNo = storeNo })
                .ToList();

            Assert.AreEqual(2, actualEvents.Count);
            var actualEvent1 = actualEvents[0];
            var actualEvent2 = actualEvents[1];
            var expectedEvent1 = data.Events.Single(e => e.Identifier == actualEvent1.Identifier);
            var expectedEvent2 = data.Events.Single(e => e.Identifier == actualEvent2.Identifier);

            Assert.AreEqual(expectedEvent1.EventReferenceId, actualEvent1.EventReferenceId);
            Assert.AreEqual(expectedEvent1.EventTypeId, actualEvent1.EventTypeId);
            Assert.AreEqual(expectedEvent1.Identifier, actualEvent1.Identifier);
            Assert.AreEqual(null, actualEvent1.InProcessBy);
            Assert.AreEqual(expectedEvent1.InsertDate, actualEvent1.InsertDate);
            Assert.AreEqual(expectedEvent1.ItemKey, actualEvent1.ItemKey);
            Assert.AreEqual(null, actualEvent1.ProcessFailedDate);
            Assert.AreEqual(1, actualEvent1.ReprocessCount);
            Assert.AreEqual(expectedEvent1.StoreNo, actualEvent1.StoreNo);

            Assert.AreEqual(expectedEvent2.EventReferenceId, actualEvent2.EventReferenceId);
            Assert.AreEqual(expectedEvent2.EventTypeId, actualEvent2.EventTypeId);
            Assert.AreEqual(expectedEvent2.Identifier, actualEvent2.Identifier);
            Assert.AreEqual(null, actualEvent2.InProcessBy);
            Assert.AreEqual(expectedEvent2.InsertDate, actualEvent2.InsertDate);
            Assert.AreEqual(expectedEvent2.ItemKey, actualEvent2.ItemKey);
            Assert.AreEqual(null, actualEvent2.ProcessFailedDate);
            Assert.AreEqual(3, actualEvent2.ReprocessCount);
            Assert.AreEqual(expectedEvent2.StoreNo, actualEvent2.StoreNo);
        }

        private void InsertTestSubTeam(int subTeamNo)
        {
            dbProvider.Connection.Execute(
                "insert into dbo.SubTeam(SubTeam_No) values(@SubTeamNo)",
                new { SubTeamNo = subTeamNo },
                dbProvider.Transaction);
        }

        private void InsertTestStore(int storeNo, int businessUnitId)
        {
            // Insert New Store
            this.dbProvider.Insert(IrmaTestObjectFactory.BuildStore()
                .With(x => x.Store_No, storeNo)
                .With(x => x.BusinessUnit_ID, businessUnitId)
                .With(x => x.StoreJurisdictionID, 1)
                .ToObject());

            // Insert New Store Region Mapping
            this.dbProvider.Insert(IrmaTestObjectFactory.Build<StoreRegionMapping>()
                .With(x => x.Store_No, storeNo)
                .With(x => x.Region_Code, "MA")
                .ToObject());
        }

        private void InsertTestData(string expectedIdentifier, int storeNo, int subTeamNo, DateTime priceBatchDetailStartDate, out int itemKey, out int priceBatchDetailId)
        {
            itemKey = this.dbProvider.Insert(
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

            PriceBatchDetail newPriceBatchDetail = new TestPriceBatchDetailBuilder()
                .WithItem_Key(itemKey)
                .WithIdentifier(expectedIdentifier)
                .WithPrice(1)
                .WithMultiple(1)
                .WithPriceChgTypeID(1)
                .WithStore_No(storeNo)
                .WithStartDate(priceBatchDetailStartDate)
                .WithCancelAllSales(true);
            priceBatchDetailId = this.AddPriceBatchDetail(newPriceBatchDetail);
        }

        private int GetUnitId(string value)
        {
            return dbProvider.GetLookupId<int>("Unit_ID", "ItemUnit", "Unit_Abbreviation", value);
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
