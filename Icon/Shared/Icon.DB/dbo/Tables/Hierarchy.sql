CREATE TABLE [dbo].[Hierarchy] (
[hierarchyID] INT  NOT NULL IDENTITY  
, [hierarchyName] NVARCHAR(255)  NOT NULL  
)
GO
ALTER TABLE [dbo].[Hierarchy] ADD CONSTRAINT [Hierarchy_PK] PRIMARY KEY CLUSTERED (
[hierarchyID]
)