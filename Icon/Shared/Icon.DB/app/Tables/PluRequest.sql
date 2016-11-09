CREATE TABLE [app].[PLURequest](
	[pluRequestID] [int] IDENTITY(1,1) NOT NULL,
	[merchandiseClassID] [int] NULL,
	[brandName] [varchar](35) NOT NULL,
	[itemDescription] [varchar](60) NOT NULL,
	[posDescription] [varchar](60) NOT NULL,
	[packageUnit] [int] NOT NULL,
	[retailSize] [decimal](9, 4) NOT NULL,
	[retailUom] [varchar](100) NOT NULL,
	[nationalClassID] [int] NOT NULL,
	[scanCodeTypeID] [int] NOT NULL,
	[nationalPLU] [varchar](13) NULL,
	[requestStatus] [varchar](50) NOT NULL,
	[requesterName] [varchar](255) NOT NULL,
	[requesterEmail] [varchar](255) NULL,
	[requestedDate] [datetime] NOT NULL,
	[lastModifiedDate] [datetime] NOT NULL,
	[lastModifiedUser] [varchar](255) NOT NULL,
	[FinancialClassID] [int] NOT NULL,
 CONSTRAINT [PK_PLURequest] PRIMARY KEY CLUSTERED 
(
	[pluRequestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE [app].[PLURequest]  WITH CHECK ADD  CONSTRAINT [ScanCodeType_PLURequest_FK1] FOREIGN KEY([scanCodeTypeID])
REFERENCES [dbo].[ScanCodeType] ([scanCodeTypeID])
GO

ALTER TABLE [app].[PLURequest] CHECK CONSTRAINT [ScanCodeType_PLURequest_FK1]
GO
