﻿CREATE TABLE [dbo].[Items] (
    [ItemId]               INT            NOT NULL,
    [ScanCode]             NVARCHAR (13)  NOT NULL,
    [ProductDesc]          NVARCHAR (255) NULL,
    [CustomerFriendlyDesc] NVARCHAR (255) NULL,
    [KitchenDesc]          NVARCHAR (255) NULL,
    [BrandName]            NVARCHAR (255) NULL,
    [LargeImageUrl]        NVARCHAR (MAX) NULL,
    [SmallImageUrl]        NVARCHAR (MAX) NULL,
    [InsertDateUtc]        DATETIME2 (7)  CONSTRAINT [DF_Items_InsertDateUtc] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastUpdatedDateUtc]   DATETIME2 (7)  NULL,
    CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED ([ItemId] ASC)
);




