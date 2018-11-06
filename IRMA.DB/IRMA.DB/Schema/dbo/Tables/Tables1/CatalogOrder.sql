CREATE TABLE [dbo].[CatalogOrder] (
    [CatalogOrderID] INT           IDENTITY (1, 1) NOT NULL,
    [CatalogID]      INT           NULL,
    [ParentID]       INT           NULL,
    [VendorID]       INT           NULL,
    [StoreID]        INT           NULL,
    [UserID]         INT           NULL,
    [FromSubTeamID]  INT           NULL,
    [ToSubTeamID]    INT           NULL,
    [ExpectedDate]   SMALLDATETIME NULL,
    [InsertDate]     SMALLDATETIME CONSTRAINT [DF_CatalogOrder_InsertDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_CatalogOrder] PRIMARY KEY CLUSTERED ([CatalogOrderID] ASC),
    CONSTRAINT [FK_Catalog_CatalogOrder] FOREIGN KEY ([CatalogID]) REFERENCES [dbo].[Catalog] ([CatalogID])
);

