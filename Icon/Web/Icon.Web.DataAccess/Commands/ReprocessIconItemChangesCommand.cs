using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessIconItemChangesCommand
    {
        public List<int> IconItemChangeIds { get; set; }
        public string RegionalConnectionStringName { get; set; }
    }
}
