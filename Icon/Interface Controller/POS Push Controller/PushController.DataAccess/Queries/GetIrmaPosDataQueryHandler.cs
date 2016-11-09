using Icon.Logging;
using Irma.Framework;
using MoreLinq;
using PushController.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PushController.DataAccess.Queries
{
    public class GetIrmaPosDataQueryHandler : IQueryHandler<GetIrmaPosDataQuery, List<IConPOSPushPublish>>
    {
        private ILogger<GetIrmaPosDataQueryHandler> logger;

        public GetIrmaPosDataQueryHandler(ILogger<GetIrmaPosDataQueryHandler> logger)
        {
            this.logger = logger;
        }

        public List<IConPOSPushPublish> Execute(GetIrmaPosDataQuery parameters)
        {
            var posData = parameters.Context.IConPOSPushPublish
                .Where(p => p.InProcessBy == parameters.Instance)
                .DistinctBy(p => new { p.Identifier, p.BusinessUnit_ID, p.ChangeType })
                .OrderBy(p => p.IConPOSPushPublishID)
                .ToList();

            if (posData.Count > 0)
            {
                logger.Info(String.Format("Retrieved {0} published POS record(s) marked by controller {1} and ready to be staged beginning with IConPOSPushPublishID {2}.",
                    posData.Count, parameters.Instance, posData[0].IConPOSPushPublishID));
            }

            return posData;
        }
    }
}
