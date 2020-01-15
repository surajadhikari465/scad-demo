using Icon.ApiController.Common;
using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Icon.DbContextFactory;
using System.Data.Entity.Infrastructure;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetMessageQueueQuery<T> : IQueryHandler<GetMessageQueueParameters<T>, List<T>> where T : class, IMessageQueue
    {
        private ILogger<GetMessageQueueQuery<T>> logger;
        private DbContextFactory.IDbContextFactory<IconContext> iconContextFactory;

        public GetMessageQueueQuery(ILogger<GetMessageQueueQuery<T>> logger, DbContextFactory.IDbContextFactory<IconContext> iconContextFactory)
        {
            this.logger = logger;
            this.iconContextFactory = iconContextFactory;
        }

        public List<T> Search(GetMessageQueueParameters<T> parameters)
        {
            DbQuery<T> messageQueueTable;

            using (var context = iconContextFactory.CreateContext())
            {
                if (parameters is GetMessageQueueParameters<MessageQueueProduct>)
                {
                    messageQueueTable = context.Set<T>().Include("MessageQueueNutrition");
                }
                else
                {
                    messageQueueTable = context.Set<T>();
                }

                var messagesReadyForProcessing = messageQueueTable.Where(mq => mq.InProcessBy == parameters.Instance).ToList();

                if (messagesReadyForProcessing.Count > 0)
                {
                    logger.Info(string.Format("Found {0} queued message(s) marked by controller {1} and ready for processing.  Starting with MessageQueueId {2}.",
                        messagesReadyForProcessing.Count, ControllerType.Instance, messagesReadyForProcessing[0].MessageQueueId));
                }
                else
                {
                    logger.Debug(string.Format("Found 0 queued messages marked by controller {0} and ready for processing.", ControllerType.Instance));
                }

                return messagesReadyForProcessing;
            }
        }
    }
}
