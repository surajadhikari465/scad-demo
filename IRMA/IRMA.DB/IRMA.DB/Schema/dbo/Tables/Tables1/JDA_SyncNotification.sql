CREATE TABLE [dbo].[JDA_SyncNotification] (
    [JDA_SyncNotification_ID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [EventKey]                VARCHAR (100)  NULL,
    [RecipientEmailAddresses] VARCHAR (2000) NULL,
    [EmailSubject]            VARCHAR (2000) NULL,
    [EmailBody]               VARCHAR (2000) NULL,
    CONSTRAINT [PK_JDA_SyncNotification] PRIMARY KEY CLUSTERED ([JDA_SyncNotification_ID] ASC)
);

