CREATE VIEW [dbo].[v4r_Item] WITH SCHEMABINDING 
AS
SELECT [ItemId]
      ,[ItemTypeId]
      ,[ItemAttributesJson]
  FROM [dbo].[Item]
  ;
GO

CREATE UNIQUE CLUSTERED INDEX [v4r_Item_CluInd] ON [dbo].[v4r_Item]
(
	[ItemId] ASC
)
GO