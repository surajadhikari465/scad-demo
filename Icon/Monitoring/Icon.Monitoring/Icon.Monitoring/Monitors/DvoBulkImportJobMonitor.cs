﻿namespace Icon.Monitoring.Monitors
{
    using Common;
    using Common.Dvo;
    using Common.IO;
    using Common.Opsgenie;
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
        private const string DvoOpsgenieMessage = "DVO Bulk Order Import Job stopped running.";
        private const string DvoFileDoesNotExistError = "DVO Bulk Order Import Job parameter file does not exist.";

        private IDvoBulkImportJobMonitorSettings dvoSettings;
        private IFileInfoAccessor fileInfoAccessor;
        private IOpsgenieTrigger opsgenieTrigger;
        private IQueryHandler<GetDvoJobStatusParameters, List<DvoRegionalJobStatus>> getDvoRegionalJobStatusQueryHandler;
        private ICommandHandler<DeleteDvoErrorStatusCommand> deleteDvoErrorStatusCommandHandler;
        private ICommandHandler<AddDvoErrorStatusCommand> addDvoErrorStatusCommandHandler;

        public DvoBulkImportJobMonitor(
            IMonitorSettings settings,
            IDvoBulkImportJobMonitorSettings dvoSettings,
            IFileInfoAccessor fileInfoAccessor,
            IOpsgenieTrigger opsgenieTrigger,
            IQueryHandler<GetDvoJobStatusParameters, List<DvoRegionalJobStatus>> getDvoRegionalJobStatusQueryHandler,
            ICommandHandler<DeleteDvoErrorStatusCommand> deleteDvoErrorStatusCommandHandler,
            ICommandHandler<AddDvoErrorStatusCommand> addDvoErrorStatusCommandHandler,
            ILogger logger)
        {
            this.settings = settings;
            this.dvoSettings = dvoSettings;
            this.fileInfoAccessor = fileInfoAccessor;
            this.opsgenieTrigger = opsgenieTrigger;
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
            DateTime lastWriteTime = fileInfo.LastWriteTime;
            DateTime currentDateTime = DateTime.Now;
            if (lastWriteTime < currentDateTime.Subtract(TimeSpan.FromMinutes(dvoBulkImportFileMaxMinuteThreshold)))
            {
                
                logger.Info($"DVO file '{fileInfo.Name}' Last Write was {lastWriteTime:G}, Current Time is {currentDateTime:G} and Max Minute Threshold Setting is {dvoBulkImportFileMaxMinuteThreshold:G}.");
                return true;
            }
                return false;
        }

        private void ProcessErrorJobStatuses(List<DvoRegionalJobStatus> errorStatuses)
        {
            var currentErrorStatuses = getDvoRegionalJobStatusQueryHandler.Search(new GetDvoJobStatusParameters { Regions = errorStatuses.Select(e => e.Region).ToList() });
            var opsgenieStatuses = errorStatuses.Where(s => !currentErrorStatuses.Any(c => c.Region == s.Region)).ToList();

            //Only send alerts for regions that don't have a current error
            if (opsgenieStatuses.Any())
            {
                logger.Warn(JsonConvert.SerializeObject(new
                {
                    Message = "Triggering Opsgenie Alert for DVO Bulk Import Job",
                    Regions = opsgenieStatuses.Select(s => new { s.Region, s.Error })
                }));

                this.opsgenieTrigger.TriggerAlert("DVO Bulk Order Job Issue",
                    DvoOpsgenieMessage,
                    opsgenieStatuses.ToDictionary(
                        s => s.Region,
                        s => $"{s.Region} - {s.FileInfo.Name} - {s.Error}"));

                foreach (var status in opsgenieStatuses)
                {
                    //Add any new error statuses to the database so that other monitor instances don't send duplicate pager duty messages
                    addDvoErrorStatusCommandHandler.Execute(new AddDvoErrorStatusCommand { Region = status.Region });
                }

            }
        }
    }
}
