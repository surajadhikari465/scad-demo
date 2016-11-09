
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Party]') AND type in (N'U'))
    DROP TABLE [dbo].[Party]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Trait]') AND type in (N'U'))
    DROP TABLE [dbo].[Trait]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddressType]') AND type in (N'U'))
    DROP TABLE [dbo].[AddressType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HierarchyPrototype]') AND type in (N'U'))
    DROP TABLE [dbo].[HierarchyPrototype]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Timezone]') AND type in (N'U'))
    DROP TABLE [dbo].[Timezone]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PartyType]') AND type in (N'U'))
    DROP TABLE [dbo].[PartyType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LocaleType]') AND type in (N'U'))
    DROP TABLE [dbo].[LocaleType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UOM]') AND type in (N'U'))
    DROP TABLE [dbo].[UOM]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PhysicalAddress]') AND type in (N'U'))
    DROP TABLE [dbo].[PhysicalAddress]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND type in (N'U'))
    DROP TABLE [dbo].[Item]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemPriceType]') AND type in (N'U'))
    DROP TABLE [dbo].[ItemPriceType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Store]') AND type in (N'U'))
    DROP TABLE [dbo].[Store]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemTrait]') AND type in (N'U'))
    DROP TABLE [dbo].[ItemTrait]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PostalCode]') AND type in (N'U'))
    DROP TABLE [dbo].[PostalCode]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Organization]') AND type in (N'U'))
    DROP TABLE [dbo].[Organization]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoreGroupMember]') AND type in (N'U'))
    DROP TABLE [dbo].[StoreGroupMember]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoreGroupType]') AND type in (N'U'))
    DROP TABLE [dbo].[StoreGroupType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrganizationType]') AND type in (N'U'))
    DROP TABLE [dbo].[OrganizationType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemGroup]') AND type in (N'U'))
    DROP TABLE [dbo].[ItemGroup]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoreGroup]') AND type in (N'U'))
    DROP TABLE [dbo].[StoreGroup]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemGroupMember]') AND type in (N'U'))
    DROP TABLE [dbo].[ItemGroupMember]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HierarchyClass]') AND type in (N'U'))
    DROP TABLE [dbo].[HierarchyClass]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Country]') AND type in (N'U'))
    DROP TABLE [dbo].[Country]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CurrencyType]') AND type in (N'U'))
    DROP TABLE [dbo].[CurrencyType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ScanCode]') AND type in (N'U'))
    DROP TABLE [dbo].[ScanCode]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrganizationName]') AND type in (N'U'))
    DROP TABLE [dbo].[OrganizationName]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Territory]') AND type in (N'U'))
    DROP TABLE [dbo].[Territory]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LocaleTrait]') AND type in (N'U'))
    DROP TABLE [dbo].[LocaleTrait]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LocaleAddress]') AND type in (N'U'))
    DROP TABLE [dbo].[LocaleAddress]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NameType]') AND type in (N'U'))
    DROP TABLE [dbo].[NameType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ScanCodeType]') AND type in (N'U'))
    DROP TABLE [dbo].[ScanCodeType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemHierarchyClass]') AND type in (N'U'))
    DROP TABLE [dbo].[ItemHierarchyClass]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[City]') AND type in (N'U'))
    DROP TABLE [dbo].[City]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemType]') AND type in (N'U'))
    DROP TABLE [dbo].[ItemType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Hierarchy]') AND type in (N'U'))
    DROP TABLE [dbo].[Hierarchy]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddressUsage]') AND type in (N'U'))
    DROP TABLE [dbo].[AddressUsage]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemPrice]') AND type in (N'U'))
    DROP TABLE [dbo].[ItemPrice]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Locale]') AND type in (N'U'))
    DROP TABLE [dbo].[Locale]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemLink]') AND type in (N'U'))
    DROP TABLE [dbo].[ItemLink]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[County]') AND type in (N'U'))
    DROP TABLE [dbo].[County]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TraitGroup]') AND type in (N'U'))
    DROP TABLE [dbo].[TraitGroup]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Address]') AND type in (N'U'))
    DROP TABLE [dbo].[Address]
GO
