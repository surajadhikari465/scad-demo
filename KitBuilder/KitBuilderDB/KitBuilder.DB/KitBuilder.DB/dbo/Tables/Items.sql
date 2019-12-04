CREATE TABLE [dbo].[Items] (
	[ItemId]               INT            NOT NULL,
    [ScanCode]             NVARCHAR (13)  NOT NULL,
    [ProductDesc]          NVARCHAR (255) NULL,
    [CustomerFriendlyDesc] NVARCHAR (255) NULL,
    [KitchenDesc]          NVARCHAR (255) NULL,
    [BrandName]            NVARCHAR (255) NULL,
    [ImageUrl]             NVARCHAR (MAX) NULL,
    [InsertDateUtc]        DATETIME2 (7)  CONSTRAINT [DF_Items_InsertDateUtc] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastUpdatedDateUtc]   DATETIME2 (7)  NULL,
	[FlexibleText]         NVARCHAR (255) NULL,
    [PosDesc]              NVARCHAR (255) NULL,
    [RetailSize]           NVARCHAR(255) NULL, 
    [RetailUOM]            NVARCHAR(255) NULL, 
    CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED ([ItemId] ASC)
);
