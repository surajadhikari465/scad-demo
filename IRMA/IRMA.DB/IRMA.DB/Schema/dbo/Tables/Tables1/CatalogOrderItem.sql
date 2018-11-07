CREATE TABLE [dbo].[CatalogOrderItem] (
    [CatalogOrderItemID] INT IDENTITY (1, 1) NOT NULL,
    [CatalogOrderID]     INT NULL,
    [CatalogItemID]      INT NULL,
    [SubTeamID]          INT NULL,
    [Quantity]           INT NULL,
    CONSTRAINT [PK_CatalogOrderItem] PRIMARY KEY CLUSTERED ([CatalogOrderItemID] ASC),
    CONSTRAINT [FK_CatalogItem_CatalogOrderItem] FOREIGN KEY ([CatalogItemID]) REFERENCES [dbo].[CatalogItem] ([CatalogItemID]),
    CONSTRAINT [FK_CatalogOrder_CatalogOrderItem] FOREIGN KEY ([CatalogOrderID]) REFERENCES [dbo].[CatalogOrder] ([CatalogOrderID])
);

