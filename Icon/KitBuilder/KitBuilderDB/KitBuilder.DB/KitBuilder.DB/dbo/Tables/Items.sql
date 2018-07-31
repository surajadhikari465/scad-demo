CREATE TABLE [dbo].[Items]
(
	[ItemId] INT NOT NULL PRIMARY KEY, 
    [ScanCode] NVARCHAR(13) NOT NULL, 
    [ProductDesc] NVARCHAR(255) NULL, 
    [CustomerFriendlyDesc] NVARCHAR(255) NULL, 
    [KitchenDesc] NVARCHAR(255) NULL, 
    [BrandName] NVARCHAR(255) NULL, 
    [LargeImageUrl] NVARCHAR(MAX) NULL, 
    [SmallImageUrl] NVARCHAR(MAX) NULL, 
    [InsertDate] DATETIME2 NOT NULL DEFAULT GetDate()
    
)
