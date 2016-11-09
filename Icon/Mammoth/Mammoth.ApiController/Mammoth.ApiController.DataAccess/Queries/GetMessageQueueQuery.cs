using Icon.ApiController.DataAccess.Queries;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ApiController.DataAccess.Queries
{
    public class GetMessageQueueQuery<T> : IQueryHandler<GetMessageQueueParameters<T>, List<T>> where T : class, IMessageQueue
    {
        private IRenewableContext<MammothContext> globalContext;

        public GetMessageQueueQuery(IRenewableContext<MammothContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public List<T> Search(GetMessageQueueParameters<T> parameters)
        {
            var messageQueueTable = globalContext.Context.Set<T>();

            var messagesReadyForProcessing = messageQueueTable.Where(mq => mq.InProcessBy == parameters.Instance).ToList();

            return messagesReadyForProcessing;
        }
    }
}
