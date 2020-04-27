using Icon.Common.DataAccess;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetScanCodesParameters : IQuery<List<string>>
    {        
        public List<string> ListOfScanCodes { get; set; }
    }
}