CREATE TABLE [app].[AppLog] (
    [AppLogID]   INT             IDENTITY (1, 1) NOT NULL,
    [AppID]      INT             NOT NULL,
    [UserName]   NVARCHAR (255)  NOT NULL,
    [InsertDate] DATETIME2 (3)   CONSTRAINT [AppLog_InsertDate_DF] DEFAULT (getdate()) NOT NULL,
    [LogDate]    DATETIME2 (3)   NOT NULL,
    [Level]      NVARCHAR (16)   NOT NULL,
    [Logger]     NVARCHAR (255)  NOT NULL,
    [Message]    NVARCHAR (4000) NOT NULL,
	[MachineName] [nvarchar](255) NULL,
    CONSTRAINT [AppLog_PK] PRIMARY KEY CLUSTERED ([AppLogID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [App_AppLog_FK] FOREIGN KEY ([AppID]) REFERENCES [app].[App] ([AppID])
);
GO

CREATE NONCLUSTERED INDEX [IX_AppID_IncludeLogDate]ON [app].[AppLog]
(
	[AppID] ASC
)
INCLUDE ( 	[LogDate]) 
GO

CREATE NONCLUSTERED INDEX [IX_AppLog_InsertDate] ON [app].[AppLog]
(
	[InsertDate] ASC
)
GO

CREATE TRIGGER app.TriggerAppLogArchive ON app.AppLog AFTER DELETE
AS
  INSERT INTO app.AppLogArchive([AppID], [UserName], [InsertDate], [LogDate], [Level], [Logger], [Message], [MachineName])
    SELECT [AppID], [UserName], [InsertDate], [LogDate], [Level], [Logger], [Message], [MachineName]
    FROM deleted
    WHERE deleted.Level = 'Error';
GO