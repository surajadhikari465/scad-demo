CREATE TYPE [dbo].[ItemAddOrUpdateType] AS TABLE
(
	[ItemId]               INT            NOT NULL,
    [ScanCode]             NVARCHAR (13)  NOT NULL,
    [ProductDesc]          NVARCHAR (255) NULL,
    [CustomerFriendlyDesc] NVARCHAR (255) NULL,
    [KitchenDesc]          NVARCHAR (255) NULL,
    [BrandName]            NVARCHAR (255) NULL,
    [ImageUrl]			   NVARCHAR (255) NULL,
	[FlexibleText]         NVARCHAR (255) NULL,
    [PosDesc]              NVARCHAR (255) NULL
)
GO

