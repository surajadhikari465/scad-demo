using System.Linq;
using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Extensions;

namespace Icon.Web.DataAccess.Commands
{
    public class AddUpdateContactTypeCommandHandler : ICommandHandler<AddUpdateContactTypeCommand>
    {
        private IDbProvider db;

        public AddUpdateContactTypeCommandHandler(IDbProvider dbProvider)
        {
            this.db = dbProvider;
        }

        public void Execute(AddUpdateContactTypeCommand data)
        {
            var tvp = data.ContactTypes.Select(x => new
                    {
                        x.ContactTypeId,
                        x.ContactTypeName,
	                    x.Archived,
                    })
            .ToList()
            .ToDataTable(); //TYPE app.ContactTypeInputType

            this.db.Connection.Query<int>(sql: "app.AddUpdateContactType",
                                          param: new { contact = tvp },
                                          transaction: this.db.Transaction,
                                          commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
