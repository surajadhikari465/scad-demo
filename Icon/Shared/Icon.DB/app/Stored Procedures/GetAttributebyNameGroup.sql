﻿CREATE PROCEDURE [app].[GetAttributebyNameGroup] @attributeName NVARCHAR(255)
	,@attributeGroupName NVARCHAR(25)
AS
BEGIN
	DECLARE @attibutegroupId INT = (
			SELECT attributeGroupId
			FROM AttributeGroup
			WHERE attributeGroupName = @attributeGroupName
			)

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
		,a.LastModifiedDate
		,a.LastModifiedBy
	FROM dbo.AttributesView a
	WHERE a.AttributeName = @attributeName
		AND a.AttributeGroupId = @attibutegroupId;

	SELECT pld.PickListId
		,pld.AttributeId
		,pld.PickListValue
	FROM dbo.PickListData pld
	JOIN Attributes a ON a.AttributeId = pld.AttributeId
	WHERE a.AttributeName = @attributeName
		AND a.AttributeGroupId = @attibutegroupId
	ORDER BY pld.PickListValue;

	SELECT acs.AttributeCharacterSetId
		,acs.AttributeId
		,cs.CharacterSetId
		,cs.Name
		,cs.RegEx
	FROM AttributeCharacterSets acs
	JOIN CharacterSets cs ON acs.CharacterSetId = cs.CharacterSetId
	JOIN Attributes a ON a.AttributeId = acs.AttributeId
	WHERE a.AttributeName = @attributeName
		AND a.AttributeGroupId = @attibutegroupId;
END