delete from StoreGroupMember
delete from store
delete from LocaleTrait
delete from locale
DBCC CHECKIDENT('Locale', RESEED, 0)
delete from LocaleType
DBCC CHECKIDENT('LocaleType', RESEED, 0)
delete from itemhierarchyclass
delete from itemtrait
delete from scancode
delete from app.plumap
delete from item
DBCC CHECKIDENT('item', RESEED, 0)
delete from itemtype
DBCC CHECKIDENT('itemtype', RESEED, 0)
delete from HierarchyClass
DBCC CHECKIDENT('HierarchyClass', RESEED, 0)
delete from hierarchyprototype
delete from Hierarchy
DBCC CHECKIDENT('Hierarchy', RESEED, 0)
delete from ItemType
DBCC CHECKIDENT('ItemType', RESEED, 0)
delete from OrganizationName
delete from NameType
DBCC CHECKIDENT('NameType', RESEED, 0)
delete from Organization
delete from OrganizationType
DBCC CHECKIDENT('OrganizationType', RESEED, 0)
delete from Party
DBCC CHECKIDENT('Party', RESEED, 0)
delete from PartyType
DBCC CHECKIDENT('PartyType', RESEED, 0)
delete from ScanCodeType
DBCC CHECKIDENT('ScanCodeType', RESEED, 0)
delete from StoreGroup
DBCC CHECKIDENT('StoreGroup', RESEED, 0)
delete from StoreGroupType
DBCC CHECKIDENT('StoreGroupType', RESEED, 0)
delete from Trait
DBCC CHECKIDENT('Trait', RESEED, 0)
delete from TraitGroup
DBCC CHECKIDENT('TraitGroup', RESEED, 0)

INSERT [dbo].[LocaleType] ([localeTypeCode], [localeTypeDesc]) VALUES ('CH', N'Chain')
GO
INSERT [dbo].[LocaleType] ([localeTypeCode], [localeTypeDesc]) VALUES ('RG', N'Region')
GO
INSERT [dbo].[LocaleType] ([localeTypeCode], [localeTypeDesc]) VALUES ('MT', N'Metro')
GO
INSERT [dbo].[LocaleType] ([localeTypeCode], [localeTypeDesc]) VALUES ('ST', N'Store')
GO

INSERT [dbo].[OrganizationType] ([orgTypeCode], [orgTypeDesc]) VALUES ('RT', N'Retailer')
GO

INSERT [dbo].[PartyType] ([partyTypeCode], [partyTypeDesc]) VALUES ('ORG', N'Organization')
GO

SET IDENTITY_INSERT [dbo].[Party] ON 
GO
INSERT [dbo].[Party] ([partyID], [partyTypeID]) VALUES (1, 1)
GO
SET IDENTITY_INSERT [dbo].[Party] OFF 
GO

SET IDENTITY_INSERT [dbo].[Organization] ON 
GO
INSERT [dbo].[Organization] ([orgPartyID], [orgTypeID], [parentOrgPartyID], [orgDesc]) VALUES (1, (select orgtypeID from OrganizationType where orgtypecode = 'RT'), NULL, N'Whole Foods')
GO
SET IDENTITY_INSERT [dbo].[Organization] OFF 
GO

SET IDENTITY_INSERT [dbo].[Locale] ON 
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1, 1, N'Whole Foods', '09/20/1980', NULL, 1, NULL)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (2, 1, N'Florida', '09/21/1980', NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (3, 1, N'Mid Atlantic', '09/21/1980', NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (4, 1, N'Mid West', '09/21/1980', NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (5, 1, N'North Atlantic', '09/21/1980', NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (6, 1, N'Northern California', '09/21/1980', NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (7, 1, N'North East', '09/21/1980', NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (8, 1, N'Pacific Northwest', '09/21/1980', NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (9, 1, N'Rocky Mountain', '09/21/1980', NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (10, 1, N'South', '09/21/1980', NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (11, 1, N'Southern Pacific', '09/21/1980', NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (12, 1, N'Southwest', '09/21/1980', NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (13, 1, N'United Kingdom', '09/21/1980', NULL, 2, 1)
GO
SET IDENTITY_INSERT [dbo].[Locale] OFF
GO

INSERT [dbo].[ItemType] ([itemTypeCode], [itemTypeDesc]) VALUES ('RTL', N'Retail Sale')
GO
INSERT [dbo].[ItemType] ([itemTypeCode], [itemTypeDesc]) VALUES ('DEP', N'Deposit')
GO
INSERT [dbo].[ItemType] ([itemTypeCode], [itemTypeDesc]) VALUES ('TAR', N'Tare')
GO

SET IDENTITY_INSERT [dbo].[Hierarchy] ON 
GO
INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName]) VALUES (1, N'Merchandising')
GO
INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName]) VALUES (2, N'Brand')
GO
INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName]) VALUES (3, N'Tax')
GO
INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName]) VALUES (4, N'Browsing')
GO
SET IDENTITY_INSERT [dbo].[Hierarchy] OFF
GO

SET IDENTITY_INSERT [dbo].[ScanCodeType] ON 
GO
INSERT [dbo].[ScanCodeType] ([scanCodeTypeID], [scanCodeTypeDesc]) VALUES (1, N'UPC')
GO
INSERT [dbo].[ScanCodeType] ([scanCodeTypeID], [scanCodeTypeDesc]) VALUES (2, N'POS PLU')
GO
INSERT [dbo].[ScanCodeType] ([scanCodeTypeID], [scanCodeTypeDesc]) VALUES (3, N'Scale PLU')
GO
SET IDENTITY_INSERT [dbo].[ScanCodeType] OFF
GO

INSERT [dbo].[TraitGroup] ([traitGroupCode], [traitGroupDesc]) VALUES ('IA', N'Item Attributes')
GO
INSERT [dbo].[TraitGroup] ([traitGroupCode], [traitGroupDesc]) VALUES ('ILA', N'Item-Locale Attributes')
GO
INSERT [dbo].[TraitGroup] ([traitGroupCode], [traitGroupDesc]) VALUES ('PA', N'Price Attributes')
GO
INSERT [dbo].[TraitGroup] ([traitGroupCode], [traitGroupDesc]) VALUES ('ECA', N'eCommerce Attributes')
GO
INSERT [dbo].[TraitGroup] ([traitGroupCode], [traitGroupDesc]) VALUES ('LT', N'Locale Traits')
GO
INSERT [dbo].[TraitGroup] ([traitGroupCode], [traitGroupDesc]) VALUES ('HT', N'History Traits')
GO

INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('PRD', N'^[a-zA-Z0-9_]*$', N'Product Description', (select traitGroupID from traitgroup where traitGroupCode = 'IA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('POS', N'^[a-zA-Z0-9_]*$', N'POS Description', (select traitGroupID from traitgroup where traitGroupCode = 'IA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('PKG', N'^[0-9]*\.?[0-9]+$', N'Package Unit', (select traitGroupID from traitgroup where traitGroupCode = 'IA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('FSE', N'0|1', N'Food Stamp Eligible', (select traitGroupID from traitgroup where traitGroupCode = 'IA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('SCT', N'0|1', N'POS Scale Tare', (select traitGroupID from traitgroup where traitGroupCode = 'IA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('DPT', N'0|1', N'Department Sale', (select traitGroupID from traitgroup where traitGroupCode = 'IA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('GFT', N'0|1', N'Gift Card', (select traitGroupID from traitgroup where traitGroupCode = 'IA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('RSZ', N'^[0-9]*\.?[0-9]+$', N'Retail Size', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('RUM', N'^[0-9]*\.?[0-9]+$', N'Retail UOM', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('TMD', N'0|1', N'TM Discount Eligible', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('CSD', N'0|1', N'Case Discount Eligible', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('PRH', N'0|1', N'Prohibit Discount', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('AGE', N'^[0-9]*\.?[0-9]+$', N'Age Restrict', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('RCL', N'0|1', N'Recall', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('RES', N'^[0-9]*\.?[0-9]+$', N'Restricted Hours', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('SBW', N'0|1', N'Sold by Weight', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('FCT', N'0|1', N'Force Tare', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('QTY', N'0|1', N'Quantity Required', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('PRQ', N'0|1', N'Price Required', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('QPR', N'^[0-9]*\.?[0-9]+$', N'Quantity Prohibit', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('VV', N'0|1', N'Visual Verify', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('RS', N'0|1', N'Restrict Sale', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('NA', N'0|1', N'Not Authorized For Sale', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('DEL', N'0|1', N'Delete', (select traitGroupID from traitgroup where traitGroupCode = 'ILA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('LOC', N'^[0-9]*\.?[0-9]+$', N'Locale', (select traitGroupID from traitgroup where traitGroupCode = 'PA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('PRC', N'^[0-9]*\.?[0-9]+$', N'Price', (select traitGroupID from traitgroup where traitGroupCode = 'PA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('PM', N'^[0-9]*\.?[0-9]+$', N'Price Multiple', (select traitGroupID from traitgroup where traitGroupCode = 'PA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('ST', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Price Start Date', (select traitGroupID from traitgroup where traitGroupCode = 'PA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('END', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Price End Date', (select traitGroupID from traitgroup where traitGroupCode = 'PA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('SHT', N'^[a-zA-Z0-9_]*$', N'Short Romance', (select traitGroupID from traitgroup where traitGroupCode = 'ECA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('LNG', N'^[a-zA-Z0-9_]*$', N'Long Romance', (select traitGroupID from traitgroup where traitGroupCode = 'ECA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('GF', N'0|1', N'Gluten Free', (select traitGroupID from traitgroup where traitGroupCode = 'ECA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('PBC', N'0|1', N'Premium Body Care', (select traitGroupID from traitgroup where traitGroupCode = 'ECA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('EX', N'0|1', N'Exclusive', (select traitGroupID from traitgroup where traitGroupCode = 'ECA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('WT', N'0|1', N'Whole Trade', (select traitGroupID from traitgroup where traitGroupCode = 'ECA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('NGM', N'0|1', N'Non GMO', (select traitGroupID from traitgroup where traitGroupCode = 'ECA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('HSH', N'0|1', N'HSH', (select traitGroupID from traitgroup where traitGroupCode = 'ECA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('E2', N'0|1', N'E2', (select traitGroupID from traitgroup where traitGroupCode = 'ECA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('VGN', N'0|1', N'Vegan', (select traitGroupID from traitgroup where traitGroupCode = 'ECA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('VEG', N'0|1', N'Vegetarian', (select traitGroupID from traitgroup where traitGroupCode = 'ECA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('KSH', N'0|1', N'Kosher', (select traitGroupID from traitgroup where traitGroupCode = 'ECA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('ECO', N'Baseline|Premium|Ultra-Premium', N'ECO Scale Rating', (select traitGroupID from traitgroup where traitGroupCode = 'ECA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('OG', N'0|1', N'Organic', (select traitGroupID from traitgroup where traitGroupCode = 'ECA'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('ABB', N'^[a-zA-Z0-9_]*$', N'Region Abbreviation', (select traitGroupID from traitgroup where traitGroupCode = 'LT'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('BU', N'^[a-zA-Z0-9_]*$', N'PS Business Unit ID', (select traitGroupID from traitgroup where traitGroupCode = 'LT'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('VER', N'^[0-9]*\.?[0-9]+$', N'ScanCode Version', (select traitGroupID from traitgroup where traitGroupCode = 'HT'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('INS', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Insert Date', (select traitGroupID from traitgroup where traitGroupCode = 'HT'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('MOD', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Modified Date', (select traitGroupID from traitgroup where traitGroupCode = 'HT'))
GO
INSERT [dbo].[Trait] ([traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES ('VAL', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Validation Date', (select traitGroupID from traitgroup where traitGroupCode = 'HT'))
GO

INSERT [dbo].[LocaleTrait] ([traitID], [localeID], [traitValue]) VALUES ((select traitid from trait where traitcode ='ABB'), 2, N'FL')
GO
INSERT [dbo].[LocaleTrait] ([traitID], [localeID], [traitValue]) VALUES ((select traitid from trait where traitcode ='ABB'), 3, N'MA')
GO
INSERT [dbo].[LocaleTrait] ([traitID], [localeID], [traitValue]) VALUES ((select traitid from trait where traitcode ='ABB'), 4, N'MW')
GO
INSERT [dbo].[LocaleTrait] ([traitID], [localeID], [traitValue]) VALUES ((select traitid from trait where traitcode ='ABB'), 5, N'NA')
GO
INSERT [dbo].[LocaleTrait] ([traitID], [localeID], [traitValue]) VALUES ((select traitid from trait where traitcode ='ABB'), 6, N'NC')
GO
INSERT [dbo].[LocaleTrait] ([traitID], [localeID], [traitValue]) VALUES ((select traitid from trait where traitcode ='ABB'), 7, N'NE')
GO
INSERT [dbo].[LocaleTrait] ([traitID], [localeID], [traitValue]) VALUES ((select traitid from trait where traitcode ='ABB'), 8, N'PN')
GO
INSERT [dbo].[LocaleTrait] ([traitID], [localeID], [traitValue]) VALUES ((select traitid from trait where traitcode ='ABB'), 9, N'RM')
GO
INSERT [dbo].[LocaleTrait] ([traitID], [localeID], [traitValue]) VALUES ((select traitid from trait where traitcode ='ABB'), 10, N'SO')
GO
INSERT [dbo].[LocaleTrait] ([traitID], [localeID], [traitValue]) VALUES ((select traitid from trait where traitcode ='ABB'), 11, N'SP')
GO
INSERT [dbo].[LocaleTrait] ([traitID], [localeID], [traitValue]) VALUES ((select traitid from trait where traitcode ='ABB'), 12, N'SW')
GO
INSERT [dbo].[LocaleTrait] ([traitID], [localeID], [traitValue]) VALUES ((select traitid from trait where traitcode ='ABB'), 13, N'UK')
GO

INSERT [dbo].[store]
( [storeid] )
SELECT localeid
FROM   locale
WHERE  localeTypeID = 4
GO