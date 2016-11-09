using Icon.Framework;
using Icon.Infor.LoadTests.LoadTestSteps;
using Microsoft.Win32.TaskScheduler;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Infor.LoadTests.GloCon
{
    public class GloConItemTest : LoadTestBase
    {
        private const string GloConLoadTestSubject = "GloCon Item Load Test Results";

        private int eventsQueued;

        protected override string TestEmailSubject { get { return GloConLoadTestSubject; } }

        public GloConItemTest(ILoadTestConfiguration configuration)
            : base(configuration)
        {
        }

        public GloConItemTest() : base()
        {

        }

        public override bool IsAbleToRun()
        {
            foreach (var gloConGroup in Configuration.ApplicationInstances.GroupBy(i => i.Server))
            {
                using (TaskService ts = new TaskService(gloConGroup.Key))
                {
                    var gloConTask = ts.FindTask("Global Controller");

                    if (!gloConTask.Enabled)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override void Run()
        {
            using (IconContext context = new IconContext())
            {
                context.Database.ExecuteSqlCommand(@"DELETE [app].[EventQueue];");
            }

            base.Run();
        }

        protected override void UpdateCurrentStatus()
        {
            var status = new LoadTestStatus();

            using (IconContext context = new IconContext())
            {
                status.UnprocessedEntities = context.Database.SqlQuery<int>("select count(*) from app.EventQueue where ProcessFailedDate is null and EventId = 2").FirstOrDefault();
                status.FailedEntities = context.Database.SqlQuery<int>("select count(*) from app.EventQueue q where q.ProcessFailedDate is not null and EventId = 2").FirstOrDefault();
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
                    SELECT TOP (@eventCount) 
	                    2,
	                    sc.scanCode,
	                    sc.itemID,
	                    iis.regioncode,
	                    GETDATE(),
	                    NULL,
	                    NULL
                    FROM ScanCode sc
                    JOIN app.IRMAItemSubscription iis ON sc.scanCode = iis.identifier",
                    new SqlParameter("@eventCount", Configuration.EntityCount));
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
