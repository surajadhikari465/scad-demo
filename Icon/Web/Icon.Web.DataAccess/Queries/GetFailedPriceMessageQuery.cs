﻿using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Icon.Web.DataAccess.Queries
{
    public class GetFailedPriceMessageQuery : IQueryHandler<GetFailedPriceMessageParameters, List<MessageQueuePrice>>
    {
        private IconContext context;
        public GetFailedPriceMessageQuery(IconContext context)
        {
            this.context = context;
        }

        public List<MessageQueuePrice> Search(GetFailedPriceMessageParameters parameters)
        {
            var failedPriceMessages = context.MessageQueuePrice.Where(mqp => mqp.MessageStatusId == MessageStatusTypes.Failed)
                .ToList();
            return failedPriceMessages;
        }
    }
}
