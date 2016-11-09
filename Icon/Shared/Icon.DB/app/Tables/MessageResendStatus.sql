CREATE TABLE [app].[MessageResendStatus](
	[MesssageResendStatusId] [int] IDENTITY(1,1) NOT NULL,
	[MessageHistoryId] [int] NOT NULL,
	[NumberOfResends] [int] NOT NULL,
	[InsertDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_MessageRetryStatus] PRIMARY KEY CLUSTERED 
(
	[MesssageResendStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [app].[MessageResendStatus]  WITH CHECK ADD  CONSTRAINT [FK_MessageRetryStatus_MessageHistory] FOREIGN KEY([MessageHistoryId])
REFERENCES [app].[MessageHistory] ([MessageHistoryId])
ON DELETE CASCADE
GO

ALTER TABLE [app].[MessageResendStatus] CHECK CONSTRAINT [FK_MessageRetryStatus_MessageHistory]
GO