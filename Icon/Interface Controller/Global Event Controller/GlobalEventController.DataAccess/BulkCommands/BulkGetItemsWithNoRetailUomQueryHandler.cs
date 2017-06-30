using GlobalEventController.Common;
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
    public class BulkGetItemsWithNoRetailUomQueryHandler : IQueryHandler<BulkGetItemsWithNoRetailUomQuery, List<ValidatedItemModel>>
    {
        private IDbContextFactory<IrmaContext> contextFactory;

        public BulkGetItemsWithNoRetailUomQueryHandler(IDbContextFactory<IrmaContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public List<ValidatedItemModel> Handle(BulkGetItemsWithNoRetailUomQuery parameters)
        {
            List<string> scanCodes = parameters.ValidatedItems.Select(i => i.ScanCode).ToList();

            SqlParameter itemList = new SqlParameter("ValidatedItemList", SqlDbType.Structured);
            itemList.TypeName = "dbo.IconUpdateItemType";
            itemList.Value = parameters.ValidatedItems.ToDataTable();

            using (var context = contextFactory.CreateContext())
            {
                string sql = @"EXEC dbo.GetIconItemsWithNoRetailUOM @ValidatedItemList";
                List<ValidatedItemModel> validatedItems = context.Database.SqlQuery<ValidatedItemModel>(sql, itemList).ToList();

                return validatedItems;
            }
        }
    }
}
