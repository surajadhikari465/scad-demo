using Icon.ApiController.Common;
using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetMessageHistoryQuery : IQueryHandler<GetMessageHistoryParameters, List<MessageHistory>>
    {
        private ILogger<GetMessageHistoryQuery> logger;
        private IRenewableContext<IconContext> globalContext;

        public GetMessageHistoryQuery(ILogger<GetMessageHistoryQuery> logger, IRenewableContext<IconContext> globalContext)
        {
            this.logger = logger;
            this.globalContext = globalContext;
        }

        public List<MessageHistory> Search(GetMessageHistoryParameters parameters)
        {
            var messages = globalContext.Context.MessageHistory
                .Where(mh => mh.InProcessBy == parameters.Instance && mh.MessageTypeId == parameters.MessageTypeId)
                .ToList();

            if (messages.Count > 0)
            {
                logger.Info(String.Format("Controller {0} found {1} MessageHistory entries ready for processing.  Starting with MessageHistoryId {2}.",
                    ControllerType.Instance, messages.Count, messages[0].MessageHistoryId));
            }
            else
            {
                logger.Info(String.Format("Controller {0} found 0 MessageHistory entries ready for processing.", ControllerType.Instance));
            }

            return messages;
        }
    }
}
