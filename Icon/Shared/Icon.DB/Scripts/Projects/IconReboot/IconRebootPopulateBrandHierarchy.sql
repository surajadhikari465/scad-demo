BEGIN
	DECLARE @designation INT
	DECLARE @hierarchyId INT

	IF NOT EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_SCHEMA = 'dbo'
				AND TABLE_NAME = 'temp_BrandHierarchy'
			)
	BEGIN
		CREATE TABLE dbo.temp_BrandHierarchy (
			[brand_name] NVARCHAR(255) NULL
			,[brand_abbreviation] NVARCHAR(255) NULL
			,[Designation] NVARCHAR(255) NULL
			)
	END

	TRUNCATE TABLE dbo.temp_BrandHierarchy

	PRINT 'Inserting Infor Items into temp_BrandHierarchy table...'

	SET @designation = (
			SELECT traitid
			FROM trait
			WHERE traitCode = 'GRD'
			)
	SET @hierarchyId = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE hierarchyName = 'Brands'
			)

	BULK INSERT dbo.temp_BrandHierarchy
	FROM '\\ODWD6801\Temp\IconConversion\BrandHierarchy.csv' WITH (
			FIRSTROW = 2
			,FIELDTERMINATOR = ','
			,ROWTERMINATOR = '\n'
			,DATAFILETYPE = 'char'
			,FORMAT = 'CSV'
			,CODEPAGE = '65001'
			);

	WITH SourceTableCTE
	AS (
		SELECT DISTINCT t.brand_name
			,s.hierarchyClassID
			,t.Designation
		FROM dbo.temp_BrandHierarchy t
		JOIN HierarchyClass s ON s.hierarchyClassName = t.brand_name
		WHERE s.HIERARCHYID = @hierarchyId
		)
	MERGE HierarchyClassTrait AS target
	USING SourceTableCTE AS source
		ON (
				target.hierarchyClassID = source.hierarchyClassID
				AND target.traitid = @designation
				)
	WHEN MATCHED
		THEN
			UPDATE
			SET target.traitValue = source.Designation
	WHEN NOT MATCHED
		THEN
			INSERT (
				traitid
				,hierarchyclassid
				,traitValue
				)
			VALUES (
				@designation
				,Source.hierarchyClassID
				,Source.Designation
				);

	SELECT t.brand_name AS BrandsNotFoundInHierarchyClass
	FROM dbo.temp_BrandHierarchy t
	WHERE t.brand_name NOT IN (
			SELECT hc.hierarchyClassName
			FROM dbo.HierarchyClass hc
			)

	DROP TABLE dbo.temp_BrandHierarchy
END