using Icon.ApiController.Common;
using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetMessageQueueQuery<T> : IQueryHandler<GetMessageQueueParameters<T>, List<T>> where T : class, IMessageQueue
    {
        private ILogger<GetMessageQueueQuery<T>> logger;
        private IRenewableContext<IconContext> globalContext;

        public GetMessageQueueQuery(ILogger<GetMessageQueueQuery<T>> logger, IRenewableContext<IconContext> globalContext)
        {
            this.logger = logger;
            this.globalContext = globalContext;
        }

        public List<T> Search(GetMessageQueueParameters<T> parameters)
        {
           DbQuery<T> messageQueueTable;

           if (parameters is GetMessageQueueParameters<MessageQueueProduct>)
           {
               messageQueueTable = globalContext.Context.Set<T>().Include("MessageQueueNutrition");
           }
           else
           {
               messageQueueTable = globalContext.Context.Set<T>();
           }

            var messagesReadyForProcessing = messageQueueTable.Where(mq => mq.InProcessBy == parameters.Instance).ToList();

            if (messagesReadyForProcessing.Count > 0)
            {
                logger.Info(String.Format("Found {0} queued message(s) marked by controller {1} and ready for processing.  Starting with MessageQueueId {2}.",
                    messagesReadyForProcessing.Count, ControllerType.Instance, messagesReadyForProcessing[0].MessageQueueId));
            }
            else
            {
                logger.Info(String.Format("Found 0 queued messages marked by controller {0} and ready for processing.", ControllerType.Instance));
            }

            return messagesReadyForProcessing;
        }
    }
}
