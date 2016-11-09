using Icon.Framework;
using RegionalEventController.Common;
using RegionalEventController.DataAccess.Interfaces;
using RegionalEventController.DataAccess.Models;
using RegionalEventController.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.Entity;
using System.Reflection;


namespace RegionalEventController.DataAccess.Queries
{
    public class GetScanCodesNeedSubscriptionQueryHandler : IQueryHandler<GetScanCodesNeedSubscriptionQuery, List<string>>
    {
        private IconContext context;
        public GetScanCodesNeedSubscriptionQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public List<string> Execute(GetScanCodesNeedSubscriptionQuery parameters)
        {
            List<string> subscribedRegionalScanCodes = (from s in context.IRMAItemSubscription
                                          where (parameters.scanCodes.Contains(s.identifier)
                                             && s.regioncode == parameters.regionCode
                                             && s.deleteDate == null)
                                          select s.identifier).ToList();

            return parameters.scanCodes.Except(subscribedRegionalScanCodes).ToList();
        }
    }
}
