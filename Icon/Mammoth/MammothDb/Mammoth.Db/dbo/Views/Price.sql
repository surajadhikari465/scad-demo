CREATE VIEW dbo.Price AS
SELECT PriceID, Region, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceUOM, CurrencyID, Multiple, AddedDate, ModifiedDate
FROM dbo.Price_FL
UNION ALL 
SELECT PriceID, Region, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceUOM, CurrencyID, Multiple, AddedDate, ModifiedDate
FROM dbo.Price_MA
UNION ALL 
SELECT PriceID, Region, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceUOM, CurrencyID, Multiple, AddedDate, ModifiedDate
FROM dbo.Price_MW
UNION ALL 
SELECT PriceID, Region, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceUOM, CurrencyID, Multiple, AddedDate, ModifiedDate
FROM dbo.Price_NA
UNION ALL 
SELECT PriceID, Region, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceUOM, CurrencyID, Multiple, AddedDate, ModifiedDate
FROM dbo.Price_NC
UNION ALL 
SELECT PriceID, Region, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceUOM, CurrencyID, Multiple, AddedDate, ModifiedDate
FROM dbo.Price_NE
UNION ALL 
SELECT PriceID, Region, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceUOM, CurrencyID, Multiple, AddedDate, ModifiedDate
FROM dbo.Price_PN
UNION ALL 
SELECT PriceID, Region, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceUOM, CurrencyID, Multiple, AddedDate, ModifiedDate
FROM dbo.Price_RM
UNION ALL 
SELECT PriceID, Region, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceUOM, CurrencyID, Multiple, AddedDate, ModifiedDate
FROM dbo.Price_SO
UNION ALL 
SELECT PriceID, Region, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceUOM, CurrencyID, Multiple, AddedDate, ModifiedDate
FROM dbo.Price_SP
UNION ALL 
SELECT PriceID, Region, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceUOM, CurrencyID, Multiple, AddedDate, ModifiedDate
FROM dbo.Price_SW
UNION ALL 
SELECT PriceID, Region, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceUOM, CurrencyID, Multiple, AddedDate, ModifiedDate
FROM dbo.Price_UK


