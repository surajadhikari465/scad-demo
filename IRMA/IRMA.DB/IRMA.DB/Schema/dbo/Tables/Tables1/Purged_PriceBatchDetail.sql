CREATE TABLE [dbo].[Purged_PriceBatchDetail](
	[PriceBatchDetailID] [int] NOT NULL,
	[InsertDate] [datetime] NOT NULL,
 PRIMARY KEY CLUSTERED 
(
	[PriceBatchDetailID] ASC,
	[InsertDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Purged_PriceBatchDetail] ADD  CONSTRAINT [DF_Purged_PriceBatchDetail_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
GO
