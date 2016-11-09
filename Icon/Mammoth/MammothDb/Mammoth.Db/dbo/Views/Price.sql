
CREATE VIEW dbo.Price AS
SELECT Region, ItemID, BusinessUnitID, StartDate, Price, AddedDate, ModifiedDate
FROM dbo.Price_FL
UNION ALL 
SELECT Region, ItemID, BusinessUnitID, StartDate, Price, AddedDate, ModifiedDate
FROM dbo.Price_MA
UNION ALL 
SELECT Region, ItemID, BusinessUnitID, StartDate, Price, AddedDate, ModifiedDate
FROM dbo.Price_MW
UNION ALL 
SELECT Region, ItemID, BusinessUnitID, StartDate, Price, AddedDate, ModifiedDate
FROM dbo.Price_NA
UNION ALL 
SELECT Region, ItemID, BusinessUnitID, StartDate, Price, AddedDate, ModifiedDate
FROM dbo.Price_NC
UNION ALL 
SELECT Region, ItemID, BusinessUnitID, StartDate, Price, AddedDate, ModifiedDate
FROM dbo.Price_NE
UNION ALL 
SELECT Region, ItemID, BusinessUnitID, StartDate, Price, AddedDate, ModifiedDate
FROM dbo.Price_PN
UNION ALL 
SELECT Region, ItemID, BusinessUnitID, StartDate, Price, AddedDate, ModifiedDate
FROM dbo.Price_RM
UNION ALL 
SELECT Region, ItemID, BusinessUnitID, StartDate, Price, AddedDate, ModifiedDate
FROM dbo.Price_SO
UNION ALL 
SELECT Region, ItemID, BusinessUnitID, StartDate, Price, AddedDate, ModifiedDate
FROM dbo.Price_SP
UNION ALL 
SELECT Region, ItemID, BusinessUnitID, StartDate, Price, AddedDate, ModifiedDate
FROM dbo.Price_SW
UNION ALL 
SELECT Region, ItemID, BusinessUnitID, StartDate, Price, AddedDate, ModifiedDate
FROM dbo.Price_UK


