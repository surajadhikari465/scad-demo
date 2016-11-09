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
    public class GetIconPosDataForUdmQueryHandler : IQueryHandler<GetIconPosDataForUdmQuery, List<IRMAPush>>
    {
        private ILogger<GetIconPosDataForUdmQueryHandler> logger;
        private IRenewableContext<IconContext> context;

        public GetIconPosDataForUdmQueryHandler(
            ILogger<GetIconPosDataForUdmQueryHandler> logger,
            IRenewableContext<IconContext> context)
        {
            this.logger = logger;
            this.context = context;
        }

        public List<IRMAPush> Execute(GetIconPosDataForUdmQuery parameters)
        {
            var posData = context.Context.IRMAPush
                .Where(ip =>
                    ip.InProcessBy == parameters.Instance &&
                    ip.EsbReadyDate != null &&
                    ip.InUdmDate == null &&
                    ip.UdmFailedDate == null)
                .DistinctBy(ip => new { ip.Identifier, ip.BusinessUnit_ID })
                .OrderBy(ip => ip.IRMAPushID)
                .ToList();

            if (posData.Count > 0)
            {
                logger.Info(String.Format("Retrieved {0} staged POS record(s) marked by controller {1} and ready for UDM processing beginning with IRMAPushID {2}.",
                    posData.Count, parameters.Instance, posData[0].IRMAPushID));
            }

            return posData;
        }
    }
}
