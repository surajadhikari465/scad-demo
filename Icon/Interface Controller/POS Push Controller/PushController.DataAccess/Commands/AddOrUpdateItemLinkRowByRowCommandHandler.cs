using Icon.RenewableContext;
using Icon.Framework;
using PushController.DataAccess.Interfaces;
using System;
using System.Linq;

namespace PushController.DataAccess.Commands
{
    public class AddOrUpdateItemLinkRowByRowCommandHandler : ICommandHandler<AddOrUpdateItemLinkRowByRowCommand>
    {
        private IRenewableContext<IconContext> context;

        public AddOrUpdateItemLinkRowByRowCommandHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public void Execute(AddOrUpdateItemLinkRowByRowCommand command)
        {

            var existingItemLink = context.Context.ItemLink.SingleOrDefault(il =>
                il.parentItemID == command.ItemLinkEntity.parentItemID &&
                il.childItemID == command.ItemLinkEntity.childItemID &&
                il.localeID == command.ItemLinkEntity.localeID);

            if (existingItemLink == null)
            {
                var newItemLink = new ItemLink
                {
                    parentItemID = command.ItemLinkEntity.parentItemID,
                    childItemID = command.ItemLinkEntity.childItemID,
                    localeID = command.ItemLinkEntity.localeID,
                };

                context.Context.ItemLink.Add(newItemLink);

                try
                {
                    context.Context.SaveChanges();
                }
                catch (Exception)
                {
                    // Detach the failed entity from the context.Context.  Without this step, EF will try to add the same failed message again the next time
                    // SaveChanges() is called.
                    context.Context.Entry(newItemLink).State = System.Data.Entity.EntityState.Detached;
                    throw;
                }
            }
            else
            {
                existingItemLink.parentItemID = command.ItemLinkEntity.parentItemID;
                existingItemLink.childItemID = command.ItemLinkEntity.childItemID;

                try
                {
                    context.Context.SaveChanges();
                }
                catch (Exception)
                {
                    // Detach the failed entity from the context.Context.  Without this step, EF will try to add the same failed message again the next time
                    // SaveChanges() is called.
                    context.Context.Entry(existingItemLink).State = System.Data.Entity.EntityState.Detached;
                    throw;
                }
            }
        }
    }
}
