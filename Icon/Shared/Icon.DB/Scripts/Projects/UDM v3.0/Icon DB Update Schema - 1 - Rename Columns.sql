-- Task 3343

--Party
EXEC sp_rename N'[dbo].[PartyType].[partyTypeCode]', N'partyTypeID', N'Column'
EXEC sp_rename N'[dbo].[OrganizationType].[orgTypeCode]', N'orgTypeID', N'Column'
-- keys
EXEC sp_rename N'[dbo].[Party].[partyTypeCode]', N'partyTypeID', N'Column'
EXEC sp_rename N'[dbo].[Organization].[orgTypeCode]', N'orgTypeID', N'Column'

--Location
EXEC sp_rename N'[dbo].[LocaleType].[localeTypeCode]', N'localeTypeID', N'Column'
EXEC sp_rename N'[dbo].[Trait].[traitCode]', N'traitID', N'Column'
EXEC sp_rename N'[dbo].[UOM].[uomCode]', N'uomID', N'Column'
-- keys
EXEC sp_rename N'[dbo].[Locale].[localeTypeCode]', N'localeTypeID', N'Column'
EXEC sp_rename N'[dbo].[LocaleTrait].[traitCode]', N'traitID', N'Column'
EXEC sp_rename N'[dbo].[LocaleTrait].[uomCode]', N'uomID', N'Column'

--Item
EXEC sp_rename N'[dbo].[TraitGroup].[traitGroupCode]', N'traitGroupID', N'Column'
EXEC sp_rename N'[dbo].[ItemType].[itemTypeCode]', N'itemTypeID', N'Column'
-- keys
EXEC sp_rename N'[dbo].[Trait].[traitGroupCode]', N'traitGroupID', N'Column'
EXEC sp_rename N'[dbo].[Item].[itemTypeCode]', N'itemTypeID', N'Column'
EXEC sp_rename N'[dbo].[ItemTrait].[traitCode]', N'traitID', N'Column'
EXEC sp_rename N'[dbo].[ItemTrait].[uomCode]', N'uomID', N'Column'

--Price
EXEC sp_rename N'[dbo].[ItemPriceType].[itemPriceTypeCode]', N'itemPriceTypeID', N'Column'
EXEC sp_rename N'[dbo].[CurrencyType].[currencyTypeCode]', N'currencyTypeID', N'Column'
-- keys
EXEC sp_rename N'[dbo].[ItemPrice].[uomCode]', N'uomID', N'Column'
EXEC sp_rename N'[dbo].[ItemPrice].[itemPriceTypeCode]', N'itemPriceTypeID', N'Column'
EXEC sp_rename N'[dbo].[ItemPrice].[currencyTypeCode]', N'currencyTypeID', N'Column'
