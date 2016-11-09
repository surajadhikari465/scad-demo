using RegionalEventController.DataAccess.Interfaces;
using System.Collections.Generic;

namespace RegionalEventController.DataAccess.Queries
{
    public class GetBrandNewScanCodesQuery : IQuery<List<string>>
    {
        public List<string> scanCodes { get; set; }
    }
}
