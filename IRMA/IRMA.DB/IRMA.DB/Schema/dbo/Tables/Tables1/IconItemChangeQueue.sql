CREATE TABLE [dbo].[IconItemChangeQueue] (
    [QID]               INT           IDENTITY (1, 1) NOT NULL,
    [Item_Key]          INT           NOT NULL,
    [Identifier]        VARCHAR (13)  NOT NULL,
    [ItemChgTypeID]     TINYINT       NOT NULL,
    [InsertDate]        DATETIME2 (7) CONSTRAINT [DF_IconItemChangeQueue_InsertDate] DEFAULT (getdate()) NOT NULL,
    [ProcessFailedDate] DATETIME2 (7) NULL,
    [InProcessBy]       VARCHAR (30)  NULL,
    CONSTRAINT [PK_IconItemChangeQueue_QID] PRIMARY KEY CLUSTERED ([QID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_IconItemChangeQueue_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_IconItemChangeQueue_ItemChgType] FOREIGN KEY ([ItemChgTypeID]) REFERENCES [dbo].[ItemChgType] ([ItemChgTypeID])
);


GO
CREATE NONCLUSTERED INDEX [IX_IconItemChangeQueue_ItemKey]
    ON [dbo].[IconItemChangeQueue]([Item_Key] ASC, [Identifier] ASC, [QID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT DELETE
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IConInterface]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[IconItemChangeQueue] TO [IConInterface]
    AS [dbo];

