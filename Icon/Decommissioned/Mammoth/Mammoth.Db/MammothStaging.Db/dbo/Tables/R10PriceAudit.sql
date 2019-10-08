CREATE TABLE [dbo].[R10PriceAudit](
	[Code] [nvarchar](60) NULL,
	[R10_Price] [decimal](19, 5) NULL,
	[R10_PM] [decimal](9, 3) NULL,
	[EffectiveDate] [datetime] NULL,
	[ExpirationDate] [datetime] NULL,
	[BusinessUnit_Id] [nvarchar](20) NULL,
	[UOM] [nvarchar](5) NULL,
	[rn] [int] NULL,
	[InsertDate] [datetime] NULL
)
GO