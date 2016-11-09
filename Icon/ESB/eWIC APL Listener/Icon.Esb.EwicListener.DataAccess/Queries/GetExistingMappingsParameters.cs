using Icon.Common.DataAccess;
using Icon.Esb.EwicAplListener.Common.Models;
using System.Collections.Generic;

namespace Icon.Esb.EwicAplListener.DataAccess.Queries
{
    public class GetExistingMappingsParameters : IQuery<List<ScanCodeModel>>
    {
        public string AgencyId { get; set; }
        public string AplScanCode { get; set; }
    }
}
