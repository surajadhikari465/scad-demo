CREATE TABLE [dbo].[Purged_ItemHistory](
	[ItemHistoryID] [int] NOT NULL,
	[DateStamp] [datetime] NOT NULL,
	[InsertDate] [datetime] NOT NULL,
 PRIMARY KEY CLUSTERED 
(
	[ItemHistoryID] ASC,
	[DateStamp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Purged_ItemHistory] ADD  CONSTRAINT [DF_Purged_ItemHistory_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
GO