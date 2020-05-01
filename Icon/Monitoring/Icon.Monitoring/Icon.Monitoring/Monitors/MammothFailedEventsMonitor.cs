using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Monitoring.Common.Enums;
using Icon.Monitoring.Common.Opsgenie;
using Icon.Monitoring.Common.Settings;
using Icon.Monitoring.DataAccess.Model;
using Icon.Monitoring.DataAccess.Queries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Monitoring.Monitors
{
    public class MammothFailedEventsMonitor : TimedControllerMonitor
    {
        private const string OpsGenieDescription = "Mammoth events have failed";
        private IQueryByRegionHandler<GetMammothFailedEventsParameters, List<MammothFailedEvent>> getMammothFailedEventsQueryHandler;
        private IOpsgenieTrigger opsgenieTrigger;

        public MammothFailedEventsMonitor(
            IMonitorSettings settings,
            IQueryByRegionHandler<GetMammothFailedEventsParameters, List<MammothFailedEvent>> getMammothFailedEventsQueryHandler,
            IOpsgenieTrigger opsgenieTrigger,
            ILogger logger)
        {
            this.settings = settings;
            this.getMammothFailedEventsQueryHandler = getMammothFailedEventsQueryHandler;
            this.opsgenieTrigger = opsgenieTrigger;
            this.logger = logger;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            try
            {
                logger.Info(JsonConvert.SerializeObject(new { Message = "Monitoring Mammoth Failed Events." }));

                var minutes = settings.MonitorTimers[nameof(MammothFailedEventsMonitor) + "Timer"].TotalMinutes;
                var endDate = DateTime.Now;
                var beginDate = endDate.AddMinutes(0 - minutes);

                foreach (IrmaRegions region in Enum.GetValues(typeof(IrmaRegions)))
                {
                    try
                    {
                        getMammothFailedEventsQueryHandler.TargetRegion = region;
                        GetMammothFailedEventsParameters parameters = new GetMammothFailedEventsParameters
                        {
                            BeginDate = beginDate,
                            EndDate = endDate
                        };

                        logger.Info(JsonConvert.SerializeObject(new
                        {
                            Message = "Retrieving Mammoth Failed Events",
                            Region = region.ToString(),
                            Parameters = parameters
                        }));

                        var failedEvents = getMammothFailedEventsQueryHandler.Search(parameters);
                        if (failedEvents != null && failedEvents.Any())
                        {
                            logger.Error(JsonConvert.SerializeObject(new
                            {
                                Message = "Mammoth Failed Events found. Creating OpsGenie Alert",
                                Region = region.ToString(),
                                Parameters = parameters,
                                FailedEventsCount = failedEvents.Count,
                                FailedEvents = failedEvents
                            }));

                            opsgenieTrigger.TriggerAlert(
                                $"Mammoth Failed Events: ${region}",
                                $"${OpsGenieDescription}: ${region}",
                                new Dictionary<string, string>
                                {
                                    { "Region", region.ToString() },
                                    { "Number of Failed Events", failedEvents.Count.ToString() },
                                    { "Timestamp", endDate.ToString() },
                                });
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(JsonConvert.SerializeObject(
                            new
                            {
                                Message = "Error occurred when pulling Mammoth failed events and generating an OpsGenie alert.",
                                Region = region.ToString(),
                                Exception = ex
                            }));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(JsonConvert.SerializeObject(
                    new
                    {
                        Message = "Error occurred when pulling Mammoth failed events and generating an OpsGenie alert.",
                        Exception = ex
                    }));
            }
        }
    }
}
