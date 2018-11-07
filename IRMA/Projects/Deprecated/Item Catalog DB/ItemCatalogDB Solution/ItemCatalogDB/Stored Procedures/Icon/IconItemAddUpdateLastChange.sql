SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Benjamin Sims
-- Create date: 2014-09-29
-- Description:	Receives a Validated Icon Item and adds/updates 
--				IconItemLastChange table
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE OBJECT_ID = OBJECT_ID('IconItemAddUpdateLastChange') AND type in (N'P', N'PC'))
BEGIN
	EXEC ('CREATE PROCEDURE [dbo].[IconItemAddUpdateLastChange] as SELECT 1')
END
GO

ALTER PROCEDURE [dbo].[IconItemAddUpdateLastChange]
	-- Add the parameters for the stored procedure here
	@LastChangedItemList dbo.IconLastChangedItemType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	-- =====================================================
	-- Declare Variables
	-- =====================================================
	DECLARE @now DATETIME;
	DECLARE @defaultNonAlignedPosDepNo INT;

	SET @defaultNonAlignedPosDepNo = 294;
	SET @now = (
			SELECT GETDATE()
			);

	DECLARE @updateRetailUomSize BIT;

	SET @updateRetailUomSize = (
			SELECT FlagValue
			FROM InstanceDataFlags
			WHERE FlagKey = 'EnableIconRetailUomSizeUpdates'
			);

	-- =====================================================
	-- IconItemLastChange
	-- =====================================================
	BEGIN TRY
		-- Update existing rows
		UPDATE lc
		SET Item_Description = vi.ProductDescription,
			POS_Description = vi.PosDescription,
			Package_Desc1 = CAST(vi.PackageUnit AS DECIMAL(9, 4)),
			Food_Stamps = CAST(vi.FoodStampEligible AS BIT),
			ScaleTare = CAST(vi.Tare AS DECIMAL(9, 4)),
			Brand_ID = ib.Brand_ID,
			TaxClassID = IsNull(tc.TaxClassID,lc.TaxClassID),
			AreNutriFactsChanged = vi.AreNutriFactsUpdated,
			ClassID = IsNull(ni.ClassID, lc.ClassID),
			InsertDate = @now,
			Package_Unit_ID = CASE @updateRetailUomSize
				WHEN 1
					THEN ISNULL(iu.Unit_ID, Package_Unit_ID)
				ELSE Package_Unit_ID
				END,
			Package_Desc2 = CASE @updateRetailUomSize
				WHEN 1
					THEN vi.RetailSize
				ELSE Package_Desc2
				END
		FROM IconItemLastChange lc
		JOIN @LastChangedItemList vi ON lc.Identifier = vi.ScanCode
		JOIN ValidatedBrand vb ON vi.BrandId = vb.IconBrandId
		JOIN ItemBrand ib ON vb.IrmaBrandId = ib.Brand_ID
		LEFT JOIN TaxClass tc ON substring(vi.TaxClassName, 1, 7) = substring(tc.TaxClassDesc, 1, 7)
		LEFT JOIN NatItemClass ni ON vi.NationalClassCode = ni.ClassID
		LEFT JOIN ItemUnit iu ON vi.RetailUom = iu.Unit_Abbreviation

		-- Insert new rows if they don't exist yet
		INSERT INTO IconItemLastChange
		SELECT vi.ScanCode AS Identifier,
			NULL AS Subteam_No,
			ib.Brand_ID AS Brand_ID,
			vi.ProductDescription AS Item_Description,
			vi.PosDescription AS POS_Description,
			CAST(vi.PackageUnit AS DECIMAL(9, 4)) AS Package_Desc1,
			CAST(vi.FoodStampEligible AS BIT) AS Food_Stamps,
			CAST(vi.Tare AS DECIMAL(9, 4)) AS ScaleTare,
			tc.TaxClassID AS TaxClassID,
			@now AS InsertDate,
			vi.AreNutriFactsUpdated AS AreNutriFactsChanged,
			ni.ClassID AS ClassID,
			CASE @updateRetailUomSize
				WHEN 1
					THEN iu.Unit_ID
				ELSE ich.Package_Unit_ID
				END AS Package_Unit_ID,
			CASE @updateRetailUomSize
				WHEN 1
					THEN vi.RetailSize
				ELSE ich.Package_Desc2
				END AS Package_Desc2
		FROM @LastChangedItemList vi
		JOIN ValidatedBrand vb ON vi.BrandId = vb.IconBrandId
		JOIN ItemBrand ib ON vb.IrmaBrandId = ib.Brand_ID
		JOIN itemidentifier ii ON vi.scancode = ii.identifier
		JOIN (
			SELECT TOP 1 Package_Desc2,
				Package_Unit_ID,
				item_key
			FROM ItemChangeHistory ich
			ORDER BY ich.Insert_Date DESC
			) ich ON ii.item_key = ich.item_key
		LEFT JOIN TaxClass tc ON substring(vi.TaxClassName, 1, 7) = substring(tc.TaxClassDesc, 1, 7)
		LEFT JOIN NatItemClass ni ON vi.NationalClassCode = ni.ClassID
		LEFT JOIN ItemUnit iu ON vi.RetailUom = iu.Unit_Abbreviation
		WHERE NOT EXISTS (
				SELECT *
				FROM IconItemLastChange lc
				WHERE lc.Identifier = vi.ScanCode
				)
		
		--===================================================================
		-- Update sub team for aligned /  If changed from aligned to non-aligned 
		--===================================================================
		UPDATE lc
		SET Subteam_No = st.SubTeam_No,
			InsertDate = @now
		FROM IconItemLastChange lc
		JOIN @LastChangedItemList di ON lc.Identifier = di.ScanCode
			AND di.SubTeamNotAligned = 0
		JOIN SubTeam st ON di.DeptNo = st.Dept_No

		UPDATE lc
		SET Subteam_No = st.SubTeam_No,
			InsertDate = @now
		FROM IconItemLastChange lc
		JOIN @LastChangedItemList di ON lc.Identifier = di.ScanCode
			AND di.SubTeamNotAligned = 1
		JOIN ItemIdentifier ii ON di.ScanCode = ii.Identifier
		JOIN Item i ON ii.Item_Key = i.Item_Key
			AND i.Retail_Sale = 1
		JOIN SubTeam oldSub ON i.SubTeam_No = oldSub.SubTeam_No
			AND oldSub.AlignedSubTeam = 1
		JOIN SubTeam st ON st.Dept_No = @defaultNonAlignedPosDepNo
	END TRY

	BEGIN CATCH
		DECLARE @err_no INT,
			@err_sev INT,
			@err_msg VARCHAR(MAX)

		SELECT @err_no = ERROR_NUMBER(),
			@err_sev = ERROR_SEVERITY(),
			@err_msg = ERROR_MESSAGE()

		RAISERROR (
				'IconItemAddUpdateLastChange failed with error no: %d and message: %s',
				@err_sev,
				1,
				@err_no,
				@err_msg
				)
	END CATCH
END

GO
