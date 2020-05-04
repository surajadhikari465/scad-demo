using System.Data;
using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;

namespace Icon.Web.DataAccess.Commands
{
    public class AddMissingColumnsToItemColumnDisplayTableCommandHandler : ICommandHandler<AddMissingColumnsToItemColumnDisplayTableCommand>
    {
        private readonly IDbProvider db;

        public AddMissingColumnsToItemColumnDisplayTableCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(AddMissingColumnsToItemColumnDisplayTableCommand data)
        {
            db.Connection.Execute("dbo.AddMissingColumnsToItemColumnDisplayOrder", 
                commandType: CommandType.StoredProcedure,
                transaction: this.db.Transaction);
        }
    }
}