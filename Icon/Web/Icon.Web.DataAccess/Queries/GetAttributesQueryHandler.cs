using Dapper;
using Icon.Common.DataAccess;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetAttributesQueryHandler : IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>
    {
        private IDbConnection connection;

        public GetAttributesQueryHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<AttributeModel> Search(EmptyQueryParameters<IEnumerable<AttributeModel>> parameters)
        {
            var sql = @"
                DECLARE @attributeGroupId int = (SELECT AttributeGroupId FROM AttributeGroup WHERE AttributeGroupName = 'Global Item'),
                        @nutritionGroupId int = (SELECT AttributeGroupId FROM AttributeGroup WHERE AttributeGroupName = 'Nutrition');
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
                    a.IsReadOnly,
                    a.XmlTraitDescription
                FROM dbo.AttributesView a
                WHERE a.AttributeGroupId <> @nutritionGroupId;

                SELECT
                    pld.PickListId,
                    pld.AttributeId,
                    pld.PickListValue
                FROM dbo.PickListData pld
                JOIN Attributes a ON a.AttributeId = pld.AttributeId
                WHERE a.AttributeGroupId <> @nutritionGroupId;

                SELECT 
                    acs.AttributeCharacterSetId,
                    acs.AttributeId,
                    cs.CharacterSetId,
                    cs.Name,
                    cs.RegEx
                FROM AttributeCharacterSets acs
                JOIN CharacterSets cs ON acs.CharacterSetId = cs.CharacterSetId
                JOIN Attributes a ON a.AttributeId = acs.AttributeId
                WHERE a.AttributeGroupId <> @nutritionGroupId;

                SELECT 
                     COUNT(1) AS ItemCount
	                ,j.[Key] As AttributeName
                FROM Item i
                CROSS APPLY OPENJSON(ItemAttributesJson) j
                WHERE j.[Value] IS NOT NULL
                GROUP BY j.[Key]";

            var results = connection.QueryMultiple(sql);
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
            var attributesCount = results.Read<AttributeItemCountModel>()
               .ToList();

            attributes.ForEach(a =>
            {
                a.PickListData = pickListDataGroups.FirstOrDefault(g => g.Key == a.AttributeId);
                a.CharacterSets = attributeCharacterSetGroups.FirstOrDefault(g => g.Key == a.AttributeId);
                a.ItemCount = attributesCount.FirstOrDefault(g => g.AttributeName == a.AttributeName)?.ItemCount ?? 0;
            });

            return attributes;
        }
    }
}