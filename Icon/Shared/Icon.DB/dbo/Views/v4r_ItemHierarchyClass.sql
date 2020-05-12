CREATE VIEW [dbo].[v4r_ItemHierarchyClass] 
WITH SCHEMABINDING 
AS 
SELECT  [itemID]
      ,[hierarchyClassID]
      ,[localeID]
  FROM [dbo].[ItemHierarchyClass]
GO

CREATE UNIQUE CLUSTERED INDEX [v4r_ItemHierarchyClass_CluInd] ON [dbo].[v4r_ItemHierarchyClass]
(
	[hierarchyClassID] ASC,
	[itemID] ASC
)
GO