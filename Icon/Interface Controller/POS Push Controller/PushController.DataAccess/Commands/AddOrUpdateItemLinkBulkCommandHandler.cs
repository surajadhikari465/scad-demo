using Icon.RenewableContext;
using Icon.Framework;
using Icon.Logging;
using MoreLinq;
using PushController.DataAccess.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace PushController.DataAccess.Commands
{
    public class AddOrUpdateItemLinkBulkCommandHandler : ICommandHandler<AddOrUpdateItemLinkBulkCommand>
    {
        private ILogger<AddOrUpdateItemLinkBulkCommandHandler> logger;
        private IRenewableContext<IconContext> context;

        public AddOrUpdateItemLinkBulkCommandHandler(
            ILogger<AddOrUpdateItemLinkBulkCommandHandler> logger,
            IRenewableContext<IconContext> context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void Execute(AddOrUpdateItemLinkBulkCommand command)
        {
            if (command.ItemLinks == null || command.ItemLinks.Count == 0)
            {
                logger.Warn("AddOrUpdateItemLinkBulkCommandHandler was called with a null or empty list.  Check execution logic in the calling method.");
                return;
            }

            SqlParameter entitiesParameter = new SqlParameter("ItemLinkEntities", SqlDbType.Structured);
            entitiesParameter.TypeName = "app.ItemLinkEntityType";

            entitiesParameter.Value = command.ItemLinks.ConvertAll(e => new
                {
                    ParentItemId = e.parentItemID,
                    ChildItemId = e.childItemID,
                    LocaleId = e.localeID
                }).ToDataTable();

            string sql = "EXEC app.AddOrUpdateItemLinkEntities @ItemLinkEntities";

            context.Context.Database.ExecuteSqlCommand(sql, entitiesParameter);
        }
    }
}
