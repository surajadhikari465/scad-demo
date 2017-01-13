namespace Icon.Monitoring.Monitors
{
    using Common;
    using Common.Dvo;
    using Common.IO;
    using Common.PagerDuty;
    using Common.Settings;
    using DataAccess.Commands;
    using DataAccess.Queries;
    using Icon.Common.DataAccess;
    using Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class DvoBulkImportJobMonitor : TimedControllerMonitor
    {
        private const string DvoPagerDutyMessage = "DVO Bulk Order Import Job stopped running.";
        private const string DvoFileDoesNotExistError = "DVO Bulk Order Import Job parameter file does not exist.";

        private IDvoBulkImportJobMonitorSettings dvoSettings;
        private IFileInfoAccessor fileInfoAccessor;
        private IPagerDutyTrigger pagerDutyTrigger;
        private IQueryHandler<GetDvoJobStatusParameters, List<DvoRegionalJobStatus>> getDvoRegionalJobStatusQueryHandler;
        private ICommandHandler<DeleteDvoErrorStatusCommand> deleteDvoErrorStatusCommandHandler;
        private ICommandHandler<AddDvoErrorStatusCommand> addDvoErrorStatusCommandHandler;

        public DvoBulkImportJobMonitor(
            IMonitorSettings settings,
            IDvoBulkImportJobMonitorSettings dvoSettings,
            IFileInfoAccessor fileInfoAccessor,
            IPagerDutyTrigger pagerDutyTrigger,
            IQueryHandler<GetDvoJobStatusParameters, List<DvoRegionalJobStatus>> getDvoRegionalJobStatusQueryHandler,
            ICommandHandler<DeleteDvoErrorStatusCommand> deleteDvoErrorStatusCommandHandler,
            ICommandHandler<AddDvoErrorStatusCommand> addDvoErrorStatusCommandHandler,
            ILogger logger)
        {
            this.settings = settings; 
            this.dvoSettings = dvoSettings;
            this.fileInfoAccessor = fileInfoAccessor;
            this.pagerDutyTrigger = pagerDutyTrigger;
            this.getDvoRegionalJobStatusQueryHandler = getDvoRegionalJobStatusQueryHandler;
            this.deleteDvoErrorStatusCommandHandler = deleteDvoErrorStatusCommandHandler;
            this.addDvoErrorStatusCommandHandler = addDvoErrorStatusCommandHandler;
            this.logger = logger;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            if (ShouldCheckStatusAndNotify())
            {
                var dvoDirectory = dvoSettings.DvoDirectoryPath;
                var dvoBulkImportFileMaxMinuteThreshold = dvoSettings.DvoBulkImportFileMaxMinuteThreshold;
                var regions = dvoSettings.DvoBulkImportJobMonitorRegions;

                List<DvoRegionalJobStatus> errorStatuses = new List<DvoRegionalJobStatus>();

                foreach (var region in regions)
                {
                    IFileInfo fileInfo = GetFileInfo(dvoDirectory, region);

                    if (!fileInfo.Exists)
                    {
                        errorStatuses.Add(new DvoRegionalJobStatus
                        {
                            Error = DvoFileDoesNotExistError,
                            FileInfo = fileInfo,
                            Region = region
                        });
                    }
                    else if (IsDvoFileOlderThanMaxMinuteThreshold(fileInfo, dvoBulkImportFileMaxMinuteThreshold))
                    {
                        errorStatuses.Add(new DvoRegionalJobStatus
                        {
                            Error = $"DVO file has not been processed within the past {dvoBulkImportFileMaxMinuteThreshold} minutes.",
                            FileInfo = fileInfo,
                            Region = region
                        });
                    }
                    else
                    {
                        //Delete any existing error statuses from possible previous failures
                        deleteDvoErrorStatusCommandHandler.Execute(new DeleteDvoErrorStatusCommand { Region = region });
                    }
                }

                ProcessErrorJobStatuses(errorStatuses);
            }
        }

        private bool ShouldCheckStatusAndNotify()
        {
            return dvoSettings.EnableDvoBulkImportJobMonitor
                && !(DateTime.Now.Between(dvoSettings.DvoBulkImportJobMonitorBlackoutStart, dvoSettings.DvoBulkImportJobMonitorBlackoutEnd));
        }

        private IFileInfo GetFileInfo(string dvoDirectory, string region)
        {
            return fileInfoAccessor.GetFileInfo(Path.Combine(dvoDirectory, $"{region}_DVO_Orders_bulk_load.prm"));
        }

        private bool IsDvoFileOlderThanMaxMinuteThreshold(IFileInfo fileInfo, int dvoBulkImportFileMaxMinuteThreshold)
        {
            return fileInfo.LastWriteTime < DateTime.Now.Subtract(TimeSpan.FromMinutes(dvoBulkImportFileMaxMinuteThreshold));
        }

        private void ProcessErrorJobStatuses(List<DvoRegionalJobStatus> errorStatuses)
        {
            var currentErrorStatuses = getDvoRegionalJobStatusQueryHandler.Search(new GetDvoJobStatusParameters { Regions = errorStatuses.Select(e => e.Region).ToList() });
            var pagerDutyStatuses = errorStatuses.Where(s => !currentErrorStatuses.Any(c => c.Region == s.Region)).ToList();

            //Only send pagerduty alerts for regions that don't have a current error
            if (pagerDutyStatuses.Any())
            {
                logger.Warn(JsonConvert.SerializeObject(new
                {
                    Message = "Triggering PagerDuty for DVO Bulk Import Job",
                    Regions = pagerDutyStatuses.Select(s => new { s.Region, s.Error })
                }));

                pagerDutyTrigger.TriggerIncident(
                    DvoPagerDutyMessage,
                    pagerDutyStatuses.ToDictionary(
                        s => s.Region,
                        s => $"{s.Region} - {s.FileInfo.Name} - {s.Error}"));

                foreach (var status in pagerDutyStatuses)
                {
                    //Add any new error statuses to the database so that other monitor instances don't send duplicate pager duty messages
                    addDvoErrorStatusCommandHandler.Execute(new AddDvoErrorStatusCommand { Region = status.Region });
                }

            }
        }
    }
}
