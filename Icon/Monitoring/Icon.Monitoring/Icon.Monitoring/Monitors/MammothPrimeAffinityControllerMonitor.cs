using Icon.Logging;
using Icon.Monitoring.Common;
using Icon.Monitoring.Common.PagerDuty;
using Icon.Monitoring.Common.Settings;
using Icon.Monitoring.DataAccess.Model;
using Icon.Monitoring.DataAccess.Queries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Icon.Monitoring.Monitors
{
    public class MammothPrimeAffinityControllerMonitor : TimedControllerMonitor
    {
        private const string JobName = "Mammoth Prime Affinity Controller";
        private const string JobScheduleStatusReady = "ready";

        private readonly IMammothPrimeAffinityControllerMonitorSettings primeAffinitySettings;
        private readonly IQueryHandlerMammoth<GetMammothJobScheduleParameters, JobSchedule> getMammothJobScheduleQuery;
        private readonly IPagerDutyTrigger pagerDutyTrigger;
        private readonly IMonitorCache cache;

        public MammothPrimeAffinityControllerMonitor(
            IMonitorSettings settings,
            IMammothPrimeAffinityControllerMonitorSettings primeAffinitySettings,
            IQueryHandlerMammoth<GetMammothJobScheduleParameters, JobSchedule> getMammothJobScheduleQuery,
            IPagerDutyTrigger pagerDutyTrigger,
            IMonitorCache cache,
            ILogger logger)
        {
            this.settings = settings;
            this.primeAffinitySettings = primeAffinitySettings;
            this.pagerDutyTrigger = pagerDutyTrigger;
            this.cache = cache;
            this.getMammothJobScheduleQuery = getMammothJobScheduleQuery;
            this.logger = logger;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            primeAffinitySettings.Load();

            foreach (var regionEnabledKvp in primeAffinitySettings.PrimeAffinityControllerMonitorEnabledByRegion)
            {
                if (regionEnabledKvp.Value)
                {
                    DateTime expectedUtcCompletionTime = primeAffinitySettings.PrimeAffinityPsgCompletionUtcTimeByRegion[regionEnabledKvp.Key];
                    if (DateTime.UtcNow >= expectedUtcCompletionTime)
                    {
                        VerifyControllerCompletedOnTime(regionEnabledKvp.Key, expectedUtcCompletionTime);
                    }
                    else
                    {
                        logger.Info(JsonConvert.SerializeObject(new
                        {
                            Message = $"Skipping Mammoth Prime Affinity Controller {regionEnabledKvp.Key} monitoring. Current time is less than expected complete time.",
                            Region = regionEnabledKvp.Key
                        }));
                    }
                }
                else
                {
                    logger.Info(JsonConvert.SerializeObject(new
                    {
                        Message = $"Skipping Mammoth Prime Affinity Controller {regionEnabledKvp.Key} monitoring. Monitoring for region is disabled.",
                        Region = regionEnabledKvp.Key
                    }));
                }
            }
        }

        private void VerifyControllerCompletedOnTime(string region, DateTime expectedUtcCompletionTime)
        {
            try
            {
                var jobSchedule = getMammothJobScheduleQuery.Search(new GetMammothJobScheduleParameters
                {
                    JobName = JobName,
                    Region = region
                });

                if (jobSchedule != null && jobSchedule.Enabled)
                {
                    if (jobSchedule.Status != JobScheduleStatusReady)
                    {
                        TriggerPagerDutyIfNoCachedPagerDutyExists(
                            region,
                            "Mammoth Prime Affinity Controller is not in ready status for region. The job could have failed to run and needs to be restarted or this could mean that the job is taking longer than expected to run.",
                            new Dictionary<string, string>
                            {
                                { "Region", region },
                                { "CurrentJobStatus", jobSchedule.Status },
                            });
                    }
                    else if (jobSchedule.LastRunEndDateTimeUtc.Value > DateTime.UtcNow.Date
                        && jobSchedule.LastRunEndDateTimeUtc <= expectedUtcCompletionTime)
                    {
                        logger.Info(JsonConvert.SerializeObject(new
                        {
                            Message = "Mammoth Prime Affinity Controller completed on time.",
                            Region = region
                        }));
                    }
                    else
                    {
                        TriggerPagerDutyIfNoCachedPagerDutyExists(
                            region,
                            "Mammoth Prime Affinity Controller has not completed by expected time for region. This could mean that the job has not ran at all today and needs to been manually started or that the job's scheduled run time is incorrect and needs to change.",
                            new Dictionary<string, string>
                            {
                                { "Region", region },
                                { "ExpectedCompletionTimeUtc", expectedUtcCompletionTime.ToString("s") },
                                { "LastRunEndDateTimeUtc", jobSchedule.LastRunEndDateTimeUtc.Value.ToString("s") }
                            });
                    }
                }
                else
                {
                    logger.Info(JsonConvert.SerializeObject(new
                    {
                        Message = $"Skipping Mammoth Prime Affinity Controller {region} monitoring. Job schedule for {region} does not exist or is disabled.",
                        Region = region
                    }));
                }
            }
            catch (Exception ex)
            {
                logger.Error(JsonConvert.SerializeObject(new
                {
                    Message = "Error occurred while trying to verify if the Mammoth Prime Affinity Controller completed for region.",
                    Region = region,
                    Error = ex
                }));
            }
        }

        private void TriggerPagerDutyIfNoCachedPagerDutyExists(string region, string errorMessage, Dictionary<string, string> jsonDetails)
        {
            string pagerDutyCacheKey = JobName + region;
            if (cache.Contains(pagerDutyCacheKey))
            {
                logger.Info(JsonConvert.SerializeObject(new
                {
                    Message = "Skipping Mammoth Prime Affinity Controller PagerDuty alert because an alert was already triggered today.",
                    Region = region,
                    Error = errorMessage
                }));
            }
            else
            {
                logger.Info(JsonConvert.SerializeObject(new
                {
                    Message = "Triggering PagerDuty alert for Mammoth Prime Affinity Controller error.",
                    Region = region,
                    Error = errorMessage
                }));
                var response = this.pagerDutyTrigger.TriggerIncident(
                                errorMessage,
                                jsonDetails);
                logger.Info(JsonConvert.SerializeObject(new
                {
                    Message = "Mammoth Prime Affinity Controller Monitor PagerDuty response.",
                    Region = region,
                    Response = response
                }));
                cache.Set(
                    pagerDutyCacheKey,
                    DateTime.Now,
                    GetTomorrowsUtcStartDate());
            }
        }
    }
}