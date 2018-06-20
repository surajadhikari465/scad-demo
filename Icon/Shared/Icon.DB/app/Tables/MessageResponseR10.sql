CREATE TABLE [app].[MessageResponseR10](
	[MessageResponseR10Id] [int] IDENTITY(1,1) NOT NULL,
	[MessageId] [nvarchar](100) NOT NULL,
	[RequestSuccess] [bit] NOT NULL,
	[SystemError] [bit] NOT NULL,
	[FailureReasonCode] [nvarchar](50) NULL,
	[ResponseText] [xml] NOT NULL,
	[InsertDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__MessageResponseR10] PRIMARY KEY CLUSTERED 
(
	[MessageResponseR10Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX MessageResponse_R10_IDX ON app.MessageResponseR10(MessageId)
GO