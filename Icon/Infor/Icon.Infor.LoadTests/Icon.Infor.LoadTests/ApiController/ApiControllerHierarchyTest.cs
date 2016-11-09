using Icon.Framework;
using Icon.Infor.LoadTests.ApiController;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Infor.LoadTests.ApiControllerTests
{
    public class ApiControllerHierarchyTest : ApiControllerTestBase
    {
        protected override string ApiControllerName
        {
            get
            {
                return "API Controller Phase 2 - Hierarchy";
            }
        }
        protected override string TestEmailSubject { get { return "API Controller Hierarchy Load Test"; } }

        public ApiControllerHierarchyTest(ILoadTestConfiguration configuration)
            : base(configuration)
        {
        }

        public ApiControllerHierarchyTest() : base()
        {

        }

        protected override void UpdateCurrentStatus()
        {
            using (IconContext context = new IconContext())
            {
                var status = new LoadTestStatus
                {
                    UnprocessedEntities = context.MessageQueueHierarchy.Count(
                        q => q.MessageStatusId == MessageStatusTypes.Ready && q.InsertDate >= initialPopulateDataTime),
                    ProcessedEntities = context.MessageQueueHierarchy.Count(
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
                    @"INSERT INTO [app].[MessageQueueHierarchy]
                       ([MessageTypeId]
                       ,[MessageStatusId]
                       ,[MessageHistoryId]
                       ,[MessageActionId]
                       ,[HierarchyId]
                       ,[HierarchyName]
                       ,[HierarchyLevelName]
                       ,[ItemsAttached]
                       ,[HierarchyClassId]
                       ,[HierarchyClassName]
                       ,[HierarchyLevel]
                       ,[HierarchyParentClassId]
                       ,[InProcessBy]
                       ,[ProcessedDate])
                    select top(@entityCount) 
                        2,
                        1,
                        null,
                        1,
                        2,
                        'Brands',
                        'Brand',
                        1,
                        hc.hierarchyClassID,
                        hc.hierarchyClassName,
                        hc.hierarchyLevel,
                        hc.HierarchyParentClassID,
                        null,
                        null
                    from HierarchyClass hc
                    where hc.hierarchyID = 2",
                    new SqlParameter("@entityCount", Configuration.EntityCount));
            }
        }
    }
}
