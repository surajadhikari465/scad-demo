using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class AddEwicExclusionCommand
    {
        public string ScanCode { get; set; }
        public List<Agency> Agencies { get; set; }
    }
}
