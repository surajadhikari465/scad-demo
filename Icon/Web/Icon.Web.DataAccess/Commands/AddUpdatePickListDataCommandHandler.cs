using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Web.DataAccess.Extensions;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddUpdatePickListDataCommandHandler : ICommandHandler<AddUpdatePickListDataCommand>
    {
        private IDbProvider db;

        public AddUpdatePickListDataCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(AddUpdatePickListDataCommand data)
        {
            if (data.PickListModel != null && data.PickListModel.Count > 0)
            {
                string sql = @"dbo.AddOrUpdatePickListData";
                int rowCount = this.db.Connection.Execute(
                    sql,
                    new
                    {
                        pickList = data.PickListModel.Select(
                            i => new
                            {
                                pickListId = i.PickListId,
                                attributeId = data.AttributeId,
                                pickListValue = i.PickListValue
                            })
                        .ToList()
                        .ToDataTable()
                    },
                    transaction: this.db.Transaction,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}