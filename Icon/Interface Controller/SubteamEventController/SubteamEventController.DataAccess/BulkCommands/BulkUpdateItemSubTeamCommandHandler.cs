using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;

namespace SubteamEventController.DataAccess.BulkCommands
{
    public class BulkUpdateItemSubTeamCommandHandler : ICommandHandler<BulkUpdateItemSubTeamCommand>
    {
        private readonly IrmaContext context;

        public BulkUpdateItemSubTeamCommandHandler(IrmaContext context)
        {
            this.context = context;
        }

        public void Handle(BulkUpdateItemSubTeamCommand command)
        {
            List<string> scanCodes = command.ItemSubTeamModels.Select(i => i.ScanCode).ToList();
            List<string> defaultIdentifiers = this.context.ItemIdentifier
                .Where(ii => scanCodes.Contains(ii.Identifier) && ii.Default_Identifier == 1 && ii.Deleted_Identifier == 0)
                .DistinctBy(ii => new { ii.Identifier })
                .Select(di => di.Identifier)
                .ToList();

            SqlParameter itemList = new SqlParameter("updatedItemList", SqlDbType.Structured);
            SqlParameter userName = new SqlParameter("UserName", SqlDbType.VarChar);
            itemList.TypeName = "dbo.IconItemWithSubteamType";
            itemList.Value = command.ItemSubTeamModels.Where(vi => defaultIdentifiers.Contains(vi.ScanCode)).DistinctBy(vi => new { vi.ScanCode }).ToList().ToDataTable();
            userName.Value = "iconcontrolleruser";

            string sql = "EXEC dbo.IconItemSubTeamUpdate @updatedItemList, @UserName";
            int r = this.context.Database.ExecuteSqlCommand(sql, itemList, userName);
        }
    }
}
