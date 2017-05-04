using Icon.ApiController.DataAccess.Commands;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Mammoth.Framework;
using System;

namespace Mammoth.ApiController.DataAccess.Commands
{
    public class UpdateMessageHistoryStatusCommandHandler : ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>
    {
        private IDbContextFactory<MammothContext> mammothContextFactory;

        public UpdateMessageHistoryStatusCommandHandler(IDbContextFactory<MammothContext> mammothContextFactory)
        {
            this.mammothContextFactory = mammothContextFactory;
        }

        public void Execute(UpdateMessageHistoryStatusCommand<MessageHistory> data)
        {
            using (var context = mammothContextFactory.CreateContext())
            {
                context.MessageHistories.Attach(data.Message);

                data.Message.MessageStatusId = data.MessageStatusId;
                data.Message.InProcessBy = null;
                data.Message.ProcessedDate = DateTime.Now;

                context.SaveChanges();
            }
        }
    }
}
