CREATE TABLE [dbo].[StoreGroup] (
[storeGroupID] INT  NOT NULL IDENTITY  
, [storeGroupTypeID] INT  NOT NULL  
, [storeGroupName] NVARCHAR(255)  NULL  
, [storeGroupDesc] NVARCHAR(255)  NULL  
)
GO
ALTER TABLE [dbo].[StoreGroup] WITH CHECK ADD CONSTRAINT [StoreGroupType_StoreGroup_FK1] FOREIGN KEY (
[storeGroupTypeID]
)
REFERENCES [dbo].[StoreGroupType] (
[storeGroupTypeID]
)
GO
ALTER TABLE [dbo].[StoreGroup] ADD CONSTRAINT [StoreGroup_PK] PRIMARY KEY CLUSTERED (
[storeGroupID]
)