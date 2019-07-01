using ApplicationMonitor.Web.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationMonitor.Web.Infrastructure
{

    public static class Environments
    {
        public static Dictionary<string, string> Icon = new Dictionary<string, string>
            {
                { "vm-icon-dev1", IconEnvironments.Dev },
                { "vm-icon-test1", IconEnvironments.Test },
                { "vm-icon-test2", IconEnvironments.Test },
                { "vm-icon-qa1", IconEnvironments.Qa },
                { "vm-icon-qa2", IconEnvironments.Qa },
                { "vm-icon-qa3", IconEnvironments.Qa },
                { "vm-icon-qa4", IconEnvironments.Qa }
            };
            public static Dictionary<string, string> Esb = new Dictionary<string, string>
            {
                { "ssl://DEV-ESB-EMS-1.wfm.pvt:7233", EsbEnvironments.Dev },
                { "ssl://cerd1636.wfm.pvt:7233", EsbEnvironments.DevDup },
                { "ssl://cerd1617.wfm.pvt:17293", EsbEnvironments.Test },
                { "ssl://cerd1637.wfm.pvt:17293", EsbEnvironments.TestDup },
                { "ssl://cerd1619.wfm.pvt:27293,ssl://cerd1622.wfm.pvt:27293", EsbEnvironments.QaFunc },
                { "ssl://cerd1639.wfm.pvt:27293,ssl://cerd1640.wfm.pvt:27293", EsbEnvironments.QaDup },
                { "ssl://cerd1630.wfm.pvt:27293,ssl://cerd1631.wfm.pvt:27293", EsbEnvironments.QaPerf }
            };
    }
}
