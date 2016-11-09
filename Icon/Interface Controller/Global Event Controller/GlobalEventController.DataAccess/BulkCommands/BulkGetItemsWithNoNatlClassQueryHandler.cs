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
    public class BulkGetItemsWithNoNatlClassQueryHandler : IQueryHandler<BulkGetItemsWithNoNatlClassQuery, List<ValidatedItemModel>>
    {
        private readonly IrmaContext context;

        public BulkGetItemsWithNoNatlClassQueryHandler(IrmaContext context)
        {
            this.context = context;
        }

        public List<ValidatedItemModel> Handle(BulkGetItemsWithNoNatlClassQuery parameters)
        {
            List<string> scanCodes = parameters.ValidatedItems.Select(i => i.ScanCode).ToList();

            SqlParameter itemList = new SqlParameter("ValidatedItemList", SqlDbType.Structured);
            itemList.TypeName = "dbo.IconUpdateItemType";
            itemList.Value = parameters.ValidatedItems.ToDataTable();

            string sql = @"EXEC dbo.GetIconItemsWithNoNatlClass @ValidatedItemList";

            DbRawSqlQuery<ValidatedItemModel> sqlQuery = this.context.Database.SqlQuery<ValidatedItemModel>(sql, itemList);
            List<ValidatedItemModel> validatedItems = new List<ValidatedItemModel>(sqlQuery);

            return validatedItems;
        }
    }
}
