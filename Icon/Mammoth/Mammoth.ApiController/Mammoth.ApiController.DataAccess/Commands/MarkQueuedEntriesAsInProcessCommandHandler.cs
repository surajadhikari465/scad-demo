using Icon.ApiController.DataAccess.Commands;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Mammoth.Framework;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Mammoth.ApiController.DataAccess.Commands
{
    public class MarkQueuedEntriesAsInProcessCommandHandler<T> : ICommandHandler<MarkQueuedEntriesAsInProcessCommand<T>> where T : class, IMessageQueue
    {
        private IDbContextFactory<MammothContext> mammothContextFactory;

        public MarkQueuedEntriesAsInProcessCommandHandler(IDbContextFactory<MammothContext> mammothContextFactory)
        {
            this.mammothContextFactory = mammothContextFactory;
        }

        public void Execute(MarkQueuedEntriesAsInProcessCommand<T> data)
        {
            using (var context = mammothContextFactory.CreateContext())
            {
                var messageQueueTable = context.Set<T>();

                int currentMessagesInProcess = messageQueueTable.Count(q => q.InProcessBy == data.Instance);

                if (currentMessagesInProcess < data.LookAhead)
                {
                    int newMessagesToMark = data.LookAhead - currentMessagesInProcess;

                    string messageQueueTableName = typeof(T).Name;

                    SqlParameter lookAheadParameter = new SqlParameter("NumberOfRows", SqlDbType.Int);
                    lookAheadParameter.Value = newMessagesToMark;

                    SqlParameter instanceParameter = new SqlParameter("JobInstance", SqlDbType.Int);
                    instanceParameter.Value = data.Instance;

                    string sql = $"EXEC esb.Mark{messageQueueTableName}EntriesAsInProcess @NumberOfRows, @JobInstance";

                    context.Database.ExecuteSqlCommand(sql, lookAheadParameter, instanceParameter);
                }
            }
        }
    }
}