using Icon.Framework;
using Irma.Framework;
using RegionalEventController.Common;
using RegionalEventController.DataAccess.Interfaces;
using RegionalEventController.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace RegionalEventController.DataAccess.Queries
{
    public class GetInvalidInProcessedQueueEntriesQueryHandler : IQueryHandler<GetInvalidInProcessedQueueEntriesQuery, List<int>>
    {
        private IrmaContext context;
        public GetInvalidInProcessedQueueEntriesQueryHandler(IrmaContext context)
        {
            this.context = context;
        }

        public List<int> Execute(GetInvalidInProcessedQueueEntriesQuery parameters)
        {
            List<int> qIds = (from q in context.IconItemChangeQueue
                              where (q.InProcessBy == StartupOptions.Instance.ToString())
                                     select q.QID).ToList();

            return qIds.Except(parameters.queueIds).ToList();
        }
    }
}
