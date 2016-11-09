using Dapper;
using System.Linq;
using System.Data;

namespace Vim.Common.DataAccess.Commands
{
    public class UpdateEventQueueInProcessCommandHandler : ICommandHandler<UpdateEventQueueInProcessCommand>
    {
        private IDbProvider dbProvider;

        public UpdateEventQueueInProcessCommandHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public void Execute(UpdateEventQueueInProcessCommand data)
        {
            dbProvider.Connection.Execute(
                @"vim.UpdateEventQueueAsInProcess",
                new
                {
                    Instance = data.Instance,
                    NumberOfRows = data.MaxNumberOfRowsToMark,
                    EventTypeIds = data.EventTypeIds.Select(i => new { I = i }).ToList().ToDataTable().AsTableValuedParameter("app.IntList")
                },
                transaction: dbProvider.Transaction,
                commandType: CommandType.StoredProcedure);
        }
    }
}
