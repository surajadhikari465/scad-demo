CREATE PROCEDURE [dbo].[UpdateItem]
	@ItemId INT,
	@BrandsHierarchyClassId INT,
	@FinancialHierarchyClassId INT,
	@MerchandiseHierarchyClassId INT,
	@NationalHierarchyClassId INT ,
	@TaxHierarchyClassId INT,
	@ManufacturerHierarchyClassId INT,
	@ItemAttributesJson NVARCHAR(MAX),
	@ItemTypeCode NVARCHAR(100)
AS
BEGIN
	DECLARE @merchandiseHierarchyId INT = (SELECT hierarchyId FROM Hierarchy WHERE hierarchyName = 'Merchandise'),
            @brandsHierarchyId INT = (SELECT hierarchyId FROM Hierarchy WHERE hierarchyName = 'Brands'),
            @taxHierarchyId INT = (SELECT hierarchyId FROM Hierarchy WHERE hierarchyName = 'Tax'),
            @financialHierarchyId INT = (SELECT hierarchyId FROM Hierarchy WHERE hierarchyName = 'Financial'),
            @nationalHierarchyId INT = (SELECT hierarchyId FROM Hierarchy WHERE hierarchyName = 'National'),
		    @manufacturerHierarchyId INT = (SELECT hierarchyId FROM Hierarchy WHERE hierarchyName = 'Manufacturer'),
            @itemTypeId INT = (SELECT itemTypeId FROM dbo.ItemType	WHERE ItemTypeCode = @ItemTypeCode)

    CREATE TABLE #tempHierarchyClassIds
    (
        HierarchyClassId INT,
        HierarchyId INT
    )

    INSERT INTO #tempHierarchyClassIds
    VALUES (@MerchandiseHierarchyClassId, @merchandiseHierarchyId),
        (@BrandsHierarchyClassId, @brandsHierarchyId),
        (@TaxHierarchyClassId, @taxHierarchyId),
        (@FinancialHierarchyClassId, @financialHierarchyId),
        (@NationalHierarchyClassId, @nationalHierarchyId),
	    (@ManufacturerHierarchyClassId, @manufacturerHierarchyId)

    UPDATE ihc
        SET hierarchyClassID = temp.HierarchyClassId
    FROM ItemHierarchyClass ihc
    JOIN HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
    JOIN Hierarchy h ON hc.hierarchyId = h.hierarchyID
    JOIN #tempHierarchyClassIds temp ON h.hierarchyId = temp.hierarchyId
    WHERE ihc.itemId = @ItemId
		AND ihc.hierarchyClassId <> temp.HierarchyClassId
		AND temp.HierarchyClassId > 0

	-- manufactuere is not a required hierarchy so there is special logic to handle it
	DECLARE @CurrentManufacturerHierarchyClassId int
	SET @CurrentManufacturerHierarchyClassId = (SELECT 
	hc.hierarchyClassID from HierarchyClass hc
	join dbo.ItemHierarchyClass ihc on ihc.hierarchyClassID = hc.hierarchyClassID
	AND hc.hierarchyID = @manufacturerHierarchyId
	AND ihc.itemID = @itemId)

	-- a manufacturer hierarchy class record for this item exists
	IF @CurrentManufacturerHierarchyClassId IS NOT NULL
	BEGIN
		If @ManufacturerHierarchyClassId = 0 -- if we update it to 0 then delete the record
		BEGIN
			DELETE FROM ItemHierarchyClass
			WHERE ItemHierarchyClass.itemID = @itemID 
			AND ItemHierarchyClass.hierarchyClassID = @CurrentManufacturerHierarchyClassId
		END
		ELSE
		BEGIN
			IF @ManufacturerHierarchyClassId <> @CurrentManufacturerHierarchyClassId
			BEGIN
				 UPDATE ihc -- a hierarchy record exists so update it
					SET hierarchyClassID = @ManufacturerHierarchyClassId
				FROM ItemHierarchyClass ihc
				JOIN HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
				JOIN Hierarchy h ON hc.hierarchyId = h.hierarchyID
				where H.hierarchyID = @manufacturerHierarchyId
				and ihc.itemID = @itemId
			END
		END
	END
	ELSE
	BEGIN 
		-- there isn't a existing manufacturer record so insert it 
		If @ManufacturerHierarchyClassId > 0
		BEGIN
			INSERT INTO ItemHierarchyClass (
					ItemId
					,hierarchyClassID
					,localeID
					)
					VALUES
					(
					@ItemId,
					@ManufacturerHierarchyClassId,
					1
					)
		END
	END


    UPDATE Item
        SET ItemAttributesJson = @ItemAttributesJson,
		ItemTypeId = @itemTypeId
    WHERE itemID = @ItemId AND (
		 ItemAttributesJson <> @ItemAttributesJson OR
		 ItemTypeId <> @itemTypeId)
END
GO


 