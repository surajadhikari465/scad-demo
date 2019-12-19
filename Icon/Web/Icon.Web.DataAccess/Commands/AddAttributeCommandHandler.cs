using Icon.Common.DataAccess;
using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddAttributeCommandHandler : ICommandHandler<AddAttributeCommand>
    {
        private IDbProvider db;

        public AddAttributeCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(AddAttributeCommand data)
        {
            int attributeId = (this.db.Connection.Query<int>(sql: "dbo.AddAttributes",
                                           param: new
                                           {
                                               displayName = data.AttributeModel.DisplayName,
                                               attributeName = data.AttributeModel.AttributeName,
                                               description = data.AttributeModel.Description,
                                               traitCode = data.AttributeModel.TraitCode,
                                               dataTypeId = data.AttributeModel.DataTypeId,
                                               maxLengthAllowed = data.AttributeModel.MaxLengthAllowed,
                                               minimumNumber = data.AttributeModel.MinimumNumber,
                                               maximumNumber = data.AttributeModel.MaximumNumber,
                                               numberOfDecimals = data.AttributeModel.NumberOfDecimals,
                                               isPickList = data.AttributeModel.IsPickList,
                                               attributeId = data.AttributeModel.AttributeId,
                                               specialCharactersAllowed = data.AttributeModel.SpecialCharactersAllowed,
                                               isRequired = data.AttributeModel.IsRequired,
                                               characterSetRegexPattern = data.AttributeModel.CharacterSetRegexPattern
                                           },
                                           transaction: this.db.Transaction,
                                           commandType: System.Data.CommandType.StoredProcedure)).First();

            data.AttributeModel.AttributeId = attributeId;
        }
    }
}