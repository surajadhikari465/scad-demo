CREATE VIEW [dbo].[v4r_Attributes] 
WITH SCHEMABINDING 
AS 
SELECT [AttributeId]
      ,[DisplayName]
      ,[AttributeName]
      ,[AttributeGroupId]
      ,[HasUniqueValues]
      ,[Description]
      ,[DefaultValue]
      ,[IsRequired]
      ,[SpecialCharactersAllowed]
      ,[TraitCode]
      ,[DataTypeId]
      ,[DisplayOrder]
      ,[InitialValue]
      ,[IncrementBy]
      ,[InitialMax]
      ,[DisplayType]
      ,[MaxLengthAllowed]
      ,[MinimumNumber]
      ,[MaximumNumber]
      ,[NumberOfDecimals]
      ,[IsPickList]
      ,[XmlTraitDescription]
      ,[AttributeGuid]
      ,[IsSpecialTransform]
  FROM [dbo].[Attributes]
GO

CREATE UNIQUE CLUSTERED INDEX [v4r_Attributes_CluInd] ON [dbo].[v4r_Attributes]
(
	[AttributeId] ASC
)
GO