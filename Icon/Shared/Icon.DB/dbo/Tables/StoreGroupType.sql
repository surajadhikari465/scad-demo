CREATE TABLE [dbo].[StoreGroupType] (
[storeGroupTypeID] INT  NOT NULL IDENTITY  
, [storeGroupTypeDesc] NVARCHAR(255)  NULL  
)
GO
ALTER TABLE [dbo].[StoreGroupType] ADD CONSTRAINT [StoreGroupType_PK] PRIMARY KEY CLUSTERED (
[storeGroupTypeID]
)