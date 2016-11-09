CREATE TABLE [dbo].[UploadLog] (
    [UploadLog_ID]     INT           IDENTITY (1, 1) NOT NULL,
    [EntryType]        VARCHAR (10)  NOT NULL,
    [UploadSession_ID] INT           NOT NULL,
    [UploadRow_ID]     INT           NULL,
    [RetryCount]       INT           NULL,
    [Item_Key]         INT           NULL,
    [Identifier]       VARCHAR (13)  NULL,
    [Timestamp]        DATETIME      NOT NULL,
    [LogText]          VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_UploadLog] PRIMARY KEY CLUSTERED ([UploadLog_ID] ASC)
);

