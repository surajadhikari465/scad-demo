using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Queries
{
    public class GetFailedEventsQuery : IQueryHandler<GetFailedEventsParameters, List<EventQueue>>
    {
        private IconContext context;

        public GetFailedEventsQuery(IconContext context)
        {
            this.context = context;
        }

        public List<EventQueue> Search(GetFailedEventsParameters parameters)
        {
            return context.EventQueue.Where(e => e.ProcessFailedDate.HasValue)
                .ToList();
        }
    }
}
