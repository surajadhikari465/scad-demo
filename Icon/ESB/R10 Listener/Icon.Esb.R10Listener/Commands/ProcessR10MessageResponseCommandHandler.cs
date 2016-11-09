using Icon.Common.DataAccess;
using Icon.Esb.R10Listener.Context;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.R10Listener.Commands
{
    public class ProcessR10MessageResponseCommandHandler : ICommandHandler<ProcessR10MessageResponseCommand>
    {
        private IRenewableContext<IconContext> context;
        private ICommandHandler<AddMessageResponseCommand> addMessageResponseCommandHandler;
        private ICommandHandler<ProcessFailedR10MessageResponseCommand> processFailedR10MessageResponseCommandHandler;

        public ProcessR10MessageResponseCommandHandler(IRenewableContext<IconContext> context,
            ICommandHandler<AddMessageResponseCommand> addMessageResponseCommandHandler,
            ICommandHandler<ProcessFailedR10MessageResponseCommand> processFailedR10MessageResponseCommandHandler)
        {
            this.context = context;
            this.addMessageResponseCommandHandler = addMessageResponseCommandHandler;
            this.processFailedR10MessageResponseCommandHandler = processFailedR10MessageResponseCommandHandler;
        }

        public void Execute(ProcessR10MessageResponseCommand data)
        {
            addMessageResponseCommandHandler.Execute(new AddMessageResponseCommand
                {
                    MessageResponse = data.MessageResponse
                });

            if(!data.MessageResponse.RequestSuccess)
            {
                processFailedR10MessageResponseCommandHandler.Execute(new ProcessFailedR10MessageResponseCommand
                    {
                        MessageResponse = data.MessageResponse
                    });
            }

            context.SaveChanges();
            context.Refresh();
        }
    }
}
