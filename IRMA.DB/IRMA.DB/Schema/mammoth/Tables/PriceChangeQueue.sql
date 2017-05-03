CREATE TABLE [mammoth].[PriceChangeQueue] (
    [QueueID]           INT           IDENTITY (1, 1) NOT NULL,
    [Item_Key]          INT           NOT NULL,
    [Store_No]          INT           NULL,
    [Identifier]        VARCHAR (13)  NOT NULL,
    [EventTypeID]       INT           NOT NULL,
    [EventReferenceID]  INT           NULL,
    [InsertDate]        DATETIME2 (7) CONSTRAINT [DF_PriceChangeQueue_InsertDate] DEFAULT (sysdatetime()) NOT NULL,
    [ProcessFailedDate] DATETIME2 (7) NULL,
    [InProcessBy]       INT           NULL,
    CONSTRAINT [PK_PriceChangeQueue_QueueID] PRIMARY KEY CLUSTERED ([QueueID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_PriceChangeQueue_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_PriceChangeQueue_ItemChangeEventType] FOREIGN KEY ([EventTypeID]) REFERENCES [mammoth].[ItemChangeEventType] ([EventTypeID]),
    CONSTRAINT [FK_PriceChangeQueue_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);


GO
GRANT DELETE
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [IRSUser]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [IConInterface]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [IConInterface]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [MammothRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [MammothRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [MammothRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[mammoth].[PriceChangeQueue] TO [MammothRole]
    AS [dbo];

