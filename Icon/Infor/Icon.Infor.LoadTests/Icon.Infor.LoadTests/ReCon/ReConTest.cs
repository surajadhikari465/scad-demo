using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irma.Framework;
using System.Configuration;
using System.Data.SqlClient;
using System.ServiceProcess;
using Icon.Common.Email;
using System.Diagnostics;

namespace Icon.Infor.LoadTests.ReCon
{
    public class ReConItemLoadTest : LoadTestBase
    {
        #region Fields

        private const string ReConLoadTestSubject = "ReCon Item Load Test Results";
        private string[] RegionsToProcess = ConfigurationManager.AppSettings["NewItemsEnabledRegionsList"].Split(',');
        private int numberOfEntitiesAdded;

        #endregion

        #region Properties

        protected override string TestEmailSubject { get { return ReConLoadTestSubject; } }

        #endregion

        #region Ctors

        public ReConItemLoadTest(ILoadTestConfiguration Configuration)
            : base(Configuration)
        {
        }

        public ReConItemLoadTest() : base()
        {

        }

        #endregion

        #region Public Methods

        public override void Run()
        {
            numberOfEntitiesAdded = 0;

            foreach (string irmaRegion in RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(connectionString);

                if (!String.IsNullOrEmpty(connectionString))
                {
                    db.Database.ExecuteSqlCommand(
                        @"delete dbo.IconItemChangeQueue");
                }
            }

            base.Run();
        }

        public override void Stop()
        {
            foreach (var reConGroup in Configuration.ApplicationInstances.GroupBy(i => i.Server))
            {
                var services = ServiceController.GetServices(reConGroup.Key)
                .Where(s => s.ServiceName == "InforNewItemEventService");

                foreach (var service in services)
                {
                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        service.Stop();
                        service.WaitForStatus(ServiceControllerStatus.Stopped);
                    }
                }
            }

            foreach (string irmaRegion in RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(connectionString);

                if (!String.IsNullOrEmpty(connectionString))
                {
                    db.Database.ExecuteSqlCommand(
                        @"delete dbo.IconItemChangeQueue");
                }
            }
        }

        public override bool IsAbleToRun()
        {
            return true;
        }

        #endregion

        #region Protected Methods

        protected override void UpdateCurrentStatus()
        {
            LoadTestStatus status = new LoadTestStatus();

            foreach (string irmaRegion in RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(connectionString);

                if (!String.IsNullOrEmpty(connectionString))
                {
                    status.UnprocessedEntities += db.Database.SqlQuery<int>("select count(*) from dbo.IconItemChangeQueue where ProcessFailedDate is null").FirstOrDefault();
                    status.FailedEntities += db.Database.SqlQuery<int>("select count(*) from dbo.IconItemChangeQueue q where q.ProcessFailedDate is not null").FirstOrDefault();
                    status.ProcessedEntities = numberOfEntitiesAdded - status.UnprocessedEntities;
                }
            }

            base.UpdateStatusBasedOnElapsedTime(status);

            currentStatus = status;
        }

        protected override void Setup()
        {
            foreach (var reConGroup in Configuration.ApplicationInstances.GroupBy(i => i.Server))
            {
                var services = ServiceController.GetServices(reConGroup.Key)
                    .Where(s => s.ServiceName == "InforNewItemEventService");

                foreach (var service in services)
                {
                    if (service.Status != ServiceControllerStatus.Running)
                    {
                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running);
                    }
                }
            }
        }

        protected override void PopulateTestData()
        {
            foreach (string irmaRegion in RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(connectionString);

                if (!String.IsNullOrEmpty(connectionString))
                {
                    db.Database.ExecuteSqlCommand(
                        @"                            
                        INSERT INTO [dbo].[IconItemChangeQueue]
                                    ([Item_Key]
                                    ,[Identifier]
                                    ,[ItemChgTypeID]
                                    ,[InsertDate]
                                    ,[ProcessFailedDate]
                                    ,[InProcessBy])
                        SELECT  TOP (@eventCount) 
                                i.Item_Key,
		                        ii.Identifier,
		                        1,
		                        getDate(),
		                        null,
		                        null
                        FROM Item i 
                        JOIN ItemIdentifier ii on i.Item_Key = ii.Item_Key
                        JOIN ValidatedScanCode vsc on ii.Identifier = vsc.ScanCode
                        WHERE 
                            i.deleted_item = 0 
                            AND i.remove_item = 0
	                        AND ii.deleted_identifier = 0 
                            AND remove_identifier = 0
	                        AND i.Retail_Sale = 1",
                            new SqlParameter("@eventCount", Configuration.EntityCount));

                    numberOfEntitiesAdded += Configuration.EntityCount;
                }
            }
        }

        #endregion
    }
}
