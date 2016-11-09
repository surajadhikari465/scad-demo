using GlobalEventController.Common;
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

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkGetValidatedItemsQueryHandler : IQueryHandler<BulkGetValidatedItemsQuery, List<ValidatedItemModel>>
    {
        private readonly ContextManager contextManager;
        private GlobalControllerSettings settings;

        public BulkGetValidatedItemsQueryHandler(ContextManager contextManager, GlobalControllerSettings settings)
        {
            this.contextManager = contextManager;
            this.settings = settings;
        }

        public List<ValidatedItemModel> Handle(BulkGetValidatedItemsQuery parameters)
        {
            SqlParameter scanCodes = new SqlParameter("ScanCodes", SqlDbType.Structured);
            
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ScanCode");
            foreach (var e in parameters.Events)
            {
                dataTable.NewRow();
                dataTable.Rows.Add(e.EventMessage);
            }

            scanCodes.Value = dataTable;
            scanCodes.TypeName = "app.ScanCodeListType";

            string sql = GetSql(this.settings.EnableInforUpdates);

            var sqlQuery = this.contextManager.IconContext.Database.SqlQuery<ValidatedItemModel>(sql, scanCodes);
            List<ValidatedItemModel> validatedItems = sqlQuery.ToList();

            // Set EventTypeId
            foreach (var item in validatedItems)
            {
                item.EventTypeId = parameters.Events.First(e => e.EventMessage == item.ScanCode).EventId;
            }

            return validatedItems;
        }

        private string GetSql(bool enableInforUpdates)
        {
            if(enableInforUpdates)
            {
                return @"EXEC infor.GetValidatedItemModel @ScanCodes";
            }
            else
            {
                return @"EXEC app.GetValidatedItemModel @ScanCodes";
            }
        }
    }
}
