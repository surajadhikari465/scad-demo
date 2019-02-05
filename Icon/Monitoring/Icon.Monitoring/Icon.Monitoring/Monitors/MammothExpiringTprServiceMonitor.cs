namespace Icon.Monitoring.Monitors
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Icon.Logging;
    using Icon.Monitoring.Common;
    using Icon.Monitoring.Common.Opsgenie;
    using Icon.Monitoring.Common.Settings;
    using Icon.Monitoring.DataAccess.Model;
    using Icon.Monitoring.DataAccess.Queries;
    using Newtonsoft.Json;
    using Icon.Monitoring.Common.Constants;
    using Icon.Monitoring.Common.Enums;

    public class MammothExpiringTprServiceMonitor : TimedControllerMonitor
    {
        private const string JobName = "Expiring TPR Service";
        private const string JobScheduleStatusReady = "ready";

        private readonly IMammothExpiringTprServiceMonitorSettings expiringTprSettings;
        private readonly IQueryHandlerMammoth<GetMammothJobScheduleParameters, JobSchedule> getMammothJobScheduleQuery;
        private readonly IOpsgenieTrigger opsgenieTrigger;
        private readonly IMonitorCache cache;

        public MammothExpiringTprServiceMonitor(
            IMonitorSettings settings,
            IMammothExpiringTprServiceMonitorSettings expiringTprSettings,
            IQueryHandlerMammoth<GetMammothJobScheduleParameters, JobSchedule> getMammothJobScheduleQuery,
            IOpsgenieTrigger opsgenieTrigger,
            IMonitorCache cache,
            ILogger logger)
        {
            this.settings = settings;
            this.expiringTprSettings = expiringTprSettings;
            this.opsgenieTrigger = opsgenieTrigger;
            this.cache = cache;
            this.getMammothJobScheduleQuery = getMammothJobScheduleQuery;
            this.logger = logger;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            expiringTprSettings.Load();

            foreach (var regionEnabledKvp in expiringTprSettings.ExpiringTprServiceMonitorEnabledByRegion)
            {
                if (regionEnabledKvp.Value)
                {
                    DateTime expectedUtcCompletionTime = expiringTprSettings.ExpiringTprServiceCompletionUtcTimeByRegion[regionEnabledKvp.Key];
                    if (DateTime.UtcNow >= expectedUtcCompletionTime)
                    {
                        VerifyControllerCompletedOnTime(regionEnabledKvp.Key, expectedUtcCompletionTime);
                    }
                    else
                    {
                        LogInfo(message: $"Skipping {JobName} {regionEnabledKvp.Key} monitoring. Current time is less than expected complete time.", region: regionEnabledKvp.Key);
                    }
                }
                else
                {
                    LogInfo(message: $"Skipping {JobName} {regionEnabledKvp.Key} monitoring. Monitoring for region is disabled.", region: regionEnabledKvp.Key);
                }
            }
        }

        private void VerifyControllerCompletedOnTime(string region, DateTime expectedUtcCompletionTime)
        {
            try
            {
                var getJobScheduleParams = new GetMammothJobScheduleParameters
                {
                    JobName = JobName,
                    Region = region
                };
                var jobSchedule = getMammothJobScheduleQuery.Search(getJobScheduleParams);

                if (jobSchedule != null && jobSchedule.Enabled)
                {
                    if (jobSchedule.Status != JobScheduleStatusReady)
                    {
                        TriggerOpsgenieIfNoCachedOpsgenieExists(
                            region,
                            $"{JobName} is not in ready status for region. The job could have failed to run and needs to be restarted or this could mean that the job is taking longer than expected to run.",
                            new Dictionary<string, string>
                            {
                                { "Region", region },
                                { "CurrentJobStatus", jobSchedule.Status },
                            });
                    }
                    else if (jobSchedule.LastRunEndDateTimeUtc.Value > DateTime.UtcNow.Date
                        && jobSchedule.LastRunEndDateTimeUtc <= expectedUtcCompletionTime)
                    {
                        LogInfo(message: $"{JobName} completed on time.", region: region);
                    }
                    else
                    {
                        TriggerOpsgenieIfNoCachedOpsgenieExists(
                            region,
                            $"{JobName} has not completed by expected time for region. This could mean that the job has not run at all today and needs to been manually started or that the job's scheduled run time is incorrect and needs to change.",
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
                    LogInfo(message: $"Skipping {JobName} {region} monitoring. Job schedule for {region} does not exist or is disabled.", region: region);
                }
            }
            catch (Exception ex)
            {
                LogError(message: $"Error occurred while trying to verify if the {JobName} completed for region.", region: region, ex: ex);
            }
        }

        private void TriggerOpsgenieIfNoCachedOpsgenieExists(string region, string errorMessage, Dictionary<string, string> jsonDetails)
        {
            string opsgenieCacheKey = JobName + region;
            if (cache.Contains(opsgenieCacheKey))
            {
                LogInfo(logger: this.logger, message: $"Skipping {JobName} Opsgenie alert because an alert was already triggered today.", region: region, error: errorMessage);
            }
            else
            {
                LogInfo(message: $"Triggering Opsgenie alert for {JobName} error.", region: region, error: errorMessage);
                var response = this.opsgenieTrigger.TriggerAlert("Expiring Tpr Service Issue",errorMessage, jsonDetails);
                LogInfo(message: $"{JobName} Monitor Opsgenie response.", region: region, opsgenieResponse: response);
                cache.Set(opsgenieCacheKey, DateTime.Now, GetTomorrowsUtcStartDate());
            }
        }
    }
}
