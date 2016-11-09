using PagerDutyAPI;

namespace Icon.Monitoring.Common.PagerDuty
{
    public class PagerDutyResponse
    {
        public string Status { get; set; }
        public string IncidentKey { get; set; }
        public string Message { get; set; }

        public PagerDutyResponse()
        {

        }

        public PagerDutyResponse(EventAPIResponse eventAPIResponse)
        {
            Status = eventAPIResponse.Status;
            IncidentKey = eventAPIResponse.IncidentKey;
            Message = eventAPIResponse.Message;
        }
    }
}
