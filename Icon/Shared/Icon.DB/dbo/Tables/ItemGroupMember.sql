CREATE TABLE [dbo].[ItemGroupMember] (
[itemID] INT  NOT NULL  
, [itemGroupID] INT  NOT NULL  
, [localeID] INT  NULL  
)
GO
ALTER TABLE [dbo].[ItemGroupMember] WITH CHECK ADD CONSTRAINT [Locale_ItemGroupMember_FK1] FOREIGN KEY (
[localeID]
)
REFERENCES [dbo].[Locale] (
[localeID]
)
GO
ALTER TABLE [dbo].[ItemGroupMember] WITH CHECK ADD CONSTRAINT [Item_ItemGroupMember_FK1] FOREIGN KEY (
[itemID]
)
REFERENCES [dbo].[Item] (
[itemID]
)
GO
ALTER TABLE [dbo].[ItemGroupMember] WITH CHECK ADD CONSTRAINT [ItemGroup_ItemGroupMember_FK1] FOREIGN KEY (
[itemGroupID]
)
REFERENCES [dbo].[ItemGroup] (
[itemGroupID]
)
GO
ALTER TABLE [dbo].[ItemGroupMember] ADD CONSTRAINT [ItemGroupMember_PK] PRIMARY KEY CLUSTERED (
[itemID]
, [itemGroupID]
)