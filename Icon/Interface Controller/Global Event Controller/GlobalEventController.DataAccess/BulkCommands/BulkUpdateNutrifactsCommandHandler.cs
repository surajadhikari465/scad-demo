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
    public class BulkUpdateNutrifactsCommandHandler : ICommandHandler<BulkUpdateNutriFactsCommand>
    {
        private IDbContextFactory<IrmaContext> contextFactory;

        public BulkUpdateNutrifactsCommandHandler(IDbContextFactory<IrmaContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Handle(BulkUpdateNutriFactsCommand command)
        {
            using (var context = contextFactory.CreateContext())
            {
                List<string> scanCodes = command.ItemNutriFacts.Select(i => i.Plu).ToList();
                List<string> defaultIdentifiers = context.ItemIdentifier
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
                context.Database.ExecuteSqlCommand(sql, nutritionItemList, userName);
            }
        }
    }
}
