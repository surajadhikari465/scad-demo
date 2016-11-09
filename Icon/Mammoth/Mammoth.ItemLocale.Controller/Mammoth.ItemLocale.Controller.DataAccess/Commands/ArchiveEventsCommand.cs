using Mammoth.Common.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ItemLocale.Controller.DataAccess.Commands
{
    public class ArchiveEventsCommand
    {
        public IEnumerable<ChangeQueueHistoryModel> Events { get; set; }
    }
}
