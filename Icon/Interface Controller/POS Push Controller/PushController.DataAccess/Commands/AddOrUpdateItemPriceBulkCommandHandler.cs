using Icon.RenewableContext;
using Icon.Framework;
using Icon.Logging;
using MoreLinq;
using PushController.DataAccess.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace PushController.DataAccess.Commands
{
    public class AddOrUpdateItemPriceBulkCommandHandler : ICommandHandler<AddOrUpdateItemPriceBulkCommand>
    {
        private ILogger<AddOrUpdateItemPriceBulkCommandHandler> logger;
        private IRenewableContext<IconContext> context;

        public AddOrUpdateItemPriceBulkCommandHandler(
            ILogger<AddOrUpdateItemPriceBulkCommandHandler> logger,
            IRenewableContext<IconContext> context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void Execute(AddOrUpdateItemPriceBulkCommand command)
        {
            if (command.ItemPrices == null || command.ItemPrices.Count == 0)
            {
                logger.Warn("AddOrUpdateItemPriceBulkCommandHandler was called with a null or empty list.  Check execution logic in the calling method.");
                return;
            }

            SqlParameter entitiesParameter = new SqlParameter("ItemPriceEntities", SqlDbType.Structured);
            entitiesParameter.TypeName = "app.ItemPriceEntityType";

            entitiesParameter.Value = command.ItemPrices.ConvertAll(e => new
                {
                    ItemId = e.itemID,
                    LocaleId = e.localeID,
                    ItemPriceTypeId = e.itemPriceTypeID,
                    UomId = e.uomID,
                    CurrencyTypeId = e.currencyTypeID,
                    ItemPriceAmount = e.itemPriceAmt,
                    BreakPointStartQuantity = e.breakPointStartQty,
                    StartDate = e.startDate,
                    EndDate = e.endDate
                }).ToDataTable();

            string sql = "EXEC app.AddOrUpdateItemPriceEntities @ItemPriceEntities";

            context.Context.Database.ExecuteSqlCommand(sql, entitiesParameter);
        }
    }
}
