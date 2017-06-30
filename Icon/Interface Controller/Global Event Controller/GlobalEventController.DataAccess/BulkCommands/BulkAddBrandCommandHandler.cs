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
    public class BulkAddBrandCommandHandler : ICommandHandler<BulkAddBrandCommand>
    {
        private IDbContextFactory<IrmaContext> contextFactory;

        public BulkAddBrandCommandHandler(IDbContextFactory<IrmaContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Handle(BulkAddBrandCommand command)
        {
            List<string> scanCodes = command.ValidatedItems.Select(i => i.ScanCode).ToList();
            using (var context = contextFactory.CreateContext())
            {
                List<string> defaultIdentifiers = context.ItemIdentifier
                    .Where(ii => scanCodes.Contains(ii.Identifier) && ii.Default_Identifier == 1 && ii.Deleted_Identifier == 0)
                    .DistinctBy(ii => new { ii.Identifier })
                    .Select(di => di.Identifier)
                    .ToList();

                SqlParameter itemList = new SqlParameter("ValidatedItemList", SqlDbType.Structured);
                SqlParameter userName = new SqlParameter("UserName", SqlDbType.VarChar);
                itemList.TypeName = "dbo.IconUpdateItemType";
                itemList.Value = command.ValidatedItems.Where(vi => defaultIdentifiers.Contains(vi.ScanCode)).DistinctBy(vi => new { vi.BrandName }).ToList().ToDataTable();
                userName.Value = "iconcontrolleruser";

                string sql = "EXEC dbo.IconItemAddBrand @ValidatedItemList, @UserName";
                context.Database.ExecuteSqlCommand(sql, itemList, userName);
            }
        }
    }
}
