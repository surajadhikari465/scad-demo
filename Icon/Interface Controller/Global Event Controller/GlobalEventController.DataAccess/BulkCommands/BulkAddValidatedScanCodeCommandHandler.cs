using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Irma.Framework;
using MoreLinq;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkAddValidatedScanCodeCommandHandler : ICommandHandler<BulkAddValidatedScanCodeCommand>
    {
        private IDbContextFactory<IrmaContext> contextFactory;

        public BulkAddValidatedScanCodeCommandHandler(IDbContextFactory<IrmaContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Handle(BulkAddValidatedScanCodeCommand command)
        {
            using (var context = contextFactory.CreateContext())
            {
                SqlParameter itemList = new SqlParameter("ValidatedItemList", SqlDbType.Structured);
                itemList.TypeName = "dbo.IconUpdateItemType";
                itemList.Value = command.ValidatedItems.DistinctBy(vi => new { vi.ScanCode }).ToList().ToDataTable();

                string sql = "EXEC dbo.IconItemAddValidatedScanCode @ValidatedItemList";
                context.Database.ExecuteSqlCommand(sql, itemList);
            }
        }
    }
}
