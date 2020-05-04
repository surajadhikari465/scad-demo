using System.Data;
using System.Linq;
using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using MoreLinq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateItemColumnOrderCommandHandler : ICommandHandler<UpdateItemColumnOrderCommand>
    {
        private readonly IDbProvider db;

        public UpdateItemColumnOrderCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(UpdateItemColumnOrderCommand command)
        {

            var tvp = command.DisplayData
                .Select(d => new
                {
                    d.ColumnType, 
                    d.ReferenceId, 
                    d.DisplayOrder
                })
                .ToDataTable().AsTableValuedParameter("dbo.ItemColumnDisplayOrderType");

            db.Connection.Execute("dbo.UpdateItemColumnDisplayOrder", new { DisplayOrderData = tvp },
                commandType: CommandType.StoredProcedure,
                transaction: this.db.Transaction);
        }
    }
}