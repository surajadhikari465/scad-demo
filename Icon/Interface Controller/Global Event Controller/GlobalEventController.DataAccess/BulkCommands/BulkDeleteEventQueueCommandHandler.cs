using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Icon.Framework;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkDeleteEventQueueCommandHandler : ICommandHandler<BulkDeleteEventQueueCommand>
    {
        private IDbContextFactory<IconContext> contextFactory;

        public BulkDeleteEventQueueCommandHandler(IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Handle(BulkDeleteEventQueueCommand command)
        {
            SqlParameter eventsToDelete = new SqlParameter("EventsToDelete", SqlDbType.Structured);
            eventsToDelete.TypeName = "app.EventQueueIdType";

            DataTable dataTable = new DataTable();
            IEnumerable<int> queueIdCollection = command.EventsToDelete.Select(e => e.QueueId).Distinct();
            dataTable.Columns.Add("QueueId");
            foreach (var id in queueIdCollection)
            {
                dataTable.NewRow();
                dataTable.Rows.Add(id);
            }
            eventsToDelete.Value = dataTable;

            string sql = @"EXEC app.DeleteEventQueue @EventsToDelete";

            using (var context = contextFactory.CreateContext())
            {
                context.Database.ExecuteSqlCommand(sql, eventsToDelete);
            }
        }
    }
}
