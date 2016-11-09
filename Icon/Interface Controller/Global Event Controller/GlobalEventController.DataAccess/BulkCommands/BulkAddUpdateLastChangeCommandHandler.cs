using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using System.Data;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkAddUpdateLastChangeCommandHandler : ICommandHandler<BulkAddUpdateLastChangeCommand>
    {
        private readonly IrmaContext context;

        public BulkAddUpdateLastChangeCommandHandler(IrmaContext context)
        {
            this.context = context;
        }

        public void Handle(BulkAddUpdateLastChangeCommand command)
        {
            List<string> scanCodes = command.IconLastChangedItems.Select(i => i.ScanCode).ToList();
            List<string> defaultIdentifiers = this.context.ItemIdentifier
                .Where(ii => scanCodes.Contains(ii.Identifier) && ii.Default_Identifier == 1 && ii.Deleted_Identifier == 0)
                .DistinctBy(ii => new { ii.Identifier })
                .Select(di => di.Identifier)
                .ToList();

            SqlParameter itemList = new SqlParameter("LastChangedItemList", SqlDbType.Structured);
            itemList.TypeName = "dbo.IconLastChangedItemType";
            itemList.Value = command.IconLastChangedItems.Where(vi => defaultIdentifiers.Contains(vi.ScanCode)).DistinctBy(vi => new { vi.ScanCode }).ToList().ToDataTable();

            string sql = "EXEC dbo.IconItemAddUpdateLastChange @LastChangedItemList";
            this.context.Database.ExecuteSqlCommand(sql, itemList);
        }
    }
}
