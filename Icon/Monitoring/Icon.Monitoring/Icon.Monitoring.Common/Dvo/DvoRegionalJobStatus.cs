using Icon.Monitoring.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.Common.Dvo
{
    public class DvoRegionalJobStatus
    {
        public IFileInfo FileInfo { get; set; }
        public string Region { get; set; }
        public string Error { get; set; }
    }
}
