/*
This table is not a core piece of the base-scan facility; it was added to support that solution and any others apps or processes
that want to use it, because every system should have this.

When this was written, it was checked into Azure repo here:
https://dev.azure.com/wholefoods/Supply%20Chain%20Application%20Development/_git/SCAD?path=/OutOfStock/DEV/OOSDatabase/dbo/Tables/AppLog.sql&version=GBmaster

Main tech doc(s) here: https://dev.azure.com/wholefoods/Supply%20Chain%20Application%20Development/_git/SCAD?path=/OutOfStock/DEV/OOSDatabase/_documentation/

This table definition is a copy of the app.AppLog table in Mammoth DB with a couple changes:
- Removed [exception] col.
- Renamed constraints to have type prefix.

*/
CREATE TABLE [dbo].[AppLog] (
  [AppLogID]    INT             IDENTITY (1, 1) NOT NULL,
  [AppID]       INT             NOT NULL,
  [Level]       NVARCHAR (100)  NULL,
  [Logger]      NVARCHAR (255)  NULL,
  [UserName]    NVARCHAR (255)  NULL,
  [MachineName] NVARCHAR (255)  NULL,
  [InsertDate]  DATETIME2 (3)   CONSTRAINT [DF_AppLog_InsertDate] DEFAULT (sysdatetime()) NOT NULL,
  [LogDate]     DATETIME2 (3)   NULL,
  [Thread]      NVARCHAR (100)  NULL,
  [Message]     NVARCHAR (max)  NULL,
  [CallSite]    NVARCHAR (max)  NULL,
  [StackTrace]  NVARCHAR (max)  NULL
  CONSTRAINT [PK_AppLog] PRIMARY KEY CLUSTERED ([AppLogID] ASC) WITH (FILLFACTOR = 100),
  CONSTRAINT [FK_AppLog_App] FOREIGN KEY ([AppID]) REFERENCES [dbo].[App] ([AppID])
);

go
CREATE NONCLUSTERED INDEX [IX_AppID_IncludeLogDate] ON [dbo].[AppLog]
(
	[AppID] ASC
)
INCLUDE ([LogDate]);

go
CREATE NONCLUSTERED INDEX [IX_AppLog_InsertDate] ON [dbo].[AppLog]
(
	[InsertDate] ASC
);

