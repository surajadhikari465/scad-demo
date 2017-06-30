using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkUpdateEventQueueInProcessCommandHandler : IQueryHandler<BulkUpdateEventQueueInProcessCommand, List<EventQueueCustom>>
    {
        private Icon.DbContextFactory.IDbContextFactory<IconContext> contextFactory;

        public BulkUpdateEventQueueInProcessCommandHandler(Icon.DbContextFactory.IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
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

            using (var context = contextFactory.CreateContext())
            {
                List<EventQueueCustom> queuedEvents = context.Database.SqlQuery<EventQueueCustom>(sql, eventNames, maxRows, instance).ToList();
                return queuedEvents;
            }
        }
    }
}
