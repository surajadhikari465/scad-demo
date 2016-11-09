using Icon.RenewableContext;
using Icon.Framework;
using PushController.DataAccess.Interfaces;
using System;

namespace PushController.DataAccess.Commands
{
    public class AddPriceMessagesRowByRowCommandHandler : ICommandHandler<AddPriceMessagesRowByRowCommand>
    {
        private IRenewableContext<IconContext> context;

        public AddPriceMessagesRowByRowCommandHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public void Execute(AddPriceMessagesRowByRowCommand command)
        {
            context.Context.MessageQueuePrice.Add(command.Message);

            try
            {
                context.Context.SaveChanges();
            }
            catch (Exception)
            {
                // Detach the failed message from the context.Context.  Without this step, EF will try to add the same failed message again the next time
                // SaveChanges() is called.
                context.Context.Entry(command.Message).State = System.Data.Entity.EntityState.Detached;
                throw;
            }
        }
    }
}
