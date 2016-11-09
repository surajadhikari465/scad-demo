CREATE TABLE [app].[R10MessageResponse](
	[R10MessageResponseId] [int] IDENTITY(1,1) NOT NULL,
	[MessageHistoryId] [int] NOT NULL,
	[RequestSuccess] [bit] NOT NULL,
	[SystemError] [bit] NOT NULL,
	[FailureReasonCode] [nvarchar](50) NULL,
	[ResponseText] [xml] NOT NULL,
	[InsertDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__R10MessageResponse] PRIMARY KEY CLUSTERED 
(
	[R10MessageResponseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [app].[R10MessageResponse]  WITH CHECK ADD  CONSTRAINT [FK_R10MessageResponse_MessageHistory] FOREIGN KEY([MessageHistoryId])
REFERENCES [app].[MessageHistory] ([MessageHistoryId])
ON DELETE CASCADE
GO

ALTER TABLE [app].[R10MessageResponse] CHECK CONSTRAINT [FK_R10MessageResponse_MessageHistory]
GO