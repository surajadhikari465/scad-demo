using System;
using System.Linq;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Icon.Framework;
using KitBuilder.ESB.Listeners.Item.Service.Constants;

namespace KitBuilder.ESB.Listeners.Item.Service.Commands
{
    public class GenerateItemMessagesCommandHandler : ICommandHandler<GenerateItemMessagesCommand>
    {
        private const string UpdatedItemIDs = "updatedItemIds";
        private const string UpdatedItemIDsType = "app.UpdatedItemIDsType";
        private const string Exec_GenerateItemUpdateEvents_SP = "exec app.GenerateItemUpdateEvents @updatedItemIDs";
        private const string Exec_GenerateItemUpdateMessages_SP = "exec infor.GenerateItemUpdateMessages @updatedItemIDs";

        private IDbContextFactory<IconContext> contextFactory;

        public GenerateItemMessagesCommandHandler(IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Execute(GenerateItemMessagesCommand data)
        {
            var itemsWithoutErrors = data.Items.Where(i => i.ErrorCode == null);
            try
            {
                using (var context = contextFactory.CreateContext())
                {
                    this.GenerateItemMessagesForEventQueue(context, data);
                    this.GenerateItemMessagesForMessageQueueProduct(context, data);
                }
            }
            catch(Exception ex)
            {
                string errorDetails = ApplicationErrors.Messages.GenerateItemMessagesError + " Exception: " + ex.ToString();
                foreach (var item in itemsWithoutErrors)
                {
                    item.ErrorCode = ApplicationErrors.Codes.GenerateItemMessagesError;
                    item.ErrorDetails = errorDetails;
                }
            }
        }

        private void GenerateItemMessagesForEventQueue(IconContext context, GenerateItemMessagesCommand data)
        {
            // See app.UpdatedItemIDsType
            var itemIds = data.Items.Select(i => new { itemID = i.ItemId })
                .ToTvp(UpdatedItemIDs, UpdatedItemIDsType);

            context.Database.ExecuteSqlCommand(Exec_GenerateItemUpdateEvents_SP, itemIds);
        }

        private void GenerateItemMessagesForMessageQueueProduct(IconContext context, GenerateItemMessagesCommand data)
        {
            // See app.UpdatedItemIDsType
            var itemIds = data.Items.Select(i => new { itemID = i.ItemId })
                .ToTvp(UpdatedItemIDs, UpdatedItemIDsType);

            context.Database.ExecuteSqlCommand(Exec_GenerateItemUpdateMessages_SP, itemIds);
        }
    }
}
