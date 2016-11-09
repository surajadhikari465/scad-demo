using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Monitoring.Common.PagerDuty
{
    public class ConsolePagerDutyTrigger : IPagerDutyTrigger
    {
        public PagerDutyResponse TriggerIncident(string description, Dictionary<string, string> details)
        {
            Console.WriteLine(description);
            details.ToList().ForEach(x => Console.WriteLine(string.Format("{0} : {1}", x.Key, x.Value)));

            return null;
        }
    }
}
