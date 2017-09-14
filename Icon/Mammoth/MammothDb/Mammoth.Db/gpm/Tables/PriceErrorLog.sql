CREATE TABLE [gpm].[PriceErrorLog](
    [PriceLogID] BIGINT IDENTITY(1,1) NOT NULL,
	[Region] [nchar](2) NOT NULL,
	[GpmID] [uniqueidentifier] NOT NULL,
	[ItemID] [int] NOT NULL,
	[BusinessUnitID] [int] NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NULL,
	[Price] [smallmoney] NOT NULL,
	[PriceType] [nvarchar](3) NOT NULL,
	[PriceTypeAttribute] [nvarchar](10) NOT NULL,
	[SellableUOM] [nvarchar](3) NOT NULL,
	[CurrencyCode] [nvarchar](3) NOT NULL,
	[Multiple] [tinyint] NOT NULL,
	[NewTagExpiration] [datetime2](7) NULL,
	[InsertDateUtc] [datetime2](7) NOT NULL,
	[ErrorMessage] nvarchar(500) NOT NULL,
	[ErrorCode] nvarchar(100) NOT NULL,
 CONSTRAINT [PK_GpmPriceLog] PRIMARY KEY NONCLUSTERED 
(
	[Region] ASC,
	[ItemID] ASC,
	[BusinessUnitID] ASC,
	[StartDate] ASC,
	[PriceType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 100) ON [FG_FL]
) ON [FG_FL]

GO

GRANT INSERT ON [gpm].[PriceErrorLog] TO [TibcoRole]
GO