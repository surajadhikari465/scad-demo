using Icon.Common.Email;
using Icon.Framework;
using Icon.Infor.LoadTests.LoadTestSteps;
using Microsoft.Win32.TaskScheduler;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Infor.LoadTests.GloCon
{
    public class GloConBrandTest : LoadTestBase
    {
        private const string GloConUnableToRunTestMessage = "GloCon is not enabled. Cannot run the test.";

        private int eventsQueued;

        protected override string TestEmailSubject { get { return "GloCon Brand Load Test Results"; } }

        public GloConBrandTest(ILoadTestConfiguration configuration)
            : base(configuration)
        {
        }

        public GloConBrandTest() : base()
        {

        }

        public override bool IsAbleToRun()
        {
            //foreach (var gloConGroup in Configuration.ApplicationInstances.GroupBy(i => i.Server))
            //{
            //    using (TaskService ts = new TaskService(gloConGroup.Key))
            //    {
            //        var gloConTask = ts.FindTask("Global Controller");

            //        if (!gloConTask.Enabled)
            //        {
            //            return false;
            //        }
            //    }
            //}

            return true;
        }

        protected override void UpdateCurrentStatus()
        {
            var status = new LoadTestStatus();

            using (IconContext context = new IconContext())
            {
                status.UnprocessedEntities = context.Database.SqlQuery<int>("select count(*) from app.EventQueue where ProcessFailedDate is null and EventId = 4").FirstOrDefault();
                status.FailedEntities = context.Database.SqlQuery<int>("select count(*) from app.EventQueue q where q.ProcessFailedDate is not null and EventId = 4").FirstOrDefault();
                status.ProcessedEntities = eventsQueued - status.UnprocessedEntities;
            }

            base.UpdateStatusBasedOnElapsedTime(status);

            currentStatus = status;
        }

        public override void Stop()
        {
            foreach (var gloConGroup in Configuration.ApplicationInstances.GroupBy(i => i.Server))
            {
                using (TaskService ts = new TaskService(gloConGroup.Key))
                {
                    var gloConTask = ts.FindTask("Global Controller");
                    gloConTask.Stop();
                }
            }

            using (IconContext context = new IconContext())
            {
                context.Database.ExecuteSqlCommand(
                    @"update app.EventQueue
                      set ProcessFailedDate = getdate()");
            }
        }

        protected override void PopulateTestData()
        {
            eventsQueued += Configuration.EntityCount;

            string brandUpdateConfiguredRegions = ConfigurationManager.AppSettings["BrandNameUpdateEventConfiguredRegions"];

            using (IconContext context = new IconContext())
            {
                context.Database.ExecuteSqlCommand(
                    @"INSERT INTO [app].[EventQueue]
                                ([EventId]
                                ,[EventMessage]
                                ,[EventReferenceId]
                                ,[RegionCode]
                                ,[InsertDate]
                                ,[ProcessFailedDate]
                                ,[InProcessBy])
                    SELECT 4, hierarchyClassName, hierarchyClassID, r.Key_Value, getdate(), null, null
                      FROM (select top (@eventCount) * from HierarchyClass where hierarchyID = 2) hc
                        cross join [dbo].[fn_ParseStringList](@regions, ',')  r",
                    new SqlParameter("@eventCount", Configuration.EntityCount),
                    new SqlParameter("@regions", brandUpdateConfiguredRegions));
            }
        }

        protected override void Setup()
        {
            foreach (var listener in Configuration.ApplicationInstances)
            {
                new StopServiceStep("GlobalEventControllerService", listener.Server).Execute();
            }

            foreach (var listener in Configuration.ApplicationInstances)
            {
                new StartServiceStep("GlobalEventControllerService", listener.Server).Execute();
            }
        }
    }
}

