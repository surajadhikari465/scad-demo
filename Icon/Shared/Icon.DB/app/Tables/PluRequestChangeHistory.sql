CREATE TABLE [app].[PLURequestChangeHistory](
	[pluRequestChangeHistoryID] [int] IDENTITY(1,1) NOT NULL,
	[pluRequestID] [int] NOT NULL,
	[pluRequestChange] [varchar](max) NOT NULL,
	[pluRequestChangeTypeID] [int] NOT NULL,
	[insertedDate] [datetime] NOT NULL,
	[insertedUser] [varchar](255) NOT NULL,
 CONSTRAINT [PK_RequestChangeHistory] PRIMARY KEY CLUSTERED 
(
	[pluRequestChangeHistoryID] ASC,
	[pluRequestID] ASC,
	[pluRequestChangeTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


ALTER TABLE [app].[PLURequestChangeHistory]  WITH CHECK ADD  CONSTRAINT [FK_pluRequestChange_pluRequestChangeTypeID] FOREIGN KEY([pluRequestChangeTypeID])
REFERENCES [app].[PLURequestChangeType] ([pluRequestChangeTypeID])
GO

ALTER TABLE [app].[PLURequestChangeHistory] CHECK CONSTRAINT [FK_pluRequestChange_pluRequestChangeTypeID]
GO

ALTER TABLE [app].[PLURequestChangeHistory]  WITH CHECK ADD  CONSTRAINT [FK_pluRequestChange_pluRequestID] FOREIGN KEY([pluRequestID])
REFERENCES [app].[PLURequest] ([pluRequestID])
GO

ALTER TABLE [app].[PLURequestChangeHistory] CHECK CONSTRAINT [FK_pluRequestChange_pluRequestID]
GO