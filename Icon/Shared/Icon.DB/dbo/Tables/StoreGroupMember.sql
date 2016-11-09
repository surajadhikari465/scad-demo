CREATE TABLE [dbo].[StoreGroupMember] (
[storeID] INT  NOT NULL  
, [storeGroupID] INT  NOT NULL  
, [storeGroupTypeID] INT  NOT NULL  
)
GO
ALTER TABLE [dbo].[StoreGroupMember] WITH CHECK ADD CONSTRAINT [Store_StoreGroupMember_FK1] FOREIGN KEY (
[storeID]
)
REFERENCES [dbo].[Store] (
[storeID]
)
GO
ALTER TABLE [dbo].[StoreGroupMember] WITH CHECK ADD CONSTRAINT [StoreGroupType_StoreGroupMember_FK1] FOREIGN KEY (
[storeGroupTypeID]
)
REFERENCES [dbo].[StoreGroupType] (
[storeGroupTypeID]
)
GO
ALTER TABLE [dbo].[StoreGroupMember] WITH CHECK ADD CONSTRAINT [StoreGroup_StoreGroupMember_FK1] FOREIGN KEY (
[storeGroupID]
)
REFERENCES [dbo].[StoreGroup] (
[storeGroupID]
)
GO
ALTER TABLE [dbo].[StoreGroupMember] ADD CONSTRAINT [StoreGroupMember_PK] PRIMARY KEY CLUSTERED (
[storeID]
, [storeGroupID]
)