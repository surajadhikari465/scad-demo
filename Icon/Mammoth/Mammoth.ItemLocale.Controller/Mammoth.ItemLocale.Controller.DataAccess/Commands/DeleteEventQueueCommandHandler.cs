using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;

namespace Mammoth.ItemLocale.Controller.DataAccess.Commands
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
                @"delete mammoth.ItemLocaleChangeQueue
                  where QueueId in @EventQueueIds",
                new
                {
                    EventQueueIds = data.QueueIds
                },
                dbProvider.Transaction);
        }
    }
}
