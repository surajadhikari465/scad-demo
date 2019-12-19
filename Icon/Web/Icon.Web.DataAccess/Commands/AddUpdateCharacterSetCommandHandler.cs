using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Web.DataAccess.Extensions;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddUpdateCharacterSetCommandHandler : ICommandHandler<AddUpdateCharacterSetCommand>
    {
        private IDbProvider db;

        public AddUpdateCharacterSetCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(AddUpdateCharacterSetCommand data)
        {
            if (data.CharacterSetModelList != null && data.CharacterSetModelList.Count > 0)
            {
                string sql = @"dbo.AddOrUpdateAttributeCharaterSets";
                int rowCount = this.db.Connection.Execute(
                    sql,
                    new
                    {
                        characterSet = data.CharacterSetModelList.Select(
                            i => new
                            {
                                attributeId = data.AttributeId,
                                characterSetId = i.CharacterSetId,

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
