namespace Icon.Infor.Listeners.Item.Commands
{
    using System.Linq;

    using Icon.Common;
    using Icon.Common.Context;
    using Icon.Common.DataAccess;
    using Icon.Framework;
    using System;
    using Constants;

    public class GenerateItemMessagesCommandHandler : ICommandHandler<GenerateItemMessagesCommand>
    {
        private const string UpdatedItemIDs = "updatedItemIds";
        private const string UpdatedItemIDsType = "app.UpdatedItemIDsType";
        private const string Exec_GenerateItemUpdateEvents_SP = "exec app.GenerateItemUpdateEvents @updatedItemIDs";
        private const string Exec_GenerateItemUpdateMessages_SP = "exec infor.GenerateItemUpdateMessages @updatedItemIDs";

        private IRenewableContext<IconContext> context;

        public GenerateItemMessagesCommandHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public void Execute(GenerateItemMessagesCommand data)
        {
            var itemsWithoutErrors = data.Items.Where(i => i.ErrorCode == null);
            try
            {
                this.GenerateItemMessagesForEventQueue(data);
                this.GenerateItemMessagesForMessageQueueProduct(data);
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

        private void GenerateItemMessagesForEventQueue(GenerateItemMessagesCommand data)
        {
            // See app.UpdatedItemIDsType
            var itemIds = data.Items.Select(i => new { itemID = i.ItemId })
                .ToTvp(UpdatedItemIDs, UpdatedItemIDsType);

            this.context.Context.Database.ExecuteSqlCommand(Exec_GenerateItemUpdateEvents_SP, itemIds);
        }

        private void GenerateItemMessagesForMessageQueueProduct(GenerateItemMessagesCommand data)
        {
            // See app.UpdatedItemIDsType
            var itemIds = data.Items.Select(i => new { itemID = i.ItemId })
                .ToTvp(UpdatedItemIDs, UpdatedItemIDsType);

            this.context.Context.Database.ExecuteSqlCommand(Exec_GenerateItemUpdateMessages_SP, itemIds);
        }
    }
}
