using Icon.Common;
using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushController.DataAccess.Commands
{
    public class DeleteItemLinksCommandHandler : ICommandHandler<DeleteItemLinksCommand>
    {
        private IRenewableContext<IconContext> context;
        private ILogger<DeleteItemLinksCommandHandler> logger;

        public DeleteItemLinksCommandHandler(ILogger<DeleteItemLinksCommandHandler> logger, IRenewableContext<IconContext> context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void Execute(DeleteItemLinksCommand command)
        {
            if (command.ItemLinks == null || command.ItemLinks.Count == 0)
            {
                logger.Warn("DeleteItemLinksCommandHandler was called with a null or empty list.  Check execution logic in the calling method.");
                return;
            }

            var entitiesParameter = command.ItemLinks
                .Select(il => new
                {
                    ParentItemId = il.ParentItemId,
                    ChildItemId = il.ChildItemId,
                    LocaleItemId = il.LocaleId
                }).ToTvp("ItemLinkEntities", "app.ItemLinkEntityType");

            string sql = "EXEC app.DeleteItemLinkEntities @ItemLinkEntities";

            context.Context.Database.ExecuteSqlCommand(sql, entitiesParameter);
        }
    }
}
