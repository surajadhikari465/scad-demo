CREATE TABLE [dbo].[Purged_VendorCostHistory](
	[VendorCostHistoryID] [int] NOT NULL,
	[InsertDate] [datetime] NOT NULL,
 PRIMARY KEY CLUSTERED 
(
	[VendorCostHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Purged_VendorCostHistory] ADD  CONSTRAINT [DF_Purged_VendorCostHistory_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
GO