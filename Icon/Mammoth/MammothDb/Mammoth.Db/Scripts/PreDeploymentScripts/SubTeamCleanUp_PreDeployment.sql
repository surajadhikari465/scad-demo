USE Mammoth
GO

-- Update Financial_SubTeam where PSNumber is NULL
PRINT 'Updating NULL PSNumbers in the Financial_SubTeam table to have a value...'
UPDATE fst
SET PSNumber = SUBSTRING(fst.Name, CHARINDEX('(', fst.Name) + 1, 4)
OUTPUT inserted.*
FROM Financial_SubTeam fst;
GO

-- Update Items table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'PSNumber' AND Object_ID = Object_ID(N'Items'))
BEGIN
	PRINT 'Updating the Items table to have PSNumber value...'
	UPDATE i
	SET FinancialHCID = f.PSNumber
	OUTPUT inserted.*
	FROM Items i
	JOIN Financial_SubTeam f on i.FinancialHCID = f.FinancialHCID
END
GO

-- Delete Financial Hierarchy from HierarchyClass
PRINT 'Deleting all Financial Hierarchy rows in the HierarchyClass table...'
DELETE hc
OUTPUT deleted.*
FROM HierarchyClass hc
JOIN Hierarchy h on hc.HierarchyID = h.HierarchyID
WHERE hierarchyName = 'Financial';
GO