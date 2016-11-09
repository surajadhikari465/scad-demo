using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.LoadTests
{
    public interface ILoadTestConfiguration
    {
        int EntityCount { get; set; }
        IEnumerable<ApplicationInstance> ApplicationInstances { get; set; }
        TimeSpan TestRunTime { get; set; }
        TimeSpan PopulateTestDataInterval { get; set; }
        List<string> EmailRecipients { get; set; }
    }
}
