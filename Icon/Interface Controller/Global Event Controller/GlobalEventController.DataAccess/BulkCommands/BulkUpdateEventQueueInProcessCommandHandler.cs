using GlobalEventController.DataAccess.Infrastructure;
using Icon.Common;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceController.Common;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkUpdateEventQueueInProcessCommandHandler : IQueryHandler<BulkUpdateEventQueueInProcessCommand, List<EventQueueCustom>>
    {
        private readonly ContextManager contextManager;

        public BulkUpdateEventQueueInProcessCommandHandler(ContextManager contextManager)
        {
            this.contextManager = contextManager;
        }
        
        public List<EventQueueCustom> Handle(BulkUpdateEventQueueInProcessCommand parameters)
        {
            SqlParameter eventNames = new SqlParameter("RegisteredEventNames", SqlDbType.Structured);
            eventNames.TypeName = "app.EventNameType";
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("EventName");
            foreach (var name in parameters.RegisteredEventNames)
            {
                dataTable.NewRow();
                dataTable.Rows.Add(name.MapToIconEvent());
            }
            eventNames.Value = dataTable;

            SqlParameter maxRows = new SqlParameter("MaxRows", SqlDbType.Int);
            maxRows.Value = parameters.MaxRows;

            SqlParameter instance = new SqlParameter("Instance", SqlDbType.NVarChar);
            instance.Value = parameters.Instance;

            string sql = @"EXEC app.UpdateEventQueueInProcess @RegisteredEventNames, @MaxRows, @Instance";

            DbRawSqlQuery<EventQueueCustom> custom = this.contextManager.IconContext.Database.SqlQuery<EventQueueCustom>(sql, eventNames, maxRows, instance);
            List<EventQueueCustom> queuedEvents = new List<EventQueueCustom>(custom);
            return queuedEvents;
        }
    }
}
