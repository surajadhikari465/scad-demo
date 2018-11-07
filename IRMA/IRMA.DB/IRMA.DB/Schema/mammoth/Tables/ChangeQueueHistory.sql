CREATE TABLE [mammoth].[ChangeQueueHistory] (
    [HistoryId]        INT            IDENTITY (1, 1) NOT NULL,
    [EventTypeId]      INT            NOT NULL,
    [Identifier]       NVARCHAR (13)  NOT NULL,
    [Item_Key]         INT            NOT NULL,
    [Store_No]         INT            NULL,
    [QueueID]          INT            NOT NULL,
    [EventReferenceId] INT            NULL,
    [QueueInsertDate]  DATETIME2 (7)  NOT NULL,
    [InsertDate]       DATETIME2 (7)  NOT NULL,
    [MachineName]      NVARCHAR (50)  NOT NULL,
    [Context]          NVARCHAR (MAX) NOT NULL,
    [ErrorCode]        NVARCHAR (255) NULL,
    [ErrorDetails]     NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([HistoryId] ASC) WITH (FILLFACTOR = 80)
);

