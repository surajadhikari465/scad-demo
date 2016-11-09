
CREATE PROCEDURE [dbo].[AddOrUpdateItems_FromStaging]
	@transactionId uniqueidentifier
AS
BEGIN
	-- =========================================
	-- Locale Variables
	-- =========================================
	DECLARE @date DATETIME = getdate();

	-- =========================================
	-- Insert / Update based on ItemID
	-- =========================================
	MERGE
		dbo.Items with (updlock, rowlock) i
	USING
		(
			SELECT
				stg.ItemID,
				stg.ItemTypeID,
				stg.ScanCode,
				m.HierarchyMerchandiseID,
				n.HierarchyNationalClassID,
				stg.BrandHCID,
				stg.TaxClassHCID,
				stg.PSNumber,
				stg.Desc_Product,
				stg.Desc_POS,
				stg.PackageUnit,
				stg.RetailSize,
				stg.RetailUOM,
				stg.FoodStampEligible,
				stg.Timestamp
			FROM
				stage.Items								stg
				LEFT JOIN dbo.Hierarchy_Merchandise		m on stg.SubBrickID = m.SubBrickHCID
				LEFT JOIN dbo.Hierarchy_NationalClass	n on stg.NationalClassID = n.ClassHCID
			WHERE 
				stg.TransactionId = @transactionId
		) s
	ON
		i.ItemID = s.ItemID
	WHEN MATCHED THEN
		UPDATE
		SET
			i.ItemTypeID				= s.ItemTypeID,
			i.HierarchyMerchandiseID	= s.HierarchyMerchandiseID,
			i.HierarchyNationalClassID	= s.HierarchyNationalClassID,
			i.BrandHCID					= s.BrandHCID,
			i.TaxClassHCID				= s.TaxClassHCID,
			i.PSNumber					= s.PSNumber,
			i.Desc_Product				= s.Desc_Product,
			i.Desc_POS					= s.Desc_POS,
			i.PackageUnit				= s.PackageUnit,
			i.RetailSize				= s.RetailSize,
			i.RetailUOM					= s.RetailUOM,
			i.FoodStampEligible			= s.FoodStampEligible,
			i.ModifiedDate				= @date
	WHEN NOT MATCHED THEN
		INSERT
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
			AddedDate
		)
		VALUES
		(
			s.ItemID,
			s.ItemTypeID,
			s.ScanCode,
			s.HierarchyMerchandiseID,
			s.HierarchyNationalClassID,
			s.BrandHCID,
			s.TaxClassHCID,
			s.PSNumber,
			s.Desc_Product,
			s.Desc_POS,
			s.PackageUnit,
			s.RetailSize,
			s.RetailUOM,
			s.FoodStampEligible,
			@date
		);
END

GO

