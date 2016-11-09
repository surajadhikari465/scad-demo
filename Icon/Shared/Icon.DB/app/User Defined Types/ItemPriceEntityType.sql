create type app.ItemPriceEntityType AS TABLE 
(
    [ItemId]					INT			NOT NULL,
	[LocaleId]					INT			NOT NULL,
	[ItemPriceTypeId]			INT			NOT NULL,
	[UomId]						INT			NOT NULL,
	[CurrencyTypeId]			INT			NOT NULL,
	[ItemPriceAmount]			MONEY		NOT NULL,
	[BreakPointStartQuantity]   INT         NULL,
	[StartDate]					DATE		NULL,
	[EndDate]					DATE		NULL
)
GO
