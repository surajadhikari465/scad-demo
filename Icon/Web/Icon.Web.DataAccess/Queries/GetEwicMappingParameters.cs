using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetEwicMappingsParameters : IQuery<List<EwicMappingModel>>
    {
        public string AplScanCode { get; set; }
    }
}
