CREATE PROCEDURE [dbo].[AddOrUpdateItems]
	@globalAttributes [dbo].ItemGlobalAttributesType READONLY
AS
BEGIN
	-- =========================================
	-- Locale Variables
	-- =========================================
	DECLARE @today DATETIME = GETDATE();
	DECLARE @totalRecordCount int;
	DECLARE @insertRecordCount int;

	-- =========================================
	-- Insert / Update based on ItemID
	-- =========================================
	SELECT * INTO #items FROM @globalAttributes;
	SET @totalRecordCount = @@ROWCOUNT;

	SELECT
		i.BrandHCID,
		i.Desc_POS,
		i.Desc_Product,
		i.FoodStampEligible,
		i.ItemID,
		i.ItemTypeID,
		n.HierarchyNationalClassID,
		i.PackageUnit,
		i.PSNumber,
		i.RetailSize,
		i.RetailUOM,
		i.ScanCode,
		m.HierarchyMerchandiseID,
		i.TaxClassHCID,
		i.Desc_CustomerFriendly, 
		i.Desc_Kitchen, 
		i.KitchenItem, 
		i.HospitalityItem, 
		i.ImageUrl
	INTO #insertItems
	FROM #items i
	LEFT JOIN dbo.Hierarchy_Merchandise		m on i.SubBrickID = m.SubBrickHCID
	LEFT JOIN dbo.Hierarchy_NationalClass	n on i.NationalClassID = n.ClassHCID
	WHERE NOT EXISTS (SELECT 1 FROM Items i2 WHERE i2.ItemID = i.ItemID);

	SET @insertRecordCount = @@ROWCOUNT

	BEGIN TRY
	BEGIN TRAN
		IF @totalRecordCount <> @insertRecordCount
			UPDATE i
			SET
				i.BrandHCID = i2.BrandHCID,
				i.Desc_POS = i2.Desc_POS,
				i.Desc_Product = i2.Desc_Product,
				i.FoodStampEligible = i2.FoodStampEligible,
				i.HierarchyMerchandiseID = m.HierarchyMerchandiseID,
				i.HierarchyNationalClassID = n.HierarchyNationalClassID,
				i.ItemTypeID = i2.ItemTypeID,
				i.PackageUnit = i2.PackageUnit,
				i.PSNumber = i2.PSNumber,
				i.RetailSize = i2.RetailSize,
				i.RetailUOM = i2.RetailUOM,
				i.TaxClassHCID = i2.TaxClassHCID,
				i.Desc_CustomerFriendly = i2.Desc_CustomerFriendly,
				i.ModifiedDate = @today,		
				i.Desc_Kitchen = i2.Desc_Kitchen, 
				i.KitchenItem = i2.KitchenItem, 
				i.HospitalityItem = i2.HospitalityItem, 
				i.ImageUrl = i2.ImageUrl
			FROM dbo.Items i
			INNER JOIN #items i2 on i2.ItemID = i.ItemID
			LEFT JOIN dbo.Hierarchy_Merchandise		m on i2.SubBrickID = m.SubBrickHCID
			LEFT JOIN dbo.Hierarchy_NationalClass	n on i2.NationalClassID = n.ClassHCID

		IF @insertRecordCount > 0
			INSERT INTO dbo.Items
			(
				ItemID,
				ItemTypeID,
				ScanCode,
				HierarchyMerchandiseID,
				HierarchyNationalClassID,
				BrandHCID,
				TaxClassHCID,
				PSNumber,
				Desc_Product,
				Desc_POS,
				PackageUnit,
				RetailSize,
				RetailUOM,
				FoodStampEligible,
				Desc_CustomerFriendly,
				AddedDate, 
				Desc_Kitchen, 
				KitchenItem, 
				HospitalityItem, 
				ImageUrl
			)
			SELECT
				i.ItemID,
				i.ItemTypeID,
				i.ScanCode,
				i.HierarchyMerchandiseID,
				i.HierarchyNationalClassID,
				i.BrandHCID,
				i.TaxClassHCID,
				i.PSNumber,
				i.Desc_Product,
				i.Desc_POS,
				i.PackageUnit,
				i.RetailSize,
				i.RetailUOM,
				i.FoodStampEligible,
				i.Desc_CustomerFriendly,
				@today,
				i.Desc_Kitchen,  
				i.KitchenItem, 
				i.HospitalityItem, 
				i.ImageUrl
			FROM #insertItems i

		COMMIT TRAN
    END TRY
    BEGIN CATCH
	    ROLLBACK TRAN;
	    THROW
    END CATCH
END
GO

GRANT EXECUTE ON [dbo].AddOrUpdateItems TO [MammothRole]
GO