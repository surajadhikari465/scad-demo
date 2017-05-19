using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Commands
{
    public class RefreshItemsCommandHandler : ICommandHandler<RefreshItemsCommand>
    {
        private IconContext context;

        public RefreshItemsCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(RefreshItemsCommand data)
        {
            var itemIds = context.ScanCode
                .Where(sc => data.ScanCodes.Contains(sc.scanCode))
                .Select(sc => sc.itemID)
                .ToList()
                .ConvertAll(i => new
                {
                    itemID = i
                })
                .ToDataTable();            

            AddProductMessages(itemIds);
            AddRegionalEvents(itemIds);
        }

        private void AddRegionalEvents(DataTable itemIds)
        {
            SqlParameter inputType = new SqlParameter("updatedItemIDs", SqlDbType.Structured)
            {
                TypeName = "app.UpdatedItemIDsType",
                Value = itemIds
            };

            string sql = "EXEC app.GenerateItemUpdateEvents @updatedItemIDs";
            context.Database.ExecuteSqlCommand(sql, inputType);
            
        }

        private void AddProductMessages(DataTable itemIds)
        {
            SqlParameter inputType = new SqlParameter("updatedItemIDs", SqlDbType.Structured)
            {
                TypeName = "app.UpdatedItemIDsType",
                Value = itemIds
            };

            string sql = "EXEC infor.GenerateItemUpdateMessages @updatedItemIDs";
            context.Database.ExecuteSqlCommand(sql, inputType);
        }
    }
}
