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
    public class SaveToMessageHistoryCommandHandler : ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>
    {
        private IRenewableContext<MammothContext> globalContext;

        public SaveToMessageHistoryCommandHandler(IRenewableContext<MammothContext> context)
        {
            globalContext = context;
        }

        public void Execute(SaveToMessageHistoryCommand<MessageHistory> data)
        {
            globalContext.Context.MessageHistories.Add(data.Message);
            globalContext.Context.SaveChanges();
        }
    }
}
