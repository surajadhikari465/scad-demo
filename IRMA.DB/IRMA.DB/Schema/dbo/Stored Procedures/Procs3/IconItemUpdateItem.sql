
CREATE PROCEDURE [dbo].[IconItemUpdateItem]
	-- Add the parameters for the stored procedure here
	@ValidatedItemList dbo.IconUpdateItemType READONLY,
	@UserName varchar(25)
AS
BEGIN
	SET NOCOUNT ON;

/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK	Description
----------------------------------------------------------------------------------------------
MZ      2015-06-23  16200 (9290)    Updated the canonical colums on the ItemOverride table
                                    for alternate jurisdiction update.
CM		2016-01-13					Added Retail Size and Retail UOM
CM		2016-02-08					Added logic to include InstanceDataFlag to check whether or 
									not to update Retail Size and Retail UOM 
JA      2016-06-22	19897 (16243)   Update StoredProc to remove the retail restictions for subteam update
EM		2017-11-22  24309			No longer update Retail Unit when UOM changes 
									from EA to LB or vice versa when region is on GPM
***********************************************************************************************/

	-- =====================================================
	-- Declare Variables
	-- =====================================================
	DECLARE @userId INT;
	DECLARE @now DATETIME;
	DECLARe @distinctList dbo.IconUpdateItemType;
	DECLARE @subTeamAligned VARCHAR(25);
	DECLARE @defaultNonAlignedPosDepNo INT;
	DECLARE @uomIdLb INT;
	DECLARE @uomIdEa INT;
	DECLARE @updateRetailUomSize BIT;
    DECLARE @regionIsOnGpm BIT;
	
	SET @userId = (SELECT u.User_ID FROM Users u WHERE u.UserName = @UserName);
	SET @now = (SELECT GETDATE());
	SET @defaultNonAlignedPosDepNo = 294;
	-- rem CREATE TABLE #tempCategory (Category_ID INT, Category_Name VARCHAR(35), SubTeam_No INT, UserID INT,  SubTeam_Type_ID INT );
	SET @subTeamAligned = 'SubTeam Aligned';
	SET @uomIdLb = (SELECT uom.Unit_ID FROM ItemUnit uom WHERE uom.Unit_Abbreviation = 'LB');
	SET @uomIdEa = (SELECT uom.Unit_ID FROM ItemUnit uom WHERE uom.Unit_Abbreviation = 'EA');
	SET @updateRetailUomSize = (SELECT FlagValue FROM InstanceDataFlags WHERE FlagKey = 'EnableIconRetailUomSizeUpdates');
	SET @regionIsOnGpm = (SELECT FlagValue FROM InstanceDataFlags WHERE FlagKey = 'GlobalPriceManagement');

	INSERT INTO @distinctList
	SELECT DISTINCT *
	FROM @ValidatedItemList vi
	

	-- =====================================================
	-- Update Item
	-- =====================================================
	BEGIN TRY
		SELECT DISTINCT
			vi.*,
			i.SubTeam_No AS IrmaSubTeamNo,
			i.Category_ID AS CategoryId 
		INTO #subTeamDistinctList
		FROM @ValidatedItemList vi
		INNER JOIN ItemIdentifier	ii	ON	vi.ScanCode		= ii.Identifier 
										AND ii.Deleted_Identifier = 0
										AND ii.Default_Identifier = 1
		INNER JOIN Item				i   ON	i.Item_Key		= ii.Item_Key 										
		INNER JOIN SubTeam			ist ON	ist.SubTeam_No	= i.SubTeam_No
		LEFT JOIN SubTeam			st  ON	st.Dept_No		= vi.DeptNo
		WHERE (st.SubTeam_No IS NULL 
			OR ist.SubTeam_No <> st.SubTeam_No) 


		--Add category if new sub team association does not exists..
		INSERT INTO ItemCategory ([Category_Name], [SubTeam_No], [User_ID])
		SELECT DISTINCT @subTeamAligned, st.SubTeam_No, @userId 
		FROM #subTeamDistinctList di 
		JOIN SubTeam st ON di.DeptNo = st.Dept_No AND di.SubTeamNotAligned = 0
		WHERE
		NOT EXISTS (SELECT 1 
					FROM ItemCategory ic
					WHERE ic.[Category_Name] = @subTeamAligned
					AND ic.[SubTeam_No] = st.SubTeam_No)

		--Get item sub team updates
		UPDATE di
		SET
			di.IrmaSubTeamNo = st.SubTeam_No,
			di.CategoryId = ic.Category_ID
		FROM
			#subTeamDistinctList		di 	
			JOIN SubTeam		    st ON di.DeptNo = st.Dept_No
									   AND di.SubTeamNotAligned = 0
			LEFT JOIN ItemCategory  ic ON st.SubTeam_No = ic.SubTeam_No			
									   AND ic.Category_Name = @subTeamAligned

		UPDATE di
		SET
			di.IrmaSubTeamNo = st.SubTeam_No,
			di.CategoryId = ic.Category_ID
		FROM
			#subTeamDistinctList		di
			JOIN ItemIdentifier		ii ON ii.Identifier = di.ScanCode AND di.SubTeamNotAligned = 1
			JOIN Item				i  ON i.Item_Key = ii.Item_Key
			JOIN SubTeam		oldSub ON i.SubTeam_No = oldSub.SubTeam_No AND oldSub.AlignedSubTeam = 1
			JOIN SubTeam		    st ON st.Dept_No = @defaultNonAlignedPosDepNo
			LEFT JOIN ItemCategory  ic ON st.SubTeam_No = ic.SubTeam_No			
									AND ic.Category_Name = @subTeamAligned
		-- =====================================================

		UPDATE i
		SET
			i.Item_Description	= vi.ProductDescription,
			i.POS_Description	= UPPER(vi.PosDescription),
			i.Package_Desc1		= CAST(vi.PackageUnit AS DECIMAL(9,4)),
			i.Food_Stamps		= CAST(vi.FoodStampEligible AS BIT),
			i.Brand_ID			= ib.Brand_ID,
			i.TaxClassID		= ISNULL(tc.TaxClassID, i.TaxClassID),
			i.ClassID			= ISNULL(ni.ClassID, i.ClassID),
			i.SubTeam_No		= CASE WHEN di.IrmaSubTeamNo IS NULL THEN i.SubTeam_No ELSE di.IrmaSubTeamNo END,
			i.Category_ID		= CASE WHEN di.CategoryId IS NULL THEN i.Category_ID ELSE di.CategoryId END,
			i.LastModifiedUser_ID = @userId,
			i.LastModifiedDate	= @now,
			i.Package_Desc2		= CASE @updateRetailUomSize WHEN 1 THEN vi.RetailSize ELSE i.Package_Desc2 END,
			i.Package_Unit_ID	= CASE @updateRetailUomSize WHEN 1 THEN ISNULL(iu.Unit_ID, i.Package_Unit_ID) ELSE i.Package_Unit_ID END,
			i.Retail_Unit_ID	=	CASE
										-- only update Retail_Unit_ID if Package_Unit_ID is changing from LB to Not LB or vice versa
                                        -- and the region is not under GlobalPriceManagement (GPM)
										WHEN 
											(@regionIsOnGpm = 0) AND (@updateRetailUomSize = 1)
											AND ((vi.RetailUom = 'LB' AND pu.Unit_Abbreviation <> 'LB')
												OR (vi.RetailUom <> 'LB' AND pu.Unit_Abbreviation = 'LB'))
											AND ((vi.RetailUom = 'LB' AND ru.Unit_Abbreviation <> 'LB')
												OR (vi.RetailUom <> 'LB' AND ru.Unit_Abbreviation = 'LB')) THEN
													(CASE iu.Unit_ID 
														WHEN @uomIdLb THEN @uomIdLb 
														ELSE @uomIdEa
													END) 
										ELSE
											i.Retail_Unit_ID 
									END
		FROM
			Item i
			JOIN ItemIdentifier		ii ON i.Item_Key = ii.Item_Key
											AND ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1
			JOIN @distinctList		vi ON ii.Identifier = vi.ScanCode
			JOIN ValidatedBrand		vb ON vi.BrandId = vb.IconBrandId
			JOIN ItemBrand			ib ON vb.IrmaBrandId = ib.Brand_ID
			LEFT JOIN TaxClass		tc ON vi.TaxClassName = tc.TaxClassDesc or SUBSTRING(vi.TaxClassName, 1, 7) = SUBSTRING(tc.TaxClassDesc, 1, 7)
			LEFT JOIN NatItemClass	ni ON vi.NationalClassCode = ni.ClassID
			LEFT JOIN ItemUnit		pu ON i.Package_Unit_ID = pu.Unit_ID
			LEFT JOIN ItemUnit 		iu ON vi.RetailUom  = iu.Unit_Abbreviation
			LEFT JOIN ItemUnit		ru ON i.Retail_Unit_ID = ru.Unit_ID
			LEFT JOIN #subTeamDistinctList di ON vi.ScanCode = di.ScanCode

	
	-- =====================================================
	-- Update ItemOverride if exists for regions use alternate jurisdiction
	-- =====================================================
		UPDATE i
		SET
			i.Item_Description	  = vi.ProductDescription,
			i.POS_Description	  = UPPER(vi.PosDescription),
			i.Package_Desc1		  = CAST(vi.PackageUnit AS DECIMAL(9,4)),
			i.Food_Stamps		  = CAST(vi.FoodStampEligible AS BIT),
			i.Brand_ID			  = ib.Brand_ID,
			i.LastModifiedUser_ID = @userId
		FROM
			ItemOverride i
			JOIN ItemIdentifier		ii ON i.Item_Key = ii.Item_Key
											AND ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1
			JOIN @distinctList		vi ON ii.Identifier = vi.ScanCode
			JOIN ValidatedBrand		vb ON vi.BrandId = vb.IconBrandId
			JOIN ItemBrand			ib ON vb.IrmaBrandId = ib.Brand_ID 
			
		-- Update Price.PosTare if there is a value from Icon but there is no value already in the Price table
		-- Get Price rows that need an update
		SELECT
			p.Item_Key,
			p.Store_No,
			vi.Tare
		INTO #priceNeedsTare
		FROM 
			Price					p
			JOIN Item				i	ON p.Item_Key = i.Item_Key
			JOIN ItemIdentifier		ii	ON i.Item_Key = ii.Item_Key
										   AND ii.Deleted_Identifier = 0
										   AND ii.Default_Identifier = 1
			JOIN @ValidatedItemList	vi	ON ii.Identifier = vi.ScanCode
		WHERE
			(p.PosTare IS NULL OR p.PosTare = 0)
			AND vi.Tare IS NOT NULL
			AND CAST(ROUND(CAST(vi.Tare AS DECIMAL(9,4)),0) AS INT) > 0

		

		-- Update Price rows
		UPDATE p
		SET
			p.PosTare = CAST(ROUND(CAST(pnt.Tare AS DECIMAL(9,4)),0) AS INT),
			p.LastScannedUserId_NonDTS = @userId
		FROM
			Price p
			JOIN #priceNeedsTare pnt ON p.Item_Key = pnt.Item_Key
										   AND p.Store_No = pnt.Store_No

	END TRY
	BEGIN CATCH
		DECLARE @err_no INT, @err_sev INT, @err_msg VARCHAR(MAX)
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
		RAISERROR ('IconItemUpdateItem failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
	END CATCH

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IconItemUpdateItem] TO [IConInterface]
    AS [dbo];

