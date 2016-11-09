using Icon.ApiController.DataAccess.Commands;
using Icon.Common;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ApiController.DataAccess.Commands
{
    public class MarkQueuedEntriesAsInProcessCommandHandler<T> : ICommandHandler<MarkQueuedEntriesAsInProcessCommand<T>> where T : class, IMessageQueue
    {
        private IRenewableContext<MammothContext> globalContext;

        public MarkQueuedEntriesAsInProcessCommandHandler(IRenewableContext<MammothContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(MarkQueuedEntriesAsInProcessCommand<T> data)
        {
            var messageQueueTable = globalContext.Context.Set<T>();

            int currentMessagesInProcess = messageQueueTable.Count(q => q.InProcessBy == data.Instance);

            if (currentMessagesInProcess < data.LookAhead)
            {
                int newMessagesToMark = data.LookAhead - currentMessagesInProcess;

                SqlParameter tableNameParameter = new SqlParameter("MessageQueueTable", SqlDbType.NVarChar);
                tableNameParameter.Value = typeof(T).Name;

                SqlParameter lookAheadParameter = new SqlParameter("NumberOfRows", SqlDbType.Int);
                lookAheadParameter.Value = newMessagesToMark;

                SqlParameter instanceParameter = new SqlParameter("JobInstance", SqlDbType.Int);
                instanceParameter.Value = data.Instance;

                string sql = "EXEC esb.MarkMessageQueueEntriesAsInProcess @MessageQueueTable, @NumberOfRows, @JobInstance";

                globalContext.Context.Database.ExecuteSqlCommand(sql, tableNameParameter, lookAheadParameter, instanceParameter);
            }
        }
    }
}