CREATE TYPE [dbo].[ItemAddOrUpdateType] AS TABLE
(
	[ItemId]               INT            NOT NULL,
    [ScanCode]             NVARCHAR (13)  NOT NULL,
    [ProductDesc]          NVARCHAR (255) NULL,
    [CustomerFriendlyDesc] NVARCHAR (255) NULL,
    [KitchenDesc]          NVARCHAR (255) NULL,
    [BrandName]            NVARCHAR (255) NULL,
    [LargeImageUrl]        NVARCHAR (MAX) NULL,
    [SmallImageUrl]        NVARCHAR (MAX) NULL
)
GO

