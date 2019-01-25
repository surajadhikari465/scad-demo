CREATE TABLE app.AppLogArchive (
 AppLogArchiveID INT IDENTITY (1, 1) NOT NULL,
 AppID           INT NOT NULL,
 Level           NVARCHAR (100),
 Logger	         NVARCHAR (255),
 UserName        NVARCHAR (255),
 MachineName     NVARCHAR (255)  NULL,
 InsertDate      DATETIME2 (3) NOT NULL,
 LogDate         DATETIME2 (3),
 Thread          NVARCHAR (100),
 Message         NVARCHAR (max)  NULL,
 CallSite        NVARCHAR (max)  NULL,
 Exception       NVARCHAR (max)  NULL, 
 StackTrace      NVARCHAR (max)  NULL
 CONSTRAINT PK_AppLogArchive PRIMARY KEY CLUSTERED (AppLogArchiveID) WITH (FILLFACTOR = 100),
 CONSTRAINT FK_App_AppLogArchive FOREIGN KEY (AppID) REFERENCES app.App(AppID)
);
GO

CREATE NONCLUSTERED INDEX IX_AppArchiveID_IncludeLogDate ON app.AppLog(AppID ASC) INCLUDE (LogDate);
GO

CREATE NONCLUSTERED INDEX IX_AppLogArchive_InsertDate ON app.AppLog(InsertDate ASC);
GO