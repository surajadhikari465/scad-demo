using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.LoadTests.Service.Models
{
    public class LoadTestModel
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string ElapsedTime { get; set; }

        #region Configuration

        public int EntityCount { get; set; }
        public string TestRunTime { get; set; }
        public string PopulateTestDataInterval { get; set; }
        public string EmailRecipients { get; set; }

        #endregion
    }
}
