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

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkAddValidatedScanCodeCommandHandler : ICommandHandler<BulkAddValidatedScanCodeCommand>
    {
        private readonly IrmaContext context;

        public BulkAddValidatedScanCodeCommandHandler(IrmaContext context)
        {
            this.context = context;
        }

        public void Handle(BulkAddValidatedScanCodeCommand command)
        {
            SqlParameter itemList = new SqlParameter("ValidatedItemList", SqlDbType.Structured);
            itemList.TypeName = "dbo.IconUpdateItemType";
            itemList.Value = command.ValidatedItems.DistinctBy(vi => new { vi.ScanCode }).ToList().ToDataTable();

            string sql = "EXEC dbo.IconItemAddValidatedScanCode @ValidatedItemList";
            this.context.Database.ExecuteSqlCommand(sql, itemList);
        }
    }
}
