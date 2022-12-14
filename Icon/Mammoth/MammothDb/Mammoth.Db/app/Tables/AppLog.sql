CREATE TABLE [app].[AppLog] (
  [AppLogID]    INT             IDENTITY (1, 1) NOT NULL,
  [AppID]       INT             NOT NULL,
  [Level]       NVARCHAR (100)  NULL,
  [Logger]      NVARCHAR (255)  NULL,
  [UserName]    NVARCHAR (255)  NULL,
  [MachineName] NVARCHAR (255)  NULL,
  [InsertDate]  DATETIME2 (3)   CONSTRAINT [AppLog_InsertDate_DF] DEFAULT (sysdatetime()) NOT NULL,
  [LogDate]     DATETIME2 (3)   NULL,
  [Thread]      NVARCHAR (100)  NULL,
  [Message]     NVARCHAR (max)  NULL,
  [CallSite]    NVARCHAR (max)  NULL,
  [Exception]   NVARCHAR (max)  NULL, 
  [StackTrace]  NVARCHAR (max)  NULL
  CONSTRAINT [AppLog_PK] PRIMARY KEY CLUSTERED ([AppLogID] ASC) WITH (FILLFACTOR = 100),
  CONSTRAINT [App_AppLog_FK] FOREIGN KEY ([AppID]) REFERENCES [app].[App] ([AppID])
);
GO

CREATE NONCLUSTERED INDEX [IX_AppID_IncludeLogDate]ON [app].[AppLog]
(
	[AppID] ASC
)
INCLUDE ([LogDate]) 
GO

CREATE NONCLUSTERED INDEX [IX_AppLog_InsertDate] ON [app].[AppLog]
(
	[InsertDate] ASC
)
GO

CREATE TRIGGER app.TriggerAppLogArchive ON app.AppLog AFTER DELETE
AS
  INSERT INTO app.AppLogArchive(AppID, Level, Logger, UserName, MachineName, InsertDate, LogDate, Thread, Message, CallSite, Exception, StackTrace)
    SELECT AppID, Level, Logger, UserName, MachineName, InsertDate, LogDate, Thread, Message, CallSite, Exception, StackTrace
    FROM deleted
    WHERE deleted.Level = 'Error';
GO

GRANT INSERT, SELECT ON [app].[AppLog] TO [TibcoRole]
GO

GRANT INSERT, SELECT ON [app].[AppLog] to [MammothRole]
GO