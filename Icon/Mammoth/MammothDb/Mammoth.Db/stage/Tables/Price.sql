CREATE TABLE [stage].[Price]
(
	[Region] NCHAR(2) NOT NULL,
	[ScanCode] NVARCHAR(13) NOT NULL, 
    [BusinessUnitId] INT NOT NULL, 
	[Multiple] TINYINT NOT NULL,
    [Price] SMALLMONEY NOT NULL, 
    [PriceType] NVARCHAR(3) NOT NULL,
	[StartDate] DATETIME NOT NULL, 
	[EndDate] DATETIME NULL,
	[PriceUom] NVARCHAR(3) NOT NULL,
	[CurrencyCode] NVARCHAR(3) NOT NULL,
    [Timestamp] DATETIME2(7) NULL,
	[TransactionId] UNIQUEIDENTIFIER NOT NULL
)
