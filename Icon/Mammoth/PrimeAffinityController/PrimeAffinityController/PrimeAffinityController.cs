using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.PrimeAffinity.Library.Processors;
using PrimeAffinityController.Commands;
using PrimeAffinityController.Constants;
using PrimeAffinityController.Models;
using PrimeAffinityController.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace PrimeAffinityController
{
    public class PrimeAffinityController : IPrimeAffinityController
    {
        private PrimeAffinityControllerSettings settings;
        private IQueryHandler<GetPrimeAffinityAddPsgsFromPricesParameters, IEnumerable<PrimeAffinityPsgPriceModel>> getPrimeAffinityAddPsgsFromPricesQuery;
        private IQueryHandler<GetPrimeAffinityDeletePsgsFromPricesParameters, IEnumerable<PrimeAffinityPsgPriceModel>> getPrimeAffinityDeletePsgsFromPricesQuery;
        private IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters> primeAffinityPsgProcessor;
        private IQueryHandler<GetJobScheduleParameters, JobScheduleModel> getJobScheduleQuery;
        private ICommandHandler<UpdateJobStatusCommand> updateJobStatusCommandHandler;
        private ICommandHandler<UpdateJobScheduleOnCompletionCommand> updateJobScheduleOnCompletionCommandHandler;
        private ILogger<PrimeAffinityController> logger;
        private Timer timer;
        private string currentRegion;

        public PrimeAffinityController(
            PrimeAffinityControllerSettings settings,
            IQueryHandler<GetPrimeAffinityAddPsgsFromPricesParameters, IEnumerable<PrimeAffinityPsgPriceModel>> getPrimeAffinityAddPsgsFromPricesQuery,
            IQueryHandler<GetPrimeAffinityDeletePsgsFromPricesParameters, IEnumerable<PrimeAffinityPsgPriceModel>> getPrimeAffinityDeletePsgsFromPricesQuery,
            IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters> primeAffinityPsgProcessor,
            IQueryHandler<GetJobScheduleParameters, JobScheduleModel> getJobScheduleQuery,
            ICommandHandler<UpdateJobStatusCommand> updateJobStatusCommandHandler,
            ICommandHandler<UpdateJobScheduleOnCompletionCommand> updateJobScheduleOnCompletionCommandHandler,
            ILogger<PrimeAffinityController> logger)
        {
            this.settings = settings;
            this.getPrimeAffinityAddPsgsFromPricesQuery = getPrimeAffinityAddPsgsFromPricesQuery;
            this.getPrimeAffinityDeletePsgsFromPricesQuery = getPrimeAffinityDeletePsgsFromPricesQuery;
            this.primeAffinityPsgProcessor = primeAffinityPsgProcessor;
            this.getJobScheduleQuery = getJobScheduleQuery;
            this.updateJobStatusCommandHandler = updateJobStatusCommandHandler;
            this.updateJobScheduleOnCompletionCommandHandler = updateJobScheduleOnCompletionCommandHandler;
            this.logger = logger;
        }

        public void Start()
        {
            logger.Info(
                new
                {
                    Message = "Controller starting.",
                }.ToJson());
            timer = new Timer(settings.RunInterval);
            timer.Elapsed += Timer_Elapsed;
            Timer_Elapsed(null, null);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            try
            {
                logger.Info(
                    new
                    {
                        Message = "Checking if scheduled to process PSGs for all regions.",
                    }.ToJson());
                currentRegion = null;
                if (!InMaintenanceTimeWindow())
                {
                    foreach (var region in settings.Regions)
                    {
                        currentRegion = region;
                        JobScheduleModel jobSchedule = null;
                        try
                        {
                            jobSchedule = getJobScheduleQuery.Search(new GetJobScheduleParameters
                            {
                                JobName = settings.JobName,
                                Region = region
                            });
                            if (ScheduledToRunJob(jobSchedule, settings.JobName, region))
                            {
                                logger.Info(
                                    new
                                    {
                                        Message = "Processing PSGs for region.",
                                        Region = region
                                    }.ToJson());

                                UpdateJobStatus(jobSchedule, ApplicationConstants.JobScheduleStatuses.Running);
                                SendAddPsgs(region);
                                SendDeletePsgs(region);
                                UpdateJobScheduleOnCompletion(jobSchedule, ApplicationConstants.JobScheduleStatuses.Ready);
                            }
                            else
                            {
                                logger.Info(
                                    new
                                    {
                                        Message = "Not scheduled to process PSGs for region.",
                                        Region = region
                                    }.ToJson());
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error(
                                new
                                {
                                    Message = "Unexpected error occurred while processing PSGs for region.",
                                    Region = region,
                                    Error = ex
                                }.ToJson());
                            UpdateJobScheduleOnCompletion(jobSchedule, ApplicationConstants.JobScheduleStatuses.Failed);
                        }
                    }
                }
                else
                {
                    logger.Info(
                        new
                        {
                            Message = "In Maintenance time window. Skipping processing PSGs.",
                            MaintenanceDay = settings.MaintenanceDay,
                            MaintenanceStartTime = settings.MaintenanceStartTime,
                            MaintenanceEndTime = settings.MaintenanceEndTime
                        }.ToJson());
                }
            }
            catch (Exception ex)
            {
                logger.Error(
                    new
                    {
                        Message = "Fatal unexpected error occurred. Further regions will not be processed. Will continue to run controller again.",
                        Region = currentRegion,
                        Error = ex
                    }.ToJson());
            }
            finally
            {
                timer.Start();
            }
        }

        private void SendAddPsgs(string region)
        {
            var primeAffinityPsgPriceModels = getPrimeAffinityAddPsgsFromPricesQuery.Search(new GetPrimeAffinityAddPsgsFromPricesParameters
            {
                Region = region,
                ExcludedPSNumbers = settings.ExcludedPSNumbers,
                PriceTypes = settings.PriceTypes
            });
            
            primeAffinityPsgProcessor.SendPsgs(new PrimeAffinityPsgProcessorParameters
            {
                Region = region,
                PrimeAffinityMessageModels = primeAffinityPsgPriceModels.ToPrimeAffinityModels(),
                MessageAction = ActionEnum.AddOrUpdate
            });

            logger.Info(new { Message = "Finished processing Prime Affinity PSGs", Region = region, MessageAction = ActionEnum.AddOrUpdate, NumberOfPrimeAffinityMessagesSent = primeAffinityPsgPriceModels.Count() }.ToJson());
        }

        private void SendDeletePsgs(string region)
        {
            var primeAffinityPsgPriceModels = getPrimeAffinityDeletePsgsFromPricesQuery.Search(new GetPrimeAffinityDeletePsgsFromPricesParameters
            {
                Region = region,
                ExcludedPSNumbers = settings.ExcludedPSNumbers,
                PriceTypes = settings.PriceTypes
            });

            primeAffinityPsgProcessor.SendPsgs(new PrimeAffinityPsgProcessorParameters
            {
                Region = region,
                PrimeAffinityMessageModels = primeAffinityPsgPriceModels.ToPrimeAffinityModels(),
                MessageAction = ActionEnum.Delete
            });

            logger.Info(new { Message = "Finished processing Prime Affinity PSGs", Region = region, MessageAction = ActionEnum.Delete, NumberOfPrimeAffinityMessagesSent = primeAffinityPsgPriceModels.Count() }.ToJson());
        }

        private void UpdateJobScheduleOnCompletion(JobScheduleModel jobSchedule, string status)
        {
            updateJobScheduleOnCompletionCommandHandler.Execute(new UpdateJobScheduleOnCompletionCommand { JobSchedule = jobSchedule, Status = status });
        }

        private void UpdateJobStatus(JobScheduleModel jobSchedule, string status)
        {
            updateJobStatusCommandHandler.Execute(new UpdateJobStatusCommand { JobSchedule = jobSchedule, Status = status });
        }

        private bool ScheduledToRunJob(JobScheduleModel jobSchedule, string jobName, string region)
        {
            if (jobSchedule == null)
            {
                logger.Error(
                    new
                    {
                        Message = "No job schedule found. Unable to run job for given region.",
                        JobName = jobName,
                        Region = region
                    }.ToJson());

                return false;
            }
            else if (!jobSchedule.Enabled)
            {
                logger.Info(
                    new
                    {
                        Message = "Job is disabled. Unable to run job for given region.",
                        JobName = jobName,
                        Region = region,
                        JobSchedule = jobSchedule
                    }.ToJson());

                return false;
            }
            else if (jobSchedule.RunAdHoc.HasValue && jobSchedule.RunAdHoc.Value)
            {
                return true;
            }
            else if (!jobSchedule.Status.Equals(ApplicationConstants.JobScheduleStatuses.Ready, StringComparison.InvariantCultureIgnoreCase))
            {
                logger.Info(
                    new
                    {
                        Message = "Job status is not in 'READY' status. Unable to run job for given region.",
                        JobName = jobName,
                        Region = region,
                        JobSchedule = jobSchedule
                    }.ToJson());

                return false;
            }
            else if (DateTime.UtcNow < jobSchedule.StartDateTimeUtc)
            {
                logger.Info(
                    new
                    {
                        Message = "StartDateTimeUtc is in the future. Will run job when current UTC time is equal to or greater than StartDateTimeUtc.",
                        JobName = jobName,
                        Region = region,
                        CurrentUtcTime = DateTime.UtcNow,
                        JobSchedule = jobSchedule
                    }.ToJson());

                return false;
            }
            else if (DateTime.UtcNow < jobSchedule.NextScheduledDateTimeUtc)
            {
                logger.Info(
                    new
                    {
                        Message = "NextScheduledDateTimeUtc is in the future. Will run job when current UTC time is equal to or greater than NextScheduledDateTimeUtc.",
                        JobName = jobName,
                        Region = region,
                        CurrentUtcTime = DateTime.UtcNow,
                        JobSchedule = jobSchedule
                    }.ToJson());

                return false;
            }
            else
            {
                return true;
            }
        }

        private bool InMaintenanceTimeWindow()
        {
            var now = DateTime.Now;
            if (now.DayOfWeek == settings.MaintenanceDay
                && now.Hour >= settings.MaintenanceStartTime.Hours
                && now.Hour <= settings.MaintenanceEndTime.Hours)
                return true;
            else
                return false;
        }

        public void Stop()
        {
            timer.Stop();
            logger.Info(
                new
                {
                    Message = "Shutting down controller."
                }.ToJson());
        }
    }
}