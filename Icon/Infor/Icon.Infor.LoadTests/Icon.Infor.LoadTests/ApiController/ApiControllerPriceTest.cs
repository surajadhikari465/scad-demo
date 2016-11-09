using Icon.Framework;
using Icon.Infor.LoadTests.ApiController;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Infor.LoadTests.ApiControllerTests
{
    public class ApiControllerPriceTest : ApiControllerTestBase
    {
        protected override string TestEmailSubject { get { return "API Controller Price Load Test"; } }
        protected override string ApiControllerName { get { return "API Controller Phase 2 - Price"; } }

        public ApiControllerPriceTest() : base()
        {

        }

        public ApiControllerPriceTest(ILoadTestConfiguration configuration)
            : base(configuration)
        {

        }

        protected override void UpdateCurrentStatus()
        {
            using (IconContext context = new IconContext())
            {
                var status = new LoadTestStatus
                {
                    UnprocessedEntities = context.MessageQueuePrice.Count(
                        q => q.MessageStatusId == MessageStatusTypes.Ready && q.InsertDate >= initialPopulateDataTime),
                    ProcessedEntities = context.MessageQueuePrice.Count(
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

                    INSERT INTO [app].[MessageQueuePrice]
                       ([MessageTypeId]
                       ,[MessageStatusId]
                       ,[MessageHistoryId]
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
                       ,[UomCode]
                       ,[UomName]
                       ,[CurrencyCode]
                       ,[Price]
                       ,[Multiple]
                       ,[SalePrice]
                       ,[SaleMultiple]
                       ,[SaleStartDate]
                       ,[SaleEndDate]
                       ,[PreviousSalePrice]
                       ,[PreviousSaleMultiple]
                       ,[PreviousSaleStartDate]
                       ,[PreviousSaleEndDate]
                       ,[InProcessBy]
                       ,[ProcessedDate])
                    select 
                        4 MessageTypeId,
                        1 MessageStatusId,
                        null MessageHistoryId,
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
                        'RegularPriceChange' ChangeType,
                        'EA' UomCode,
                        'EACH' UomName,
                        'USD' CurrencyCode,
                        1.99 Price,
                        1 Multiple,
                        NULL SalePrice,
                        NULL SaleMultiple,
                        NULL SaleStartDate,
                        NULL SaleEndDate,
                        NULL PreviousSalePrice,
                        NULL PreviousSaleMultiple,
                        NULL PreviousSaleStartDate,
                        NULL PreviousSaleEndDate,
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
