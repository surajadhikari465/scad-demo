
CREATE TABLE [esb].[MessageQueueItemArchive](
	[MessageQueueItemArchiveId] [bigint] IDENTITY(1,1) NOT NULL,
	[MessageQueueItemJson] [nvarchar](max) NOT NULL,
	[ErrorOccurred] [bit] NOT NULL,
	[ErrorMessage] [nvarchar](max) NOT NULL,
	[WarningMessage] [nvarchar](max) NOT NULL,
	[MessageId] [uniqueidentifier] NOT NULL,
	[Message] [xml] NOT NULL,
	[MessageHeader] [nvarchar](max) NULL,
	[InsertDateUtc] DATETIME2(7) NOT NULL,
	[Machine] [nvarchar](128) NOT NULL
 CONSTRAINT [PK_MessageQueueItemArchive] PRIMARY KEY CLUSTERED 
(
	[MessageQueueItemArchiveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY], 

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

