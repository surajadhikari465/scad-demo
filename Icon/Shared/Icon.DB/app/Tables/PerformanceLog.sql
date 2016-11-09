CREATE TABLE [app].[PerformanceLog] (
    [PerformanceLogID] INT IDENTITY (1, 1) NOT NULL,
    [AppID]      INT             NOT NULL,
    [UserName]   NVARCHAR (255)  NOT NULL,
    [InsertDate] DATETIME2 (3)   CONSTRAINT [PerformanceLog_InsertDate_DF] DEFAULT (getdate()) NOT NULL,
    [LogDate]    DATETIME2 (3)   NOT NULL,
    [Level]      NVARCHAR (16)   NOT NULL,
    [Logger]     NVARCHAR (255)  NOT NULL,
    [Message]    NVARCHAR (4000) NOT NULL,
	[MachineName] [nvarchar](255) NULL,
    CONSTRAINT [PerformanceLog_PK] PRIMARY KEY CLUSTERED ([PerformanceLogID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [App_PerformanceLog_FK] FOREIGN KEY ([AppID]) REFERENCES [app].[App] ([AppID])
);

