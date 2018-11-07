CREATE TABLE [dbo].[OrderImportExceptionLog] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [OrderHeader_ID]  INT           NOT NULL,
    [Timestamp]       DATETIME      NOT NULL,
    [Msg]             VARCHAR (MAX) NULL,
    [Store_No]        INT           NULL,
    [SubTeam_No]      INT           NULL,
    [Identifier]      VARCHAR (13)  NULL,
    [Item_ID]         VARCHAR (20)  NULL,
    [ItemDescription] VARCHAR (60)  NULL,
    [PackSize]        VARCHAR (10)  NULL,
    [OrderUnit]       VARCHAR (10)  NULL,
    [Cost]            SMALLMONEY    NULL,
    CONSTRAINT [PK_OrderImportExceptionLog] PRIMARY KEY NONCLUSTERED ([ID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE CLUSTERED INDEX [PK_OrderImportExceptionLog_ID]
    ON [dbo].[OrderImportExceptionLog]([ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_OrderImportExceptionLog_OHID]
    ON [dbo].[OrderImportExceptionLog]([OrderHeader_ID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT DELETE
    ON OBJECT::[dbo].[OrderImportExceptionLog] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[OrderImportExceptionLog] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderImportExceptionLog] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[OrderImportExceptionLog] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[OrderImportExceptionLog] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[OrderImportExceptionLog] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderImportExceptionLog] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[OrderImportExceptionLog] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[OrderImportExceptionLog] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[OrderImportExceptionLog] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderImportExceptionLog] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[OrderImportExceptionLog] TO [IRMAReportsRole]
    AS [dbo];

