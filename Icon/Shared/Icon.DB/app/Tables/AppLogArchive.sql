CREATE TABLE app.AppLogArchive (
  [AppLogArchiveID]  INT IDENTITY (1, 1) NOT NULL,
  [AppID]       INT             NOT NULL,
  [UserName]    NVARCHAR (255)  NOT NULL,
  [InsertDate]  DATETIME2 (3)   NOT NULL,
  [LogDate]     DATETIME2 (3)   NOT NULL,
  [Level]       NVARCHAR (16)   NOT NULL,
  [Logger]      NVARCHAR (255)  NOT NULL,
  [Message]     NVARCHAR (4000) NOT NULL,
  [MachineName] NVARCHAR(255) NULL,
  CONSTRAINT PK_AppLogArchive PRIMARY KEY CLUSTERED (AppLogArchiveID ASC) WITH (FILLFACTOR = 80),
  CONSTRAINT FK_App_AppLogArchive FOREIGN KEY (AppID) REFERENCES app.App(AppID)
);
GO
CREATE NONCLUSTERED INDEX IX_AppIDArchive_IncludeLogDate ON app.AppLogArchive(AppID ASC) INCLUDE(LogDate);
GO
CREATE NONCLUSTERED INDEX IX_AppLogArchive_InsertDate ON app.AppLogArchive(InsertDate ASC);
GO