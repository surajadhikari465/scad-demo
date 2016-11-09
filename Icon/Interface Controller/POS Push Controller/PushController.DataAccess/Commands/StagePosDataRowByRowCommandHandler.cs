using Icon.RenewableContext;
using Icon.Framework;
using PushController.DataAccess.Interfaces;
using System;

namespace PushController.DataAccess.Commands
{
    public class StagePosDataRowByRowCommandHandler : ICommandHandler<StagePosDataRowByRowCommand>
    {
        private IRenewableContext<IconContext> context;

        public StagePosDataRowByRowCommandHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public void Execute(StagePosDataRowByRowCommand command)
        {
            context.Context.IRMAPush.Add(command.PosDataEntity);

            try
            {
                context.Context.SaveChanges();
            }
            catch (Exception)
            {
                // Detach the failed message from the context.Context.  Without this step, EF will try to add the same failed message again the next time
                // SaveChanges() is called.
                context.Context.Entry(command.PosDataEntity).State = System.Data.Entity.EntityState.Detached;
                throw;
            }
        }
    }
}
