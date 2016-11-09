using Icon.RenewableContext;
using Icon.Framework;
using PushController.DataAccess.Interfaces;
using System;
using System.Linq;

namespace PushController.DataAccess.Commands
{
    public class AddOrUpdateItemPriceRowByRowCommandHandler : ICommandHandler<AddOrUpdateItemPriceRowByRowCommand>
    {
        private IRenewableContext<IconContext> context;

        public AddOrUpdateItemPriceRowByRowCommandHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public void Execute(AddOrUpdateItemPriceRowByRowCommand command)
        {
            var existingItemPrice = context.Context.ItemPrice.SingleOrDefault(ip =>
                ip.itemID == command.ItemPriceEntity.itemID &&
                ip.localeID == command.ItemPriceEntity.localeID &&
                ip.itemPriceTypeID == command.ItemPriceEntity.itemPriceTypeID);

            if (existingItemPrice == null)
            {
                var newItemPrice = new ItemPrice
                {
                    itemID = command.ItemPriceEntity.itemID,
                    localeID = command.ItemPriceEntity.localeID,
                    itemPriceTypeID = command.ItemPriceEntity.itemPriceTypeID,
                    itemPriceAmt = command.ItemPriceEntity.itemPriceAmt,
                    uomID = command.ItemPriceEntity.uomID,
                    currencyTypeID = command.ItemPriceEntity.currencyTypeID,
                    breakPointStartQty = command.ItemPriceEntity.breakPointStartQty,
                    startDate = command.ItemPriceEntity.startDate,
                    endDate = command.ItemPriceEntity.endDate
                };

                context.Context.ItemPrice.Add(newItemPrice);

                try
                {
                    context.Context.SaveChanges();
                }
                catch (Exception)
                {
                    // Detach the failed entity from the context.Context.  Without this step, EF will try to add the same failed message again the next time
                    // SaveChanges() is called.
                    context.Context.Entry(newItemPrice).State = System.Data.Entity.EntityState.Detached;
                    throw;
                }
            }
            else
            {
                existingItemPrice.itemPriceAmt = command.ItemPriceEntity.itemPriceAmt;
                existingItemPrice.uomID = command.ItemPriceEntity.uomID;
                existingItemPrice.currencyTypeID = command.ItemPriceEntity.currencyTypeID;
                existingItemPrice.breakPointStartQty = command.ItemPriceEntity.breakPointStartQty;
                existingItemPrice.startDate = command.ItemPriceEntity.startDate;
                existingItemPrice.endDate = command.ItemPriceEntity.endDate;

                try
                {
                    context.Context.SaveChanges();
                }
                catch (Exception)
                {
                    // Detach the failed entity from the context.Context.  Without this step, EF will try to add the same failed message again the next time
                    // SaveChanges() is called.
                    context.Context.Entry(existingItemPrice).State = System.Data.Entity.EntityState.Detached;
                    throw;
                }
            }
        }
    }
}
