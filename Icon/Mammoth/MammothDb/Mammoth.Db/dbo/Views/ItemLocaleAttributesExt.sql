CREATE VIEW [dbo].[ItemLocaleAttributesExt]
	AS
SELECT
	[Region]
	,[ItemAttributeLocaleID]
	,[ItemID]
	,[LocaleID]
	,[AttributeID]
	,[AttributeValue]
	,[AddedDate]
	,[ModifiedDate]
FROM [ItemAttributes_Locale_FL_Ext]
UNION ALL
SELECT
	[Region]
	,[ItemAttributeLocaleID]
	,[ItemID]
	,[LocaleID]
	,[AttributeID]
	,[AttributeValue]
	,[AddedDate]
	,[ModifiedDate]
FROM [ItemAttributes_Locale_MA_Ext]
UNION ALL
SELECT
	[Region]
	,[ItemAttributeLocaleID]
	,[ItemID]
	,[LocaleID]
	,[AttributeID]
	,[AttributeValue]
	,[AddedDate]
	,[ModifiedDate]
FROM [ItemAttributes_Locale_MW_Ext]
UNION ALL
SELECT
	[Region]
	,[ItemAttributeLocaleID]
	,[ItemID]
	,[LocaleID]
	,[AttributeID]
	,[AttributeValue]
	,[AddedDate]
	,[ModifiedDate]
FROM [ItemAttributes_Locale_NA_Ext]
UNION ALL
SELECT
	[Region]
	,[ItemAttributeLocaleID]
	,[ItemID]
	,[LocaleID]
	,[AttributeID]
	,[AttributeValue]
	,[AddedDate]
	,[ModifiedDate]
FROM [ItemAttributes_Locale_NC_Ext]
UNION ALL
SELECT
	[Region]
	,[ItemAttributeLocaleID]
	,[ItemID]
	,[LocaleID]
	,[AttributeID]
	,[AttributeValue]
	,[AddedDate]
	,[ModifiedDate]
FROM [ItemAttributes_Locale_NE_Ext]
UNION ALL
SELECT
	[Region]
	,[ItemAttributeLocaleID]
	,[ItemID]
	,[LocaleID]
	,[AttributeID]
	,[AttributeValue]
	,[AddedDate]
	,[ModifiedDate]
FROM [ItemAttributes_Locale_PN_Ext]
UNION ALL
SELECT
	[Region]
	,[ItemAttributeLocaleID]
	,[ItemID]
	,[LocaleID]
	,[AttributeID]
	,[AttributeValue]
	,[AddedDate]
	,[ModifiedDate]
FROM [ItemAttributes_Locale_RM_Ext]
UNION ALL
SELECT
	[Region]
	,[ItemAttributeLocaleID]
	,[ItemID]
	,[LocaleID]
	,[AttributeID]
	,[AttributeValue]
	,[AddedDate]
	,[ModifiedDate]
FROM [ItemAttributes_Locale_SO_Ext]
UNION ALL
SELECT
	[Region]
	,[ItemAttributeLocaleID]
	,[ItemID]
	,[LocaleID]
	,[AttributeID]
	,[AttributeValue]
	,[AddedDate]
	,[ModifiedDate]
FROM [ItemAttributes_Locale_SP_Ext]
UNION ALL
SELECT
	[Region]
	,[ItemAttributeLocaleID]
	,[ItemID]
	,[LocaleID]
	,[AttributeID]
	,[AttributeValue]
	,[AddedDate]
	,[ModifiedDate]
FROM [ItemAttributes_Locale_SW_Ext]
UNION ALL
SELECT
	[Region]
	,[ItemAttributeLocaleID]
	,[ItemID]
	,[LocaleID]
	,[AttributeID]
	,[AttributeValue]
	,[AddedDate]
	,[ModifiedDate]
FROM [ItemAttributes_Locale_TS_Ext]
UNION ALL
SELECT
	[Region]
	,[ItemAttributeLocaleID]
	,[ItemID]
	,[LocaleID]
	,[AttributeID]
	,[AttributeValue]
	,[AddedDate]
	,[ModifiedDate]
FROM [ItemAttributes_Locale_UK_Ext]
GO

GRANT SELECT ON [dbo].[ItemLocaleAttributesExt] TO dds_esl_role
GO