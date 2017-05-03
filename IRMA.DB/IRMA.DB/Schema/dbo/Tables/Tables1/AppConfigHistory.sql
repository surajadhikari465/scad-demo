CREATE TABLE [dbo].[AppConfigHistory] (
    [ID]              INT                                            IDENTITY (1, 1) NOT NULL,
    [Action]          VARCHAR (50)                                   NOT NULL,
    [EnvironmentID]   UNIQUEIDENTIFIER                               NULL,
    [EnvironmentName] VARCHAR (50)                                   NULL,
    [ApplicationID]   UNIQUEIDENTIFIER                               NULL,
    [ApplicationName] VARCHAR (50)                                   NULL,
    [Configuration]   XML(DOCUMENT [dbo].[ApplicationConfiguration]) NULL,
    [KeyID]           INT                                            NULL,
    [KeyName]         VARCHAR (150)                                  NULL,
    [Value]           VARCHAR (350)                                  NULL,
    [Deleted]         BIT                                            NULL,
    [SystemTime]      DATETIME                                       NOT NULL,
    [User_ID]         INT                                            NOT NULL,
    CONSTRAINT [PK_AppConfigHistory] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_AppConfigHistory_UserID] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID])
);

