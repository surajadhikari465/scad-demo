using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateAttributeCommandHandler : ICommandHandler<UpdateAttributeCommand>
    {
        private IDbProvider db;

        public UpdateAttributeCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(UpdateAttributeCommand data)
        {
            this.db.Connection.Query<int>(sql: "dbo.UpdateAttributes",
                param: new
                {
                    attributeId = data.AttributeModel.AttributeId,
                    displayName = data.AttributeModel.DisplayName,
                    description = data.AttributeModel.Description,
                    maxLengthAllowed = data.AttributeModel.MaxLengthAllowed,
                    minimumNumber = data.AttributeModel.MinimumNumber,
                    maximumNumber = data.AttributeModel.MaximumNumber,
                    numberOfDecimals = data.AttributeModel.NumberOfDecimals,
                    isPickList = data.AttributeModel.IsPickList,
                    specialCharactersAllowed = data.AttributeModel.SpecialCharactersAllowed,
                    isRequired = data.AttributeModel.IsRequired,
                    characterSetRegexPattern = data.AttributeModel.CharacterSetRegexPattern,
                    defaultValue = data.AttributeModel.DefaultValue,
                    isActive = data.AttributeModel.IsActive
                },
                transaction: this.db.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}