using Icon.Framework;
using Icon.Infor.LoadTests.ApiController;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Infor.LoadTests.ApiControllerTests
{
    public class ApiControllerItemLocaleTest : ApiControllerTestBase
    {
        protected override string ApiControllerName
        {
            get
            {
                return "API Controller Phase 2 - Item Locale";
            }
        }
        protected override string TestEmailSubject { get { return "API Controller Item Locale Load Test"; } }

        public ApiControllerItemLocaleTest() : base()
        {

        }

        public ApiControllerItemLocaleTest(ILoadTestConfiguration configuration)
            : base(configuration)
        {

        }

        protected override void UpdateCurrentStatus()
        {
            using (IconContext context = new IconContext())
            {
                var status = new LoadTestStatus
                {
                    UnprocessedEntities = context.MessageQueueItemLocale.Count(
                        q => q.MessageStatusId == MessageStatusTypes.Ready && q.InsertDate >= initialPopulateDataTime),
                    ProcessedEntities = context.MessageQueueItemLocale.Count(
                        q => q.MessageStatusId == MessageStatusTypes.Associated && q.InsertDate >= initialPopulateDataTime),
                };

                base.UpdateStatusBasedOnElapsedTime(status);

                currentStatus = status;
            }
        }

        protected override void PopulateTestData()
        {
            using (IconContext context = new IconContext())
            {
                context.Database.ExecuteSqlCommand(
                    @"declare @irmaPushID int = (select top 1 IRMAPushID from app.IRMAPush),
                              @itemIds app.UpdatedItemIDsType
                    insert into @itemIds
                    select top(@entityCount) 
                        i.itemID
                    from dbo.Item i
                    join ItemTrait val on i.itemID = val.itemID
                    join Trait t on val.traitID = t.traitID 
                        and t.traitCode = 'VAL'

                    INSERT INTO [app].[MessageQueueItemLocale]
                       ([MessageTypeId]
                       ,[MessageStatusId]
                       ,[MessageHistoryId]
                       ,[MessageActionId]
                       ,[IRMAPushID]
                       ,[InsertDate]
                       ,[RegionCode]
                       ,[BusinessUnit_ID]
                       ,[ItemId]
                       ,[ItemTypeCode]
                       ,[ItemTypeDesc]
                       ,[LocaleId]
                       ,[LocaleName]
                       ,[ScanCodeId]
                       ,[ScanCode]
                       ,[ScanCodeTypeId]
                       ,[ScanCodeTypeDesc]
                       ,[ChangeType]
                       ,[LockedForSale]
                       ,[Recall]
                       ,[TMDiscountEligible]
                       ,[Case_Discount]
                       ,[AgeCode]
                       ,[Restricted_Hours]
                       ,[Sold_By_Weight]
                       ,[ScaleForcedTare]
                       ,[Quantity_Required]
                       ,[Price_Required]
                       ,[QtyProhibit]
                       ,[VisualVerify]
                       ,[LinkedItemScanCode]
                       ,[PreviousLinkedItemScanCode]
                       ,[PosScaleTare]
                       ,[InProcessBy]
                       ,[ProcessedDate])
                    select 
                        3 MessageTypeId,
                        1 MessageStatusId,
                        null MessageHistoryId,
                        1 MessageActionId,
                        @irmaPushID IRMAPushID,
                        GETDATE() InsertDate,
                        'FL' RegionCode,
                        10130 BusinessUnit_ID,
                        i.itemID ItemId,
                        'RET' ItemTypeCode,
                        'Retail' ItemTypeDesc,
                        1 LocaleId,
                        'TEST' LocaleName,
                        sc.scanCodeID ScanCodeId,
                        sc.scanCode ScanCode,
                        sc.scanCodeTypeID ScanCodeTypeId,
                        sct.scanCodeTypeDesc ScanCodeTypeDesc,
                        'ItemLocaleAttributeChange' ChangeType,
                        0 LockedForSale,
                        0 Recall,
                        0 TMDiscountEligible,
                        0 Case_Discount,
                        0 AgeCode,
                        0 Restricted_Hours,
                        0 Sold_By_Weight,
                        0 ScaleForcedTare,
                        0 Quantity_Required,
                        0 Price_Required,
                        0 QtyProhibit,
                        0 VisualVerify,
                        NULL LinkedItemScanCode,
                        NULL PreviousLinkedItemScanCode,
                        NULL PosScaleTare,
                        NULL InProcessBy,
                        NULL ProcessedDate
                    from @itemIDs i
                    join ScanCode sc on i.itemID = sc.itemID
                    join ScanCodeType sct on sc.scanCodeTypeID = sct.scanCodeTypeID",
                    new SqlParameter("@entityCount", Configuration.EntityCount));
            }
        }
    }
}
