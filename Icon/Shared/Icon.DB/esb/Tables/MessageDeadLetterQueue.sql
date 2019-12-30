CREATE TABLE [esb].[MessageDeadLetterQueue](
	[MessageDeadLetterQueueId] [int] IDENTITY(1,1) NOT NULL,
	[JsonObject] [nvarchar](max) NOT NULL,
	[InsertDateUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_MessageDeadLetterQueue] PRIMARY KEY CLUSTERED 
(
	[MessageDeadLetterQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] 
GO

ALTER TABLE [esb].[MessageDeadLetterQueue] ADD  CONSTRAINT [DF_MessageDeadLetterQueue_InsertDateUtc]  DEFAULT (sysutcdatetime()) FOR [InsertDateUtc]
GO
