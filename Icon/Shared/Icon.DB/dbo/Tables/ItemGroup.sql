CREATE TABLE [dbo].[ItemGroup] (
[itemGroupID] INT  NOT NULL IDENTITY  
, [itemGroupDesc] NVARCHAR(255)  NULL  
)
GO
ALTER TABLE [dbo].[ItemGroup] ADD CONSTRAINT [ItemGroup_PK] PRIMARY KEY CLUSTERED (
[itemGroupID]
)