CREATE TABLE [gpm].[ActivePricesStaging]
(
	[PriceID] INT NOT NULL,
	[Region] NVARCHAR(2) NOT NULL,
	[ItemID] INT NOT NULL,
	[BusinessUnitID] INT NOT NULL,
	[StartDate] DATETIME2(7) NOT NULL,
	[EndDate] DATETIME2(7) NULL,
	[Price] DECIMAL(19,4) NOT NULL,
	[PriceType] NVARCHAR(3) NOT NULL,
	[PriceTypeAttribute] NVARCHAR(10) NOT NULL,
	[SellableUOM] NVARCHAR(3) NOT NULL,
	[CurrencyCode] NVARCHAR(3) NOT NULL,
	[Multiple] INT NOT NULL
)

GO

CREATE CLUSTERED INDEX [CIX_ActivePricesStaging_PriceID_Region] ON [gpm].[ActivePricesStaging] ([Region], [PriceID])
