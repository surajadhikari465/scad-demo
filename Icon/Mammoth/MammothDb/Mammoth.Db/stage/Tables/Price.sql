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
GO

CREATE NONCLUSTERED INDEX [IX_Price_TransactionId_Region] ON [stage].[Price] ([TransactionId] ASC, [Region] ASC)
	INCLUDE ([ScanCode], [BusinessUnitId], [Multiple], [Price], [PriceType], [StartDate], [EndDate], [PriceUom], [CurrencyCode])
GO