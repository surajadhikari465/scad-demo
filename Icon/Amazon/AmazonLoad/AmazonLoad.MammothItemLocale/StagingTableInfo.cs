using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonLoad.MammothItemLocale
{
    public class StagingTableInfo
    {
        public StagingTableInfo() {
            TotalRecords = 0;
            UnprocessedRecords = 0;
            UnProcessedGroupIds = new List<int>();
        }
        public int TotalRecords { get; set; }
        public int UnprocessedRecords { get; set; }
        public List<int> UnProcessedGroupIds { get; set; }
    }
}
