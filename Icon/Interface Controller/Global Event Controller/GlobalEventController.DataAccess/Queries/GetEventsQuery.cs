using GlobalEventController.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Framework;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetEventsQuery : IQuery<List<EventQueue>>
    {
        public int MaxNumberOfEvents { get; set; }
        public List<string> RegisteredEvents { get; set; }
        public string[] RegionList { get; set; }
    }
}
