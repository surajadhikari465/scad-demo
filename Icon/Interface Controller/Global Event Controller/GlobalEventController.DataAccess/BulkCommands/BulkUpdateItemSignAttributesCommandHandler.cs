using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Irma.Framework;
using MoreLinq;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkUpdateItemSignAttributesCommandHandler : ICommandHandler<BulkUpdateItemSignAttributesCommand>
    {
        private IDbContextFactory<IrmaContext> contextFactory;

        public BulkUpdateItemSignAttributesCommandHandler(IDbContextFactory<IrmaContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Handle(BulkUpdateItemSignAttributesCommand command)
        {
            SqlParameter itemList = new SqlParameter("Items", SqlDbType.Structured);
            itemList.TypeName = "dbo.IconUpdateItemType";
            itemList.Value = command.ValidatedItems.ToList().ToDataTable();

            using (var context = contextFactory.CreateContext())
            {
                string sql = "EXEC dbo.IconUpdateItemSignAttributes @Items";
                context.Database.ExecuteSqlCommand(sql, itemList);
            }
        }
    }
}
