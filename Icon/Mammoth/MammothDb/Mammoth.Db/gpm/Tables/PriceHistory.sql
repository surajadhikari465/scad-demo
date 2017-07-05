CREATE TABLE [gpm].[PriceHistory](
	[Region] [nchar](2) NOT NULL,
	[PriceID] [bigint] NOT NULL,
	[GpmID] [uniqueidentifier] NOT NULL,
	[ItemID] [int] NOT NULL,
	[BusinessUnitID] [int] NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NULL,
	[Price] DECIMAL(19, 4) NOT NULL,
	[PriceType] [nvarchar](3) NOT NULL,
	[PriceTypeAttribute] [nvarchar](10) NOT NULL,
	[SellableUOM] [nvarchar](3) NOT NULL,
	[CurrencyCode] [nvarchar](3) NOT NULL,
	[Multiple] [tinyint] NOT NULL,
	[NewTagExpiration] [datetime2](7) NULL,
	[InsertDateUtc] [datetime2](7) NOT NULL, -- to hold when price was initially created.
	[ModifiedDateUtc] [datetime2](7) NULL, -- to hold modified date of Price_XX table
	[PriceHistoryInsertDateUtc] [datetime2](7) NOT NULL DEFAULT (SYSUTCDATETIME()), -- to hold the datetime it was inserted into PriceHistory table
)
GO

CREATE CLUSTERED INDEX [CIX_GpmPriceHistory]
    ON [gpm].[PriceHistory]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100);
GO

CREATE INDEX [IX_PriceHistory_Region_ItemID_Bu_StartDate_PriceType]
	ON [gpm].[PriceHistory] ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC);
