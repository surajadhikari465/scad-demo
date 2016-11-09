CREATE TABLE ##MerchAndFinTemp (
	[Sub Brick Code] NVARCHAR(255),
	[Sub Brick Description] NVARCHAR(255),
	[Financial Hierarchy Code] NVARCHAR(255),
	[Financial Hierarchy Description] NVARCHAR(255)
	);
BULK INSERT ##MerchAndFinTemp
FROM '\\irmadevfile\e$\icondata\MerchAndFin.txt' WITH (FirstRow = 2)
DECLARE @ID INT
SELECT @id = traitid
FROM trait
WHERE traitcode = 'MFM'

INSERT INTO hierarchyclasstrait (
	traitid,
	hierarchyclassid,
	traitvalue
	)
SELECT @ID,
	sbc.hierarchyclassid,
	mfm.[Financial Hierarchy Description] AS traitvalue
FROM ##MerchAndFinTemp mfm
INNER JOIN HierarchyClassTrait sbc ON sbc.traitvalue = mfm.[Sub Brick Code]
INNER JOIN trait t ON sbc.traitid = t.traitid
	AND t.traitCode = 'SBC'
DROP TABLE ##MerchAndFinTemp
SELECT *
FROM HierarchyClasstrait hct
INNER JOIN trait t ON t.traitid = hct.traitid
	AND t.traitcode = 'MFM'