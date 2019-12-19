CREATE VIEW dbo.AttributesView
AS
SELECT a.AttributeId,
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
	dt.DataType AS DataTypeName,
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
	awc.GridColumnWidth,
	awc.CharacterSetRegexPattern,
	awc.IsReadOnly,
	a.XmlTraitDescription
FROM dbo.Attributes a
INNER JOIN dbo.DataType dt ON a.DataTypeId = dt.DataTypeId
INNER JOIN dbo.AttributesWebConfiguration awc ON a.AttributeId = awc.AttributeId