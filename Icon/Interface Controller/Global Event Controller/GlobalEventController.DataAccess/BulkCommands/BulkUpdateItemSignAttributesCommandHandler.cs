using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MoreLinq;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkUpdateItemSignAttributesCommandHandler : ICommandHandler<BulkUpdateItemSignAttributesCommand>
    {
        private IrmaContext context;

        public BulkUpdateItemSignAttributesCommandHandler(IrmaContext context)
        {
            this.context = context;
        }

        public void Handle(BulkUpdateItemSignAttributesCommand command)
        {
            SqlParameter itemList = new SqlParameter("Items", SqlDbType.Structured);
            itemList.TypeName = "dbo.IconUpdateItemType";
            itemList.Value = command.ValidatedItems.ToList().ToDataTable();

            string sql = "EXEC dbo.IconUpdateItemSignAttributes @Items";
            this.context.Database.ExecuteSqlCommand(sql, itemList);
        }
    }
}
