using Icon.Framework;
using Icon.Infor.LoadTests.ApiController;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Infor.LoadTests.ApiControllerTests
{
    public class ApiControllerProductTest : ApiControllerTestBase
    {
        protected override string TestEmailSubject { get { return "API Controller Product Load Test"; } }
        protected override string ApiControllerName { get { return "API Controller Phase 2 - Product"; } }

        #region Ctors

        public ApiControllerProductTest() : base() { }

        public ApiControllerProductTest(ILoadTestConfiguration configuration)
            : base(configuration)
        {
        }

        #endregion

        protected override void UpdateCurrentStatus()
        {
            using (IconContext context = new IconContext())
            {
                var status = new LoadTestStatus
                {
                    UnprocessedEntities = context.MessageQueueProduct.Count(
                        q => q.MessageStatusId == MessageStatusTypes.Ready && q.InsertDate >= initialPopulateDataTime),
                    ProcessedEntities = context.MessageQueueProduct.Count(
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
                    @"declare @itemIds app.UpdatedItemIDsType
                    insert into @itemIds
                    select top(@entityCount) 
                        i.itemID
                    from dbo.Item i
                    join ItemTrait val on i.itemID = val.itemID
                    join Trait t on val.traitID = t.traitID 
                        and t.traitCode = 'VAL'

                    exec infor.GenerateItemUpdateMessages @itemIds",
                    new SqlParameter("@entityCount", Configuration.EntityCount));
            }
        }
    }
}
