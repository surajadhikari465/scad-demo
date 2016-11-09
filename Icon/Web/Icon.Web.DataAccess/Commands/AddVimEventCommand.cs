using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Commands
{
    public class AddVimEventCommand
    {
        public int EventReferenceId { get; set; }
        public int EventTypeId { get; set; }
        public string EventMessage { get; set; }
    }
}

