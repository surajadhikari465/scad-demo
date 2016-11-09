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
    public class BulkUpdateNutrifactsCommandHandler : ICommandHandler<BulkUpdateNutriFactsCommand>
    {
        private readonly IrmaContext context;

        public BulkUpdateNutrifactsCommandHandler(IrmaContext context)
        {
            this.context = context;
        }

        public void Handle(BulkUpdateNutriFactsCommand command)
        {
            List<string> scanCodes = command.ItemNutriFacts.Select(i => i.Plu).ToList();
            List<string> defaultIdentifiers = this.context.ItemIdentifier
                .Where(ii => scanCodes.Contains(ii.Identifier) && ii.Default_Identifier == 1 && ii.Deleted_Identifier == 0)
                .DistinctBy(ii => new { ii.Identifier })
                .Select(di => di.Identifier)
                .ToList();

            SqlParameter nutritionItemList = new SqlParameter("IconNutrifacts", SqlDbType.Structured);
            SqlParameter userName = new SqlParameter("UserName", SqlDbType.VarChar);
            nutritionItemList.TypeName = "dbo.IconItemNutriFactsType";
            nutritionItemList.Value = command.ItemNutriFacts.Where(vi => defaultIdentifiers.Contains(vi.Plu)).DistinctBy(vi => new { vi.Plu }).ToList().ToDataTable();
            userName.Value = "iconcontrolleruser";

            string sql = "EXEC dbo.UpdateNutriFactsFromIcon @IconNutrifacts, @UserName";
            this.context.Database.ExecuteSqlCommand(sql, nutritionItemList, userName);
        }
    }
}
