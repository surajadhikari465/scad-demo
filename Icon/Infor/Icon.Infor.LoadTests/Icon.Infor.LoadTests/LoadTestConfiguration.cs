using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.LoadTests
{
    public class LoadTestConfiguration : ILoadTestConfiguration
    {
        public int EntityCount { get; set; }
        public IEnumerable<ApplicationInstance> ApplicationInstances { get; set; }
        public TimeSpan TestRunTime { get; set; }
        public TimeSpan PopulateTestDataInterval { get; set; }
        public List<string> EmailRecipients { get; set; }
    }
}
