using Icon.ApiController.Common;
using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Commands
{
    public class MarkQueuedEntriesAsInProcessCommandHandler<T> : ICommandHandler<MarkQueuedEntriesAsInProcessCommand<T>> where T : class, IMessageQueue
    {
        private ILogger<MarkQueuedEntriesAsInProcessCommandHandler<T>> logger;
        private IDbContextFactory<IconContext> iconContextFactory;

        public MarkQueuedEntriesAsInProcessCommandHandler(
            ILogger<MarkQueuedEntriesAsInProcessCommandHandler<T>> logger,
            IDbContextFactory<IconContext> iconContextFactory)
        {
            this.logger = logger;
            this.iconContextFactory = iconContextFactory;
        }

        public void Execute(MarkQueuedEntriesAsInProcessCommand<T> data)
        {
            if (data.LookAhead == default(int))
            {
                logger.Error("The look-ahead value is zero.  This will prevent the controller from finding any queued entries to process.");
                return;
            }

            using (var context = iconContextFactory.CreateContext())
            {
                var messageQueueTable = context.Set<T>();

                int currentMessagesInProcess = messageQueueTable.Count(q => q.InProcessBy == data.Instance);

                if (currentMessagesInProcess < data.LookAhead)
                {
                    int newMessagesToMark = data.LookAhead - currentMessagesInProcess;

                    if (data.BusinessUnit == default(int))
                    {
                        logger.Info(string.Format("Controller {0} has {1} records in process and will attempt to mark {2} additional records.  The look-ahead value is {3}.",
                        ControllerType.Instance.ToString(), currentMessagesInProcess, newMessagesToMark.ToString(), data.LookAhead.ToString()));
                    }
                    else
                    {
                        logger.Info(string.Format("Controller {0} has {1} records in process and will attempt to mark {2} additional records for business unit {3}.  The look-ahead value is {4}.",
                        ControllerType.Instance.ToString(), currentMessagesInProcess, newMessagesToMark.ToString(), data.BusinessUnit.ToString(), data.LookAhead.ToString()));
                    }

                    string messageQueueTableName = typeof(T).Name;

                    SqlParameter numberOfRows = new SqlParameter("NumberOfRows", SqlDbType.Int);
                    numberOfRows.Value = newMessagesToMark;

                    SqlParameter jobInstance = new SqlParameter("JobInstance", SqlDbType.Int);
                    jobInstance.Value = data.Instance;

                    SqlParameter businessUnit = new SqlParameter("BusinessUnit", SqlDbType.Int);
                    businessUnit.Value = data.BusinessUnit;

                    string sql = $"EXEC app.Mark{messageQueueTableName}EntriesAsInProcess @NumberOfRows, @JobInstance, @BusinessUnit";

                    context.Database.ExecuteSqlCommand(sql, numberOfRows, jobInstance, businessUnit);
                }
            }
        }
    }
}
