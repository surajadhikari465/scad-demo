CREATE TABLE [stage].[GpmDataConversion]
(
	[Region] [nchar](2) NOT NULL,
    [PriceID] [bigint] IDENTITY(1,1) NOT NULL,
    [GpmID] [uniqueidentifier] NOT NULL,
    [ItemID] [int] NOT NULL,
    [BusinessUnitID] [int] NOT NULL,
    [StartDate] [datetime2](0) NOT NULL,
    [EndDate] [datetime2](0) NULL,
    [Price] [smallmoney] NOT NULL,
    [PriceType] [nvarchar](3) NOT NULL,
    [PriceTypeAttribute] [nvarchar](10) NOT NULL,
    [SellableUOM] [nvarchar](3) NOT NULL,
    [CurrencyCode] [nvarchar](3) NOT NULL,
    [Multiple] [tinyint] NOT NULL,
    [NewTagExpiration] [datetime2](0) NULL,
    [InsertDateUtc] [datetime2](7) NOT NULL DEFAULT (SYSUTCDATETIME())
)
GO

CREATE NONCLUSTERED INDEX [CIX_StageGpmDataConversion]
    ON [stage].[GpmDataConversion]([Region] ASC, [PriceID] ASC)
	INCLUDE
	(
		GpmID,
		ItemID,
		BusinessUnitID,
		StartDate,
		EndDate,
		Price,
		PriceType,
		PriceTypeAttribute,
		SellableUOM,
		CurrencyCode,
		Multiple,
		NewTagExpiration,
		InsertDateUtc
	) WITH (FILLFACTOR = 100);
GO

GRANT SELECT, INSERT on [stage].[GpmDataConversion] to [MammothRole]
GO