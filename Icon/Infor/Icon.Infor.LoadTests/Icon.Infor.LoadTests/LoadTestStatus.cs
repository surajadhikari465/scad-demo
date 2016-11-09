using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.LoadTests
{
    public class LoadTestStatus : ILoadTestStatus
    {
        public virtual double ElapsedTime { get; set; }
        public int FailedEntities { get; set; }
        public int ProcessedEntities { get; set; }
        public int UnprocessedEntities { get; set; }

        [IgnoreDataMember]
        public static LoadTestStatus Default
        {
            get
            {
                return new LoadTestStatus
                {
                    ElapsedTime = 0,
                    FailedEntities = 0,
                    ProcessedEntities = 0,
                    UnprocessedEntities = 0
                };
            }
        }
    }
}
