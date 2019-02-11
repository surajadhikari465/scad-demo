using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;

namespace MammothWebApi.DataAccess.Commands
{
    public class AddOrUpdateItemLocaleCommandHandler : ICommandHandler<AddOrUpdateItemLocaleCommand>
    {
        readonly IDbProvider db;

        public AddOrUpdateItemLocaleCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(AddOrUpdateItemLocaleCommand data)
        {
					this.db.Connection.Execute(sql: "dbo.AddOrUpdateItemAttributesLocaleRegion",
					                           param: new { region = data.Region, dateStamp = data.Timestamp, transactionId = data.TransactionId },
					                           transaction: this.db.Transaction,
					                           commandType: System.Data.CommandType.StoredProcedure);

        }
    }
}