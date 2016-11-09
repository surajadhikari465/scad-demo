using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedIrmaPushCommand
    {
        public List<int> IrmaPushIds { get; set; }
    }
}
