using Icon.ApiController.DataAccess.Queries;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
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
        private IDbContextFactory<MammothContext> mammothContextFactory;

        public GetMessageQueueQuery(IDbContextFactory<MammothContext> mammothContextFactory)
        {
            this.mammothContextFactory = mammothContextFactory;
        }

        public List<T> Search(GetMessageQueueParameters<T> parameters)
        {
            using (var context = mammothContextFactory.CreateContext())
            {
                var messageQueueTable = context.Set<T>();

                var messagesReadyForProcessing = messageQueueTable.Where(mq => mq.InProcessBy == parameters.Instance);

                return messagesReadyForProcessing.ToList();
            }
        }
    }
}
