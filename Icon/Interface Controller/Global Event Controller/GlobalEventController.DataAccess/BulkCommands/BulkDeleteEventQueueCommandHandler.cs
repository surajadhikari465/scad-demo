using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceController.Common;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkDeleteEventQueueCommandHandler : ICommandHandler<BulkDeleteEventQueueCommand>
    {
        private readonly ContextManager contextManager;

        public BulkDeleteEventQueueCommandHandler(ContextManager contextManager)
        {
            this.contextManager = contextManager;
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

            try
            {
                this.contextManager.IconContext.Database.ExecuteSqlCommand(sql, eventsToDelete);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
