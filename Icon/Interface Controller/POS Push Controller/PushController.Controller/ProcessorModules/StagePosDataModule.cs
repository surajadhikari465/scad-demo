using Icon.RenewableContext;
using Icon.Logging;
using InterfaceController.Common;
using Irma.Framework;
using Irma.Framework.RenewableContext;
using PushController.Common;
using PushController.Common.Models;
using PushController.Controller.PosDataGenerators;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace PushController.Controller.ProcessorModules
{
    public class StagePosDataModule : IIrmaPosDataProcessingModule
    {
        private ILogger<StagePosDataModule> logger;
        private IIrmaContextProvider contextProvider;
        private IRenewableContext<IrmaContext> globalContext;
        private IQueryHandler<GetJobStatusQuery, JobStatus> getJobStatusQueryHandler;
        private IQueryHandler<GetIrmaPosDataQuery, List<IConPOSPushPublish>> getIrmaPosDataQueryHandler;
        private ICommandHandler<MarkPublishedRecordsAsInProcessCommand> markPublishedRecordsAsInProcessCommandHandler;
        private ICommandHandler<UpdatePublishTableDatesCommand> updatePublishTableDatesCommandHandler;
        private IPosDataGenerator<IrmaPushModel> irmaPushDataGenerator;

        public string CurrentRegion { get; private set; }

        public StagePosDataModule(
            ILogger<StagePosDataModule> logger,
            IIrmaContextProvider contextProvider,
            IQueryHandler<GetJobStatusQuery, JobStatus> getJobStatusQueryHandler,
            IQueryHandler<GetIrmaPosDataQuery, List<IConPOSPushPublish>> getIrmaPosDataQueryHandler,
            ICommandHandler<MarkPublishedRecordsAsInProcessCommand> markPublishedRecordsAsInProcessCommandHandler,
            ICommandHandler<UpdatePublishTableDatesCommand> updatePublishTableDatesCommandHandler,
            IPosDataGenerator<IrmaPushModel> irmaPushDataGenerator)
        {
            this.logger = logger;
            this.contextProvider = contextProvider;
            this.getJobStatusQueryHandler = getJobStatusQueryHandler;
            this.getIrmaPosDataQueryHandler = getIrmaPosDataQueryHandler;
            this.markPublishedRecordsAsInProcessCommandHandler = markPublishedRecordsAsInProcessCommandHandler;
            this.updatePublishTableDatesCommandHandler = updatePublishTableDatesCommandHandler;
            this.irmaPushDataGenerator = irmaPushDataGenerator;
        }

        public void Execute()
        {
            string regionalConnectionString;

            foreach (string region in StartupOptions.RegionsToProcess)
            {
                CurrentRegion = region;

                regionalConnectionString = ConnectionBuilder.GetConnection(region);
                globalContext = new GlobalIrmaContext(contextProvider.GetRegionalContext(regionalConnectionString), regionalConnectionString);

                if (!PosPushIsRunning(region))
                {
                    MarkPosDataAsInProcess();
                    var publishedPosData = GetPublishedPosData();

                    while (publishedPosData.Count > 0)
                    {
                        logger.Info(String.Format("Found {0} records to be processed for the {1} region.", publishedPosData.Count.ToString(), region));

                        var posDataReadyToBeStaged = ConvertIrmaPosData(publishedPosData);

                        if (posDataReadyToBeStaged.Count > 0)
                        {
                            StagePosData(posDataReadyToBeStaged);
                            UpdatePosDataProcessedDate(publishedPosData);
                        }

                        globalContext.Refresh();
                        MarkPosDataAsInProcess();
                        publishedPosData = GetPublishedPosData();
                    }
                }
            }
        }

        private void UpdatePosDataProcessedDate(List<IConPOSPushPublish> publishedPosData)
        {
            var command = new UpdatePublishTableDatesCommand
            {
                Context = globalContext.Context,
                ProcessedSuccessfully = true,
                PublishedPosData = publishedPosData,
                Date = DateTime.Now
            };

            updatePublishTableDatesCommandHandler.Execute(command);
        }

        private void StagePosData(List<IrmaPushModel> posDataReadyToBeStaged)
        {
            irmaPushDataGenerator.StagePosData(posDataReadyToBeStaged);
        }

        private List<IrmaPushModel> ConvertIrmaPosData(List<IConPOSPushPublish> publishedPosData)
        {
            return irmaPushDataGenerator.ConvertPosData(publishedPosData);
        }

        private List<IConPOSPushPublish> GetPublishedPosData()
        {
            var query = new GetIrmaPosDataQuery
            {
                Context = globalContext.Context,
                Instance = StartupOptions.Instance
            };

            return getIrmaPosDataQueryHandler.Execute(query);
        }

        private void MarkPosDataAsInProcess()
        {
            var command = new MarkPublishedRecordsAsInProcessCommand
            {
                Context = globalContext.Context,
                Instance = StartupOptions.Instance,
                MaxRecordsToProcess = StartupOptions.MaxRecordsToProcess
            };

            markPublishedRecordsAsInProcessCommandHandler.Execute(command);
        }

        private bool PosPushIsRunning(string region)
        {
            double maxWaitTime;
            if (!Double.TryParse(ConfigurationManager.AppSettings["MaxWaitTime"], out maxWaitTime))
            {
                maxWaitTime = 60d;
            }

            var jobStatusQuery = new GetJobStatusQuery
            {
                Context = globalContext.Context,
                JobName = "POSPushJob"
            };

            var posPushJobStatus = getJobStatusQueryHandler.Execute(jobStatusQuery);

            bool jobIsRunning;

            if (posPushJobStatus.Status == "RUNNING" && posPushJobStatus.StatusDescription.Contains("IconPOSPushPublish"))
            {
                jobIsRunning = true;

                var pusheenTime = DateTime.Now.ToUniversalTime();
                var posPushTime = DateTime.Parse(posPushJobStatus.LastRun.Value.ToString()).ToUniversalTime();

                double currentPublishDuration = (pusheenTime - posPushTime).TotalMinutes;

                if (currentPublishDuration < maxWaitTime)
                {
                    logger.Info(String.Format("{0} - The POS Push is currently publishing data.  The application will move on to the next region and retry later.", region));
                }
                else
                {
                    logger.Warn(String.Format("{0} - The POS Push job has been running for {1} minutes.", region, currentPublishDuration));
                }
            }
            else
            {
                jobIsRunning = false;
            }

            return jobIsRunning;
        }
    }
}
