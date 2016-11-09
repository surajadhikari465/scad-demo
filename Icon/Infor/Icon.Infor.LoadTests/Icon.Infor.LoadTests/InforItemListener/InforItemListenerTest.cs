using Icon.Esb;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Infor.LoadTests.LoadTestSteps;
using Icon.Infor.MessageGenerator;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Infor.LoadTests.InforItemListener
{
    public class InforItemListenerTest : LoadTestBase
    {
        #region Fields

        private const string InforItemListenerLoadTestSubject = "Infor Item Listener Load Test Results";
        private DateTime startTime;
        private EsbConnectionSettings esbConnectionSettings;

        #endregion

        #region Properties

        protected override string TestEmailSubject { get { return InforItemListenerLoadTestSubject; } }

        #endregion

        #region Ctors

        public InforItemListenerTest(ILoadTestConfiguration configuration)
            : base(configuration)
        {
            esbConnectionSettings = EsbConnectionSettings.CreateSettingsFromConfig();
        }

        public InforItemListenerTest() : base()
        {
            esbConnectionSettings = EsbConnectionSettings.CreateSettingsFromConfig();
        }

        #endregion Ctors

        #region Public Methods

        public override void Run()
        {
            startTime = DateTime.Now;
            base.Run();
        }

        protected override void UpdateCurrentStatus()
        {
            var status = new LoadTestStatus();

            using (IconContext context = new IconContext())
            {
                status.ProcessedEntities = context.Database.SqlQuery<int>(
                    @"select count(*)
                    from app.AppLog al 
                    where al.AppID = 17 
                        and al.InsertDate > @testStartTime
                        and al.Message like 'Parsed message from queue%'",
                    new SqlParameter("@testStartTime", startTime))
                    .First();
                status.FailedEntities = context.Database.SqlQuery<int>(
                    @"select count(*)
                    from app.AppLog al 
                    where al.AppID = 17 
                        and al.InsertDate > @testStartTime
                        and al.Level = 'Error'",
                    new SqlParameter("@testStartTime", startTime))
                    .First();
            }

            base.UpdateStatusBasedOnElapsedTime(status);

            currentStatus = status;
        }

        public override void Stop()
        {
            foreach (var listener in Configuration.ApplicationInstances)
            {
                new StopServiceStep("InforLoadTestItemListener", listener.Server).Execute();
            }
        }

        public override bool IsAbleToRun()
        {
            return true;
        }

        #endregion

        #region Protected Methods

        protected override void PopulateTestData()
        {
            using (EsbProducer producer = new EsbProducer(esbConnectionSettings))
            {
                producer.OpenConnection();
                using (IconContext context = new IconContext())
                {
                    int batchSize = 100;
                    for (int itemsSent = 0; itemsSent < Configuration.EntityCount; itemsSent += batchSize)
                    {
                        int itemsToSend = Configuration.EntityCount - itemsSent < batchSize ? Configuration.EntityCount - itemsSent : batchSize;

                        var items = context.Item
                            .Include(i => i.ItemTrait)
                            .Include(i => i.ItemHierarchyClass)
                            .Take(itemsToSend)
                            .ToList();

                        var message = new ItemMessageGenerator(context)
                            .CreateItemMessage(items);

                        producer.Send(message);
                    }
                }
            }
        }

        protected override void Setup()
        {
            foreach (var listener in Configuration.ApplicationInstances)
            {
                new StopServiceStep("InforLoadTestItemListener", listener.Server).Execute();
            }

            foreach (var listener in Configuration.ApplicationInstances)
            {
                new StartServiceStep("InforLoadTestItemListener", listener.Server).Execute();
            }
        }

        #endregion
    }
}
