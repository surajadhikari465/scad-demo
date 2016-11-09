using Icon.ApiController.DataAccess.Commands;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ApiController.DataAccess.Commands
{
    public class UpdateMessageHistoryStatusCommandHandler : ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>
    {
        private IRenewableContext<MammothContext> globalContext;

        public UpdateMessageHistoryStatusCommandHandler(IRenewableContext<MammothContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(UpdateMessageHistoryStatusCommand<MessageHistory> data)
        {
            data.Message.MessageStatusId = data.MessageStatusId;
            data.Message.InProcessBy = null;
            data.Message.ProcessedDate = DateTime.Now;

            globalContext.Context.SaveChanges();
        }
    }
}
