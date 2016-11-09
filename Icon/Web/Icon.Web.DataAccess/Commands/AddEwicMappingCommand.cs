using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class AddEwicMappingCommand
    {
        public string AplScanCode { get; set; }
        public string WfmScanCode { get; set; }
        public List<Agency> Agencies { get; set; }
    }
}
