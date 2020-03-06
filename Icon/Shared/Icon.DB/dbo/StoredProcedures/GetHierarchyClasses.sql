CREATE PROCEDURE dbo.GetHierarchyClasses 
     @HierarchyId INT
	,@HierarchyClassId INT NULL
	,@HierarchyLineageFilter NVARCHAR(255) NULL
AS
BEGIN
	DECLARE @HierarchyName NVARCHAR(255)

	IF @HierarchyName IS NULL
	BEGIN
		SET @hierarchyName = (
				SELECT hierarchyName
				FROM Hierarchy
				WHERE HIERARCHYID = @HierarchyId
				)
	END

	IF @HierarchyName = 'Brands'
	BEGIN
		SELECT hc.HIERARCHYID AS HIERARCHYID
			,hc.hierarchyClassID AS HierarchyClassId
			,hc.hierarchyClassName AS HierarchyClassName
			,hc.hierarchyLevel AS HierarchyLevel
			,hc.HierarchyLineage AS HierarchyLineage
			,hc.hierarchyParentClassID AS HierarchyParentClassId
		FROM dbo.BrandHierarchyView hc
		WHERE (
				@HierarchyClassId IS NULL
				OR hc.hierarchyClassID = @HierarchyClassId
				)
			AND (
				@HierarchyLineageFilter IS NULL
				OR hc.HierarchyLineage LIKE '%' + @HierarchyLineageFilter + '%'
				)
				ORDER BY HierarchyLineage
	END

	IF @HierarchyName = 'Merchandise'
	BEGIN
		SELECT hc.HIERARCHYID AS HIERARCHYID
			,hc.hierarchyClassID AS HierarchyClassId
			,hc.hierarchyClassName AS HierarchyClassName
			,hc.hierarchyLevel AS HierarchyLevel
			,hc.HierarchyLineage AS HierarchyLineage
			,hc.hierarchyParentClassID AS HierarchyParentClassId
		FROM dbo.MerchandiseHierarchyView hc
		WHERE (
				@HierarchyClassId IS NULL
				OR hc.hierarchyClassID = @HierarchyClassId
				)
			AND (
				@HierarchyLineageFilter IS NULL
				OR hc.HierarchyLineage LIKE '%' + @HierarchyLineageFilter + '%'
				)
				ORDER BY HierarchyLineage
	END

	IF @HierarchyName = 'Tax'
	BEGIN
		SELECT hc.HIERARCHYID AS HIERARCHYID
			,hc.hierarchyClassID AS HierarchyClassId
			,hc.hierarchyClassName AS HierarchyClassName
			,hc.hierarchyLevel AS HierarchyLevel
			,hc.HierarchyLineage AS HierarchyLineage
			,hc.hierarchyParentClassID AS HierarchyParentClassId
		FROM dbo.TaxHierarchyView hc
		WHERE (
				@HierarchyClassId IS NULL
				OR hc.hierarchyClassID = @HierarchyClassId
				)
			AND (
				@HierarchyLineageFilter IS NULL
				OR hc.HierarchyLineage LIKE '%' + @HierarchyLineageFilter + '%'
				)
				ORDER BY HierarchyLineage
	END

	IF @HierarchyName = 'National'
	BEGIN
		SELECT hc.HIERARCHYID AS HIERARCHYID
			,hc.hierarchyClassID AS HierarchyClassId
			,hc.hierarchyClassName AS HierarchyClassName
			,hc.hierarchyLevel AS HierarchyLevel
			,hc.HierarchyLineage AS HierarchyLineage
			,hc.hierarchyParentClassID AS HierarchyParentClassId
		FROM dbo.NationalClassHierarchyView hc
		WHERE (
				@HierarchyClassId IS NULL
				OR hc.hierarchyClassID = @HierarchyClassId
				)
			AND (
				@HierarchyLineageFilter IS NULL
				OR hc.HierarchyLineage LIKE '%' + @HierarchyLineageFilter + '%'
				)
				ORDER BY HierarchyLineage
	END

	IF @HierarchyName = 'Financial'
	BEGIN
		SELECT hc.HIERARCHYID AS HIERARCHYID
			,hc.hierarchyClassID AS HierarchyClassId
			,hc.hierarchyClassName AS HierarchyClassName
			,hc.hierarchyLevel AS HierarchyLevel
			,hc.HierarchyLineage AS HierarchyLineage
			,hc.hierarchyParentClassID AS HierarchyParentClassId
		FROM dbo.FinancialHierarchyView hc
		WHERE (
				@HierarchyClassId IS NULL
				OR hc.hierarchyClassID = @HierarchyClassId
				)
			AND (
				@HierarchyLineageFilter IS NULL
				OR hc.HierarchyLineage LIKE '%' + @HierarchyLineageFilter + '%'
				)
				ORDER BY HierarchyLineage
	END
	
	IF @HierarchyName = 'Manufacturer'
	BEGIN
		SELECT hc.HIERARCHYID AS HIERARCHYID
			,hc.hierarchyClassID AS HierarchyClassId
			,hc.hierarchyClassName AS HierarchyClassName
			,hc.hierarchyLevel AS HierarchyLevel
			,hc.HierarchyLineage AS HierarchyLineage
			,hc.hierarchyParentClassID AS HierarchyParentClassId
		FROM [dbo].[ManufacturerHierarchyView] hc
		WHERE (
				@HierarchyClassId IS NULL
				OR hc.hierarchyClassID = @HierarchyClassId
				)
			AND (
				@HierarchyLineageFilter IS NULL
				OR hc.HierarchyLineage LIKE '%' + @HierarchyLineageFilter + '%'
				)
				ORDER BY HierarchyLineage
	END
END