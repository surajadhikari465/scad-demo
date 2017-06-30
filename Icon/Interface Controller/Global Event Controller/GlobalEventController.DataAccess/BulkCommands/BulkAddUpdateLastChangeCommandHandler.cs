using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Irma.Framework;
using MoreLinq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkAddUpdateLastChangeCommandHandler : ICommandHandler<BulkAddUpdateLastChangeCommand>
    {
        private IDbContextFactory<IrmaContext> contextFactory;

        public BulkAddUpdateLastChangeCommandHandler(IDbContextFactory<IrmaContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Handle(BulkAddUpdateLastChangeCommand command)
        {
            using (var context = contextFactory.CreateContext())
            {
                List<string> scanCodes = command.IconLastChangedItems.Select(i => i.ScanCode).ToList();
                List<string> defaultIdentifiers = context.ItemIdentifier
                    .Where(ii => scanCodes.Contains(ii.Identifier) && ii.Default_Identifier == 1 && ii.Deleted_Identifier == 0)
                    .DistinctBy(ii => new { ii.Identifier })
                    .Select(di => di.Identifier)
                    .ToList();

                SqlParameter itemList = new SqlParameter("LastChangedItemList", SqlDbType.Structured);
                itemList.TypeName = "dbo.IconLastChangedItemType";
                itemList.Value = command.IconLastChangedItems.Where(vi => defaultIdentifiers.Contains(vi.ScanCode)).DistinctBy(vi => new { vi.ScanCode }).ToList().ToDataTable();

                string sql = "EXEC dbo.IconItemAddUpdateLastChange @LastChangedItemList";
                context.Database.ExecuteSqlCommand(sql, itemList);
            }
        }
    }
}
