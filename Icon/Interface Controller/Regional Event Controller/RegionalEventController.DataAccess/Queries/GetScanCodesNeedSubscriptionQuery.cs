using RegionalEventController.DataAccess.Interfaces;
using System.Collections.Generic;

namespace RegionalEventController.DataAccess.Queries
{
    public class GetScanCodesNeedSubscriptionQuery : IQuery<List<string>>
    {
        public List<string> scanCodes { get; set; }
        public string regionCode { get; set; }
    }
}
