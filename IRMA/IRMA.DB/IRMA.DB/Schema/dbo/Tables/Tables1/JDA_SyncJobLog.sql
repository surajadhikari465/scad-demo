CREATE TABLE [dbo].[JDA_SyncJobLog] (
    [JDA_SyncJobLog_ID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [IsFailed]          BIT            DEFAULT ((0)) NOT NULL,
    [RunDate]           DATETIME       NOT NULL,
    [ErrorMessage]      VARCHAR (4000) NULL,
    [JobName]           VARCHAR (100)  NULL,
    CONSTRAINT [PK_JDA_SyncJobLog] PRIMARY KEY CLUSTERED ([JDA_SyncJobLog_ID] ASC)
);

