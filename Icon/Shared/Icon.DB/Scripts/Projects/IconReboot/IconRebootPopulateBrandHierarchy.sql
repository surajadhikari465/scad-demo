DECLARE @designation INT
DECLARE @hierarchyId INT

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'temp_BrandHierarchy')
	DROP TABLE dbo.temp_BrandHierarchy
		
CREATE TABLE dbo.temp_BrandHierarchy
(
	[brand_name] NVARCHAR(255) NULL
	,[brand_abbreviation] NVARCHAR(255) NULL
	,[Designation] NVARCHAR(255) NULL
)			

PRINT 'Inserting Infor Items into temp_BrandHierarchy table...'

SET @designation = (SELECT traitid FROM trait WHERE traitCode = 'GRD')
SET @hierarchyId = (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Brands')

BULK INSERT dbo.temp_BrandHierarchy
FROM 'E:\sql_temp_01\IconRebootShare\BrandDesignation.csv' WITH (
		FIRSTROW = 2
		,FIELDTERMINATOR = ','
		,DATAFILETYPE = 'char'
		,FORMAT = 'CSV'
		,CODEPAGE = '65001');

ALTER TABLE dbo.temp_BrandHierarchy ADD [ID] INT identity (1,1)

DELETE E
FROM dbo.temp_BrandHierarchy E
LEFT JOIN (
	SELECT max(ID) ID
	FROM dbo.temp_BrandHierarchy
	GROUP BY brand_name
	) T ON E.ID = T.ID
WHERE T.ID IS NULL;

WITH SourceTableCTE
AS (SELECT t.brand_name
		,s.hierarchyClassID
		,t.Designation
	FROM dbo.temp_BrandHierarchy t
	JOIN HierarchyClass s ON s.hierarchyClassName = t.brand_name
	WHERE s.HIERARCHYID = @hierarchyId)
MERGE HierarchyClassTrait AS target
USING SourceTableCTE AS source
	ON (target.hierarchyClassID = source.hierarchyClassID
		AND target.traitid = @designation)
WHEN MATCHED
	THEN
		UPDATE
		SET target.traitValue = source.Designation
WHEN NOT MATCHED
	THEN
		INSERT 
			(traitid
			,hierarchyclassid
			,traitValue)
		VALUES
			(@designation
			,Source.hierarchyClassID
			,Source.Designation);

SELECT t.brand_name AS BrandsNotFoundInHierarchyClass
FROM dbo.temp_BrandHierarchy t
WHERE t.brand_name NOT IN (
		SELECT hc.hierarchyClassName
		FROM dbo.HierarchyClass hc
		WHERE hc.hierarchyID = @hierarchyId)
	
DROP TABLE dbo.temp_BrandHierarchy