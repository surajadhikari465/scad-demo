using Icon.RenewableContext;
using Icon.Framework;
using PushController.DataAccess.Interfaces;
using System;

namespace PushController.DataAccess.Commands
{
    public class AddItemLocaleMessagesRowByRowCommandHandler : ICommandHandler<AddItemLocaleMessagesRowByRowCommand>
    {
        private IRenewableContext<IconContext> context;

        public AddItemLocaleMessagesRowByRowCommandHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public void Execute(AddItemLocaleMessagesRowByRowCommand command)
        {
            context.Context.MessageQueueItemLocale.Add(command.Message);

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
