using System.Data;
using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;

namespace Icon.Web.DataAccess.Commands
{
    public class SetDefaultItemColumnOrderCommandHandler : ICommandHandler<SetDefaultItemColumnOrderCommand>
    {
        private readonly IDbProvider db;

        public SetDefaultItemColumnOrderCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(SetDefaultItemColumnOrderCommand data)
        {
            db.Connection.Execute("dbo.SetDefaultItemColumnDisplayOrder", 
                commandType: CommandType.StoredProcedure, 
                transaction: this.db.Transaction);
        }
    }
}