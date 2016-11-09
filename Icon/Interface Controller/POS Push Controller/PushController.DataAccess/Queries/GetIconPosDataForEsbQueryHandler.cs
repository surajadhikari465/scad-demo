using Icon.RenewableContext;
using Icon.Framework;
using Icon.Logging;
using MoreLinq;
using PushController.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PushController.DataAccess.Queries
{
    public class GetIconPosDataForEsbQueryHandler : IQueryHandler<GetIconPosDataForEsbQuery, List<IRMAPush>>
    {
        private ILogger<GetIconPosDataForEsbQueryHandler> logger;
        private IRenewableContext<IconContext> context;

        public GetIconPosDataForEsbQueryHandler(
            ILogger<GetIconPosDataForEsbQueryHandler> logger,
            IRenewableContext<IconContext> context)
        {
            this.logger = logger;
            this.context = context;
        }

        public List<IRMAPush> Execute(GetIconPosDataForEsbQuery parameters)
        {
            var posData = context.Context.IRMAPush
                .Where(ip =>
                    parameters.Instance == ip.InProcessBy &&
                    ip.EsbReadyDate == null &&
                    ip.EsbReadyFailedDate == null)
                .DistinctBy(ip => new { ip.Identifier, ip.BusinessUnit_ID })
                .OrderBy(ip => ip.IRMAPushID)
                .ToList();

            if (posData.Count > 0)
            {
                logger.Info(String.Format("Retrieved {0} staged POS record(s) marked by controller {1} and ready for ESB processing beginning with IRMAPushID {2}.",
                    posData.Count, parameters.Instance.ToString(), posData[0].IRMAPushID));
            }

            return posData;
        }
    }
}
