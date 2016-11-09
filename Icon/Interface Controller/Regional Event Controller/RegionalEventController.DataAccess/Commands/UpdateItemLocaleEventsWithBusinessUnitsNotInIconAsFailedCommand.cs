using RegionalEventController.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegionalEventController.DataAccess.Commands
{
    public class UpdateItemLocaleEventsWithBusinessUnitsNotInIconAsFailedCommand
    {
        public List<ItemLocaleEventModel> ItemLocaleEvents { get; set; }
    }
}
