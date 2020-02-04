IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'temp_InforHierarchyDefaults')
	DROP TABLE dbo.temp_InforHierarchyDefaults;

CREATE TABLE dbo.temp_InforHierarchyDefaults
(
	[Node Code] nvarchar(255) NOT NULL,
	[Level Name] nvarchar(255) NOT NULL,
	[Node Name] nvarchar(255) NOT NULL,
	[Item Attribute Name] nvarchar(255) NOT NULL,
	[Item Attribute Default Value] nvarchar(255) NOT NULL
);

-- File provided by the SCAMPA team
DECLARE @print nvarchar(255) = 'Inserting Infor Hierarchy ''Configuration'' from file into temp_InforHierarchyDefaults table... ' + CAST(SYSUTCDATETIME() as nvarchar);
RAISERROR(@print,0,1) WITH NOWAIT;
BULK INSERT dbo.temp_InforHierarchyDefaults
FROM 'E:\sql_temp_01\IconRebootShare\infor_im_merch_defaults.txt' -- needs to be added with file path and filename. Currently C:\TEMP\IconConversion\infor_im_merchandise_defaults_tilda.txt
WITH
(
	FIRSTROW = 2,
	FIELDTERMINATOR = '~', -- this might change but for now it's a tilda
	ROWTERMINATOR = '0x0a'
);

-- Insert all records into #temp table
-- Remove spaces before and after pipe characters for the 'Item Attribute Default Value' column
IF OBJECT_ID('tempdb..#InforHierarchyAttributes') IS NOT NULL
	DROP TABLE #InforHierarchyAttributes

SELECT
	CAST(d.[Node Code] as int) as HierarchyClassId,
	d.[Level Name] as LevelName,
	d.[Node Name] as HierarchyClassName,
	d.[Item Attribute Name] as AttributeName,
	REPLACE(d.[Item Attribute Default Value],' | ','|') as AttributeValue -- removing spaces before and after the pipe character
INTO #InforHierarchyAttributes
FROM dbo.temp_InforHierarchyDefaults d

CREATE INDEX IX_#InforHierarchyAttributes_HierarchyClassId ON #InforHierarchyAttributes (HierarchyClassId);

-- Remove all records from HierarchyClassTrait for the Prohibit Discount, Merch Fin Mapping, and Non-Merchandise traits
DECLARE @prohibitDiscountTraitId int = (SELECT traitID FROM Trait WHERE traitDesc = 'Prohibit Discount');
DECLARE @merchFinMappingTraitId int = (SELECT traitID FROM Trait WHERE traitDesc = 'Merch Fin Mapping');
DECLARE @nonMerchTraitId int = (SELECT traitID FROM Trait WHERE traitDesc = 'Non-Merchandise');

SET @print = 'Deleting all HierarchyClassTrait records for Prohibit Discount, Merch Fin Mapping, and Non-Merchandise traitIDs... ' + CAST(SYSUTCDATETIME() as nvarchar);
RAISERROR(@print,0,1) WITH NOWAIT;
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'temp_DeletedHierarchyClassTrait')
	DROP TABLE temp_DeletedHierarchyClassTrait

CREATE TABLE dbo.temp_DeletedHierarchyClassTrait (hierarchyClassID int NOT NULL, traitID int NOT NULL, traitValue nvarchar(255) NOT NULL, uomId int NULL)

DELETE
FROM HierarchyClassTrait
OUTPUT
	deleted.hierarchyClassID,
	deleted.traitID,
	deleted.traitValue,
	deleted.uomID
	INTO dbo.temp_DeletedHierarchyClassTrait (hierarchyClassID, traitID, traitValue, uomId)
WHERE traitID IN (@prohibitDiscountTraitId, @merchFinMappingTraitId, @nonMerchTraitId);

-- Add Prohibit Discount trait
SET @print = 'Inserting Prohibit Discount sub-brick trait values... ' + CAST(SYSUTCDATETIME() as nvarchar);
RAISERROR(@print,0,1) WITH NOWAIT;
INSERT INTO HierarchyClassTrait
(
	hierarchyClassID,
	traitID,
	traitValue
)
SELECT
	hc.hierarchyClassID,
	@prohibitDiscountTraitId as traitID,
	'1' as traitValue
FROM #InforHierarchyAttributes ha
JOIN dbo.HierarchyClass hc on ha.HierarchyClassId = hc.hierarchyClassID
WHERE ha.AttributeName = 'Prohibit Discount'
AND ha.LevelName = 'GS1 Brick'

-- Add Merch Fin Mapping Trait
-- Add value as the HierarchyClassId for the finanical hierarchy
-- SubTeam infor comes in the file in the pattern of 'SubTeamId|SubTeamNumber|SubTeamName(0000)'
SET @print = 'Inserting Merch Fin Mapping sub-brick trait values... ' + CAST(SYSUTCDATETIME() as nvarchar);
RAISERROR(@print,0,1) WITH NOWAIT;

INSERT INTO HierarchyClassTrait (hierarchyClassID, traitID, traitValue)
SELECT
	ha.HierarchyClassId as hierarchyClassID,
	@merchFinMappingTraitId as traitID,
	SUBSTRING(ha.AttributeValue,0,CHARINDEX('|',ha.AttributeValue,1)) as traitValue -- this is the financial hierarchy class Id in Infor
FROM #InforHierarchyAttributes ha
WHERE ha.AttributeName = 'Subteam'

-- Add Non-Merch trait values
-- The infor values has each string setup in the pattern 'ItemTypeName|ItemTypeCode'
-- We need to populate the value with the ItemTypeID
SET @print = 'Inserting Non-Merchandise sub-brick trait values... ' + CAST(SYSUTCDATETIME() as nvarchar);
RAISERROR(@print,0,1) WITH NOWAIT;
INSERT INTO dbo.HierarchyClassTrait
(
	hierarchyClassID,
	traitID,
	traitValue
)
SELECT
	ha.HierarchyClassId as hierarchyClassID,
	@nonMerchTraitId as traitID,
	SUBSTRING(ha.AttributeValue,0,CHARINDEX('|',ha.AttributeValue,1)) AS traitValue
FROM #InforHierarchyAttributes ha
WHERE ha.AttributeName = 'Item Type'
	AND SUBSTRING(ha.AttributeValue,0,CHARINDEX('|',ha.AttributeValue,1)) <> 'N/A'

-- Update trait patterns for Merch Fin Mapping trait
UPDATE Trait
SET traitPattern = '^[0-9]*$'
WHERE traitID = @merchFinMappingTraitId