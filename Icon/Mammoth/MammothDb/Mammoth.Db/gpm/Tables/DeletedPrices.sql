CREATE TABLE [gpm].[DeletedPrices](
	[Region] [nchar](2) NOT NULL,
	[PriceID] [bigint] NOT NULL,
	[GpmID] [uniqueidentifier] NOT NULL,
	[ItemID] [int] NOT NULL,
	[BusinessUnitID] [int] NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NULL,
	[Price] [smallmoney] NOT NULL,
	[PriceType] [nvarchar](3) NOT NULL,
	[PriceTypeAttribute] [nvarchar](10) NOT NULL,
	[PriceUOM] [nvarchar](3) NOT NULL,
	[CurrencyID] [int] NOT NULL,
	[Multiple] [tinyint] NOT NULL,
	[NewTagExpiration] [datetime2](7) NULL,
	[AddedDate] [datetime2](7),
	[DeleteDate] [datetime2](7) NOT NULL DEFAULT (sysdatetime())
	CONSTRAINT [PK_GpmDeletedPrices] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100)
)
GO

CREATE CLUSTERED INDEX [CIX_GpmDeletedPrice]
    ON [gpm].[DeletedPrices]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100);
GO

CREATE NONCLUSTERED INDEX [IX_GpmDeletedPrice_GpmIdGuid]
	ON [gpm].[DeletedPrices] (GpmID ASC) WITH (FILLFACTOR = 100);
GO