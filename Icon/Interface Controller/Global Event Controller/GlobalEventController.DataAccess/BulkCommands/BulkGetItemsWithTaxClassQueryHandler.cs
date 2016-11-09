using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkGetItemsWithTaxClassQueryHandler : IQueryHandler<BulkGetItemsWithTaxClassQuery, List<ValidatedItemModel>>
    {
        private readonly IrmaContext context;

        public BulkGetItemsWithTaxClassQueryHandler(IrmaContext context)
        {
            this.context = context;
        }

        public List<ValidatedItemModel> Handle(BulkGetItemsWithTaxClassQuery parameters)
        {
            List<string> scanCodes = parameters.ValidatedItems.Select(i => i.ScanCode).ToList();

            SqlParameter itemList = new SqlParameter("ValidatedItemList", SqlDbType.Structured);
            itemList.TypeName = "dbo.IconUpdateItemType";
            itemList.Value = parameters.ValidatedItems.ToDataTable();
           
            string sql = @"EXEC dbo.GetIconItemWithTax @ValidatedItemList";

            DbRawSqlQuery<ValidatedItemModel> sqlQuery = this.context.Database.SqlQuery<ValidatedItemModel>(sql, itemList);
            List<ValidatedItemModel> validatedItems = new List<ValidatedItemModel>(sqlQuery);

            return validatedItems;
        }
    }
}
