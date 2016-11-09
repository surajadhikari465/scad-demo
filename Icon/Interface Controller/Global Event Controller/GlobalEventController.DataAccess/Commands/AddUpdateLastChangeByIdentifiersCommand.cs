using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irma.Framework;

namespace GlobalEventController.DataAccess.Commands
{
    public class AddUpdateLastChangeByIdentifiersCommand
    {
        public List<ItemIdentifier> Identifiers { get; set; }
    }
}
