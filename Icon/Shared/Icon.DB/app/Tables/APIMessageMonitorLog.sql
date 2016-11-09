CREATE TABLE [app].[APIMessageMonitorLog]
(
	[APIMessageMonitorLogID] [int] IDENTITY(1,1) NOT NULL,
	[MessageTypeID] [int] NOT NULL,
	[StartTime] [datetime2] NULL,
	[EndTime] [datetime2] NULL,
	[CountProcessedMessages] [int] NULL,
	[CountFailedMessages] [int] NULL,
	CONSTRAINT [PK_APIMessageMonitorLog] 
		PRIMARY KEY CLUSTERED ([APIMessageMonitorLogID] ASC) 
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80),
	CONSTRAINT [FK_APIMessageMonitorLog_MessageTypeId] 
		FOREIGN KEY ([MessageTypeId]) REFERENCES [app].[MessageType] ([MessageTypeId])
);
GO

