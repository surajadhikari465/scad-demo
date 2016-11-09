CREATE TABLE [dbo].[ItemPriceType](
	[ItemPriceTypeId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[ItemPriceTypeCode] [nvarchar](3) NOT NULL,
	[ItemPriceTypeDesc] [nvarchar](255) NOT NULL
)