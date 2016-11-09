using System.Collections.Generic;

namespace Icon.Monitoring.Common.PagerDuty
{
    public interface IPagerDutyTrigger
    {
        PagerDutyResponse TriggerIncident(string description, Dictionary<string, string> details);
    }
}
