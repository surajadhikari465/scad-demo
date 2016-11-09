
CREATE PROCEDURE [dbo].[IconItemAddUpdateLastChange]
	-- Add the parameters for the stored procedure here
	@LastChangedItemList dbo.IconLastChangedItemType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	-- =====================================================
	-- Declare Variables
	-- =====================================================
	DECLARE @now datetime;
	DECLARE @defaultNonAlignedPosDepNo int;
	set @defaultNonAlignedPosDepNo = 294;
	SET @now = (SELECT GETDATE());
	DECLARE @updateRetailUomSize bit;
	SET @updateRetailUomSize = (SELECT FlagValue FROM InstanceDataFlags WHERE FlagKey = 'EnableIconRetailUomSizeUpdates');
	DECLARE @Identifier varchar(13);
	SET @identifier = (SELECT vi.ScanCode FROM @LastChangedItemList vi);
	DECLARE @irmaLastPackageDesc2 decimal(9,4);
	SET @irmaLastPackageDesc2 = (SELECT TOP 1 Package_Desc2 FROM ItemChangeHistory ich INNER JOIN ItemIdentifier ii ON ich.Item_Key = ii.Item_Key WHERE ii.Identifier = @identifier ORDER BY ich.Insert_Date DESC);
	DECLARE @irmaLastPackageUnitId int;
	SET @irmaLastPackageUnitId = (SELECT TOP 1 Package_Unit_ID FROM ItemChangeHistory ich INNER JOIN ItemIdentifier ii ON ich.Item_Key = ii.Item_Key WHERE ii.Identifier = @identifier ORDER BY ich.Insert_Date DESC);
	-- =====================================================
	-- IconItemLastChange
	-- =====================================================
	BEGIN TRY
		-- Update existing rows
		UPDATE lc
		SET
			Item_Description	= vi.ProductDescription,
			POS_Description		= vi.PosDescription,
			Package_Desc1		= CAST(vi.PackageUnit as decimal(9,4)),
			Food_Stamps			= CAST(vi.FoodStampEligible as bit),
			ScaleTare			= CAST(vi.Tare as decimal(9,4)),
			Brand_ID			= ib.Brand_ID,
			TaxClassID			= tc.TaxClassID,
			AreNutriFactsChanged = vi.AreNutriFactsUpdated,
			ClassID		 = ni.ClassID,
			InsertDate			= @now,
			Package_Unit_ID		= case @updateRetailUomSize when 1 then iu.Unit_ID else Package_Unit_ID end,
			Package_Desc2       = case @updateRetailUomSize when 1 then vi.RetailSize else Package_Desc2 end
		FROM
			IconItemLastChange		lc
			JOIN @LastChangedItemList vi on lc.Identifier = vi.ScanCode
			JOIN TaxClass			tc on substring(vi.TaxClassName, 1, 7) = substring(tc.TaxClassDesc, 1, 7)
			JOIN ValidatedBrand		vb on vi.BrandId = vb.IconBrandId
			JOIN ItemBrand			ib on vb.IrmaBrandId = ib.Brand_ID
			JOIN NatItemClass		ni on vi.NationalClassCode = ni.ClassID
			JOIN ItemUnit			iu on vi.RetailUom = iu.Unit_Abbreviation

		-- Insert new rows if they don't exist yet
		INSERT INTO IconItemLastChange
		SELECT
			vi.ScanCode										as Identifier,
			NULL											as Subteam_No,
			ib.Brand_ID										as Brand_ID,
			vi.ProductDescription							as Item_Description,
			vi.PosDescription								as POS_Description,
			CAST(vi.PackageUnit as decimal(9,4))			as Package_Desc1,
			CAST(vi.FoodStampEligible as bit)				as Food_Stamps,
			CAST(vi.Tare as decimal(9,4))				as ScaleTare,
			tc.TaxClassID									as TaxClassID,
			@now											as InsertDate,
			vi.AreNutriFactsUpdated						as AreNutriFactsChanged,
			ni.ClassID										as ClassID,
			case @updateRetailUomSize when 1 then iu.Unit_ID else @irmaLastPackageUnitId  end as Package_Unit_ID,
			case @updateRetailUomSize when 1 then vi.RetailSize else @irmaLastPackageDesc2 end as Package_Desc2
		FROM
			@LastChangedItemList	vi
			JOIN TaxClass		tc on substring(vi.TaxClassName, 1, 7) = substring(tc.TaxClassDesc, 1, 7)
			JOIN ValidatedBrand vb on vi.BrandId		= vb.IconBrandId
			JOIN ItemBrand		ib on vb.IrmaBrandId	= ib.Brand_ID
			JOIN NatItemClass		ni on vi.NationalClassCode = ni.ClassID
			JOIN ItemUnit			iu on vi.RetailUom = iu.Unit_Abbreviation
		WHERE NOT EXISTS (SELECT * FROM IconItemLastChange lc WHERE lc.Identifier = vi.ScanCode)

		--===================================================================
		-- Update sub team for aligned /  If changed from aligned to non-aligned 
		--===================================================================
		UPDATE lc
		SET
			Subteam_No			= st.SubTeam_No,
			InsertDate			= @now
		FROM
			IconItemLastChange		lc
			JOIN @LastChangedItemList			di on lc.Identifier = di.ScanCode and di.SubTeamNotAligned = 0			
			JOIN SubTeam				st on	di.DeptNo = st.Dept_No

		UPDATE lc
		SET		
			Subteam_No			= st.SubTeam_No,
			InsertDate			= @now
		FROM
			IconItemLastChange			lc
			JOIN @LastChangedItemList			di on lc.Identifier = di.ScanCode and di.SubTeamNotAligned = 1
			JOIN ItemIdentifier			ii on	di.ScanCode = ii.Identifier
			JOIN Item					i on ii.Item_Key = i.Item_Key and i.Retail_Sale = 1		
			JOIN SubTeam			oldSub on   i.SubTeam_No = oldSub.SubTeam_No and oldSub.AlignedSubTeam = 1
			JOIN SubTeam				st on	st.Dept_No = @defaultNonAlignedPosDepNo


	END TRY
	BEGIN CATCH
		DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
		RAISERROR ('IconItemAddUpdateLastChange failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
	END CATCH

END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IconItemAddUpdateLastChange] TO [IConInterface]
    AS [dbo];

