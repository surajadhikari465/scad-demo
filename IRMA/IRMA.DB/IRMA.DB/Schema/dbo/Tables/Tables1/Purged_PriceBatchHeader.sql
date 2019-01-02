CREATE TABLE [dbo].[Purged_PriceBatchHeader](
	[PriceBatchHeaderID] [int] NOT NULL,
	[InsertDate] [datetime] NOT NULL,
 PRIMARY KEY CLUSTERED 
(
	[PriceBatchHeaderID] ASC,
	[InsertDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Purged_PriceBatchHeader] ADD  CONSTRAINT [DF_Purged_PriceBatchHeader_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
GO