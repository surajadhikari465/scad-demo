CREATE VIEW [gpm].[Prices]
	AS 
	SELECT Region, PriceID, GpmID, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, PercentOff, InsertDateUtc, ModifiedDateUtc
	FROM [gpm].[Price_FL]
	UNION ALL
	SELECT Region, PriceID, GpmID, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, PercentOff, InsertDateUtc, ModifiedDateUtc
	FROM [gpm].[Price_MA]
	UNION ALL
	SELECT Region, PriceID, GpmID, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, PercentOff, InsertDateUtc, ModifiedDateUtc
	FROM [gpm].[Price_MW]
	UNION ALL
	SELECT Region, PriceID, GpmID, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, PercentOff, InsertDateUtc, ModifiedDateUtc
	FROM [gpm].[Price_NA]
	UNION ALL
	SELECT Region, PriceID, GpmID, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, PercentOff, InsertDateUtc, ModifiedDateUtc
	FROM [gpm].[Price_NC]
	UNION ALL
	SELECT Region, PriceID, GpmID, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, PercentOff, InsertDateUtc, ModifiedDateUtc
	FROM [gpm].[Price_NE]
	UNION ALL
	SELECT Region, PriceID, GpmID, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, PercentOff, InsertDateUtc, ModifiedDateUtc
	FROM [gpm].[Price_PN]
	UNION ALL
	SELECT Region, PriceID, GpmID, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, PercentOff, InsertDateUtc, ModifiedDateUtc
	FROM [gpm].[Price_RM]
	UNION ALL
	SELECT Region, PriceID, GpmID, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, PercentOff, InsertDateUtc, ModifiedDateUtc
	FROM [gpm].[Price_SO]
	UNION ALL
	SELECT Region, PriceID, GpmID, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, PercentOff, InsertDateUtc, ModifiedDateUtc
	FROM [gpm].[Price_SP]
	UNION ALL
	SELECT Region, PriceID, GpmID, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, PercentOff, InsertDateUtc, ModifiedDateUtc
	FROM [gpm].[Price_SW]
	UNION ALL
	SELECT Region, PriceID, GpmID, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, PercentOff, InsertDateUtc, ModifiedDateUtc
	FROM [gpm].[Price_TS]
	UNION ALL
	SELECT Region, PriceID, GpmID, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, PercentOff, InsertDateUtc, ModifiedDateUtc
	FROM [gpm].[Price_UK]
GO

GRANT SELECT ON [gpm].[Prices] TO [TibcoRole]
GO