using System.Collections.Generic;
using System.Data;
using System.Linq;
//using BulkItemUploadProcessor.Common.Models;
using Dapper;
using Icon.Common.DataAccess;
using Icon.Common.Models;
using AttributeCharacterSetModel = Icon.Common.Models.AttributeCharacterSetModel;
using CharacterSetModel = Icon.Common.Models.CharacterSetModel;
using PickListModel = Icon.Common.Models.PickListModel;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetAttributesQueryHandler : IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>
    {
        private IDbConnection Connection;

        public GetAttributesQueryHandler(IDbConnection connection)
        {
            this.Connection = connection;
        }

        public IEnumerable<AttributeModel> Search(EmptyQueryParameters<IEnumerable<AttributeModel>> parameters)
        {
            var sql = @"
                DECLARE @attributeGroupId int = (SELECT AttributeGroupId FROM AttributeGroup WHERE AttributeGroupName = 'Global Item')
                SELECT 
                    a.AttributeId,
	                a.DisplayName,
	                a.AttributeName,
	                a.AttributeGroupId,
	                a.HasUniqueValues,
	                a.Description,
	                a.DefaultValue,
	                a.IsRequired,
	                a.SpecialCharactersAllowed,
	                a.TraitCode,
	                a.DataTypeId,
	                a.DataTypeName,
	                a.DisplayOrder,
	                a.InitialValue,
	                a.IncrementBy,
	                a.InitialMax,
	                a.DisplayType,
	                a.MaxLengthAllowed,
	                a.MinimumNumber,
                    a.MaximumNumber,
                    a.NumberOfDecimals,
	                a.IsPickList,
	                a.GridColumnWidth,
                    a.CharacterSetRegexPattern,
	                a.IsReadOnly
                FROM dbo.AttributesView a

                SELECT 
                    PickListId,
                    AttributeId,
                    PickListValue
                FROM dbo.PickListData

                SELECT 
                    acs.AttributeCharacterSetId,
                    acs.AttributeId,
                    cs.CharacterSetId,
                    cs.Name,
                    cs.RegEx
                FROM AttributeCharacterSets acs
                JOIN CharacterSets cs ON acs.CharacterSetId = cs.CharacterSetId";

            var results = Connection.QueryMultiple(sql);
            var attributes = results.Read<AttributeModel>()
                .ToList();
            var pickListDataGroups = results.Read<PickListModel>()
                .GroupBy(p => p.AttributeId);
            var attributeCharacterSetGroups = results.Read<AttributeCharacterSetModel, CharacterSetModel, AttributeCharacterSetModel>(
                    (attributeCharacterSetModel, characterSetModel) =>
                    {
                        attributeCharacterSetModel.CharacterSetModel = characterSetModel;
                        return attributeCharacterSetModel;
                    },
                    "CharacterSetId")
                .GroupBy(c => c.AttributeId);

            attributes.ForEach(a =>
            {
                a.PickListData = pickListDataGroups.FirstOrDefault(g => g.Key == a.AttributeId);
                a.CharacterSets = attributeCharacterSetGroups.FirstOrDefault(g => g.Key == a.AttributeId);
            });

            return attributes;
        }

      
    }
}