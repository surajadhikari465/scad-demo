using Dapper;

namespace Vim.Common.DataAccess.Commands
{
    public class DeleteEventQueueCommandHandler : ICommandHandler<DeleteEventQueueCommand>
    {
        private IDbProvider dbProvider;

        public DeleteEventQueueCommandHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public void Execute(DeleteEventQueueCommand data)
        {
            dbProvider.Connection.Execute(
                @"delete vim.EventQueue
                  where QueueId in @EventQueueIds",
                new
                {
                    EventQueueIds = data.QueueIds
                },
                dbProvider.Transaction);
        }
    }
}

