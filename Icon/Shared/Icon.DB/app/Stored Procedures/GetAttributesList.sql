CREATE PROCEDURE [app].[GetAttributesList] 
@includeItemCount BIT = 0
AS
BEGIN
	DECLARE @attributeGroupId INT = (
			SELECT AttributeGroupId
			FROM AttributeGroup
			WHERE AttributeGroupName = 'Global Item'
			)
		,@nutritionGroupId INT = (
			SELECT AttributeGroupId
			FROM AttributeGroup
			WHERE AttributeGroupName = 'Nutrition'
			);

	SELECT a.AttributeId
		,a.DisplayName
		,a.AttributeName
		,a.AttributeGroupId
		,a.HasUniqueValues
		,a.Description
		,a.DefaultValue
		,a.IsRequired
		,a.SpecialCharactersAllowed
		,a.TraitCode
		,a.DataTypeId
		,a.DataTypeName
		,a.DisplayOrder
		,a.InitialValue
		,a.IncrementBy
		,a.InitialMax
		,a.DisplayType
		,a.MaxLengthAllowed
		,a.MinimumNumber
		,a.MaximumNumber
		,a.NumberOfDecimals
		,a.IsPickList
		,a.GridColumnWidth
		,a.CharacterSetRegexPattern
		,a.IsReadOnly
		,a.XmlTraitDescription
		,a.IsActive
	FROM dbo.AttributesView a
	WHERE a.AttributeGroupId <> @nutritionGroupId;

	SELECT pld.PickListId
		,pld.AttributeId
		,pld.PickListValue
	FROM dbo.PickListData pld
	JOIN Attributes a ON a.AttributeId = pld.AttributeId
	WHERE a.AttributeGroupId <> @nutritionGroupId
	ORDER BY pld.PickListValue;

	SELECT acs.AttributeCharacterSetId
		,acs.AttributeId
		,cs.CharacterSetId
		,cs.Name
		,cs.RegEx
	FROM AttributeCharacterSets acs
	JOIN CharacterSets cs ON acs.CharacterSetId = cs.CharacterSetId
	JOIN Attributes a ON a.AttributeId = acs.AttributeId
	WHERE a.AttributeGroupId <> @nutritionGroupId;

	IF (IsNull(@includeItemCount, 0) = 1)
	BEGIN
		SELECT COUNT(1) AS ItemCount
			,j.[Key] AS AttributeName
		FROM Item i WITH (NOLOCK)
		CROSS APPLY OPENJSON(ItemAttributesJson) j
		WHERE j.[Value] IS NOT NULL
		GROUP BY j.[Key];
	END
END