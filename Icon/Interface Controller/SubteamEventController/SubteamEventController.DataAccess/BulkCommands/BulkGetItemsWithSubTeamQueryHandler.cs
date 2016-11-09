using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SubteamEventController.DataAccess.BulkCommands
{
    public class BulkGetItemsWithSubTeamQueryHandler : IQueryHandler<BulkGetItemsWithSubTeamQuery, List<ItemSubTeamModel>>
    {
        private readonly IconContext context;

        public BulkGetItemsWithSubTeamQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public List<ItemSubTeamModel> Handle(BulkGetItemsWithSubTeamQuery parameters)
        {
            SqlParameter scanCodes = new SqlParameter("ScanCodes", SqlDbType.Structured);
            
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("EventName");
            foreach (var scancode in parameters.ScanCodes)
            {
                dataTable.NewRow();
                dataTable.Rows.Add(scancode);
            }

            scanCodes.Value = dataTable;
            scanCodes.TypeName = "app.ScanCodeListType";

            string sql = @"EXEC app.GetItemSubTeamModel @ScanCodes";

            DbRawSqlQuery<ItemSubTeamModel> sqlQuery = this.context.Database.SqlQuery<ItemSubTeamModel>(sql, scanCodes);
            List<ItemSubTeamModel> validatedItems = new List<ItemSubTeamModel>(sqlQuery);

            return validatedItems;
        }
    }
}
