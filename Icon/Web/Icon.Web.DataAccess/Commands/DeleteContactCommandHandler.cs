using System.Linq;
using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Extensions;

namespace Icon.Web.DataAccess.Commands
{
    class DeleteContactCommandHandler : ICommandHandler<DeleteContactCommand>
    {
        private IDbProvider db;

        public DeleteContactCommandHandler(IDbProvider dbProvider)
        {
            this.db = dbProvider;
        }

        public void Execute(DeleteContactCommand data)
        {
            var tvp = data.ContactIds.Select(x => new { I = x })
            .ToList()
            .ToDataTable(); //TYPE app.IntList

            this.db.Connection.Query<int>(sql: "app.DeleteContact",
                                          param: new { userName = data.UserName, ids = tvp },
                                          transaction: this.db.Transaction,
                                          commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
