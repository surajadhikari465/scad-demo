using System.Linq;
using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Extensions;

namespace Icon.Web.DataAccess.Commands
{
    public class DeleteContactTypeCommandHandler : ICommandHandler<DeleteContactTypeCommand>
    {
        private IDbProvider db;

        public DeleteContactTypeCommandHandler(IDbProvider dbProvider)
        {
            this.db = dbProvider;
        }

        public void Execute(DeleteContactTypeCommand data)
        {
            var tvp = data.ContactTypeIds.Select(x => new { I = x })
            .ToList()
            .ToDataTable(); //TYPE app.IntList

            this.db.Connection.Query<int>(sql: "app.DeleteContactType",
                                          param: new { ids = tvp },
                                          transaction: this.db.Transaction,
                                          commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}