CREATE TABLE [dbo].[Purged_EInvoicing_Item](
	[EInvoice_id] [int] NOT NULL,
	[line_num] [int] NOT NULL,
	[InsertDate] [datetime] NOT NULL,
 PRIMARY KEY CLUSTERED 
(
	[EInvoice_id] ASC,
	[line_num] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO

