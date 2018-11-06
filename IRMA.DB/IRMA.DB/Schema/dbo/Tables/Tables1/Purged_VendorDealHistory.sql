CREATE TABLE [dbo].[Purged_VendorDealHistory](
	[VendorDealHistoryID] [int] NOT NULL,
	[InsertDate] [datetime] NOT NULL,
 PRIMARY KEY CLUSTERED 
(
	[VendorDealHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Purged_VendorDealHistory] ADD  CONSTRAINT [DF_Purged_VendorDealHistory_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
GO